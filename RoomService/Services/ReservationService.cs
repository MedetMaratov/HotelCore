using System.Linq.Expressions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Enums;
using RoomService.Interfaces;
using RoomService.Models;

namespace RoomService.Services;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _dbContext;
    private readonly IOccupiedRoomService _occupiedRoomService;
    private readonly IRoomFinderService _roomFinderService;
    private readonly IGuestService _guestService;
    private readonly IReservatorService _reservatorService;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ReservationService> _logger;
    private const string KeyForRedisCache = "reservations";


    public ReservationService(
        AppDbContext dbContext,
        IOccupiedRoomService occupiedRoomService,
        IRoomFinderService roomFinderService,
        IGuestService guestService,
        IReservatorService reservatorService,
        IDistributedCache distributedCache,
        ILogger<ReservationService> logger)
    {
        _dbContext = dbContext;
        _occupiedRoomService = occupiedRoomService;
        _roomFinderService = roomFinderService;
        _guestService = guestService;
        _reservatorService = reservatorService;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<ResponseReservationDto>>> GetAllAsync(CancellationToken ct)
    {
        IEnumerable<ResponseReservationDto> reservations;

        var cashedReservations = await _distributedCache.GetStringAsync(KeyForRedisCache, ct);
        if (string.IsNullOrEmpty(cashedReservations))
        {
            reservations = await _dbContext
                .Reservations
                .Select(MapToResponseReservationDto)
                .ToListAsync(ct);

            if (reservations.Any())
            {
                await _distributedCache.SetStringAsync(KeyForRedisCache, JsonConvert.SerializeObject(reservations), ct);
                _logger.LogInformation($"Cashed list of reservations");
            }

            return Result.Ok(reservations);
        }

        reservations = JsonConvert.DeserializeObject<IEnumerable<ResponseReservationDto>>(cashedReservations);
        return Result.Ok(reservations);
    }

    public async Task<Result<IEnumerable<ResponseReservationDto>>> GetAllByHotelBranchIdAsync(Guid hotelBranchId,
        CancellationToken ct)
    {
        var reservations = await _dbContext
            .Reservations
            .Where(r => r.HotelBranchId == hotelBranchId)
            .Select(MapToResponseReservationDto)
            .ToListAsync(ct);

        return Result.Ok<IEnumerable<ResponseReservationDto>>(reservations);
    }


    public async Task<Result<Guid>> ReserveAsync(CreateReservationDto createReservationDto, CancellationToken ct)
    {
        createReservationDto.RoomForReserveDtos.ReservationStart = DateTime.SpecifyKind(createReservationDto.RoomForReserveDtos.ReservationStart, DateTimeKind.Utc);
        createReservationDto.RoomForReserveDtos.ReservationEnd = DateTime.SpecifyKind(createReservationDto.RoomForReserveDtos.ReservationEnd, DateTimeKind.Utc);

        var reservator = new Reservator()
        {
            Id = Guid.NewGuid(),
            FullName = createReservationDto.ReservatorFullName,
            ContactEmail = createReservationDto.ReservatorContactEmail,
            ContactPhoneNumber = createReservationDto.ReservatorContactPhoneNumber
        };
        await _dbContext.Reservators.AddAsync(reservator, ct);
        var reservation = CreateReservationFromDto(createReservationDto);
        reservation.ReservationCreatorId = reservator.Id;

        var room = await _dbContext
            .Rooms
            .Where(r => r.Type.Id == createReservationDto.RoomForReserveDtos.RoomTypeId)
            .FirstOrDefaultAsync(r => !_dbContext.OccupiedRooms
                .Any(or =>
                    or.Reservation.DateIn.CompareTo(createReservationDto.RoomForReserveDtos.ReservationStart) >= 0 &&
                    or.Reservation.DateOut.CompareTo(createReservationDto.RoomForReserveDtos.ReservationEnd) <= 0), ct);

        if (room.Id == Guid.Empty)
            return Result.Fail("Not enough available rooms");
        
        var occupiedRoomDto = new CreateOccupiedRoomDto()
        {
            ReservationId = reservation.Id,
            RoomId = room.Id
        };
        foreach (var guestName in createReservationDto.GuestNames)
        {
            occupiedRoomDto.RoomId = room.Id;
            var guest = new Guest()
            {
                Id = Guid.NewGuid(),
                FullName = guestName
            };
            await _dbContext.Guests.AddAsync(guest, ct);

            var occupiedRoom = _occupiedRoomService.Create(occupiedRoomDto, ct);
            occupiedRoom.GuestId = guest.Id;
            reservation.OccupiedRooms.Add(occupiedRoom);
            _logger.LogInformation($"{nameof(OccupiedRoom)} created with Id: {occupiedRoom.Id}");
        }

        await _dbContext.AddAsync(reservation, ct);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Reservation)} with Id: {reservation.Id} completed successfully");
        await _distributedCache.RemoveAsync(KeyForRedisCache, ct);
        return Result.Ok(reservation.Id);
    }

    private static Reservation CreateReservationFromDto(CreateReservationDto createReservationDto)
    {
        return new Reservation()
        {
            Id = Guid.NewGuid(),
            DateIn = createReservationDto.DateIn.ToUniversalTime(),
            DateOut = createReservationDto.DateOut.ToUniversalTime(),
            MadeBy = createReservationDto.ReservationMethod,
            Status = ReservationStatus.Waiting,
            OccupiedRooms = new List<OccupiedRoom>(),
            HotelBranchId = createReservationDto.HotelBranchId
        };
    }

    private static readonly Expression<Func<Reservation, ResponseReservationDto>> MapToResponseReservationDto = r =>
        new ResponseReservationDto
        {
            Id = r.Id,
            DateIn = r.DateIn,
            DateOut = r.DateOut,
            MadeBy = r.MadeBy,
            Status = r.Status.ToString(),
            HotelBranchId = r.HotelBranchId,
            HotelBranch = r.HotelBranch,
            ReservationCreatorId = r.ReservationCreatorId,
        };
}