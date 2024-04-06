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
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        AppDbContext dbContext,
        IOccupiedRoomService occupiedRoomService,
        IRoomFinderService roomFinderService,
        IDistributedCache distributedCache,
        ILogger<ReservationService> logger)
    {
        _dbContext = dbContext;
        _occupiedRoomService = occupiedRoomService;
        _roomFinderService = roomFinderService;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<ResponseReservationDto>>> GetAllAsync(CancellationToken ct)
    {        
        IEnumerable<ResponseReservationDto> reservations;
        const string keyForRedisCache = "reservations";
        
        var cashedReservations = await _distributedCache.GetStringAsync(keyForRedisCache, ct);
        if (string.IsNullOrEmpty(cashedReservations))
        {
            reservations = await _dbContext
                .Reservations
                .Select(MapToResponseReservationDto)
                .ToListAsync(ct);
            
            if (reservations.Any())
            {
                await _distributedCache.SetStringAsync(keyForRedisCache, JsonConvert.SerializeObject(reservations), ct);
                _logger.LogInformation($"Cashed list of reservations");
            }        
            
            return Result.Ok(reservations);
        }

        reservations = JsonConvert.DeserializeObject<IEnumerable<ResponseReservationDto>>(cashedReservations);
        return Result.Ok(reservations);
    }

    public async Task<Result<Guid>> ReserveAsync(CreateReservationDto createReservationDto, CancellationToken ct)
    {
        var reservation = CreateReservationFromDto(createReservationDto);
        var roomIdsForReserve = new List<Guid>();
        foreach (var roomsForReserveDto in createReservationDto.RoomsForReserveDtos)
        {
            var roomIds = await _roomFinderService.GetRoomIdsForReservationAsync(roomsForReserveDto, ct);
            if (roomIds.IsFailed)
                return Result.Fail("Not enough available rooms");
            roomIdsForReserve.AddRange(roomIds.Value);
        }

        var occupiedRoomDto = new CreateOccupiedRoomDto()
        {
            ReservationId = reservation.Id
        };

        foreach (var id in roomIdsForReserve)
        {
            occupiedRoomDto.RoomId = id;
            var occupiedRoom = _occupiedRoomService.Create(occupiedRoomDto, ct);
            reservation.OccupiedRooms.Add(occupiedRoom);
            _logger.LogInformation($"{nameof(OccupiedRoom)} created with Id: {occupiedRoom.Id}");
        }

        await _dbContext.AddAsync(reservation, ct);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Reservation)} with Id: {reservation.Id} completed successfully");

        return Result.Ok(reservation.Id);
    }

    private static Reservation CreateReservationFromDto(CreateReservationDto createReservationDto)
    {
        return new Reservation()
        {
            Id = Guid.NewGuid(),
            DateIn = createReservationDto.DateIn,
            DateOut = createReservationDto.DateOut,
            ReservationCreatorId = createReservationDto.ReservationCreatorId,
            MadeBy = createReservationDto.ReservationMethod,
            Status = ReservationStatus.Waiting,
            OccupiedRooms = new List<OccupiedRoom>(),
            HotelBranchId = createReservationDto.HotelBranchId
        };
    }
    
    private static readonly Expression<Func<Reservation, ResponseReservationDto>> MapToResponseReservationDto = r => new ResponseReservationDto
    {
        Id = r.Id,
        DateIn = r.DateIn,
        DateOut = r.DateOut,
        MadeBy = r.MadeBy,
        Status = r.Status,
        HotelBranchId = r.HotelBranchId,
        HotelBranch = r.HotelBranch,
        ReservationCreatorId = r.ReservationCreatorId
    };
}