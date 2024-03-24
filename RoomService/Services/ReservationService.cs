using FluentResults;
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
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        AppDbContext dbContext, 
        IOccupiedRoomService occupiedRoomService,
        ILogger<ReservationService> logger)
    {
        _dbContext = dbContext;
        _occupiedRoomService = occupiedRoomService;
        _logger = logger;
    }

    public async Task<Result<Guid>> ReserveAsync(CreateReservationDto createReservationDto, CancellationToken ct)
    {
        var reservation = CreateReservationFromDto(createReservationDto);
        var occupiedRoomDto = new CreateOccupiedRoomDto()
        {
            ReservationId = reservation.Id
        };
        foreach (var id in createReservationDto.roomIdsForReserve)
        {
            occupiedRoomDto.RoomId = id;
            var occupiedRoomResult = await _occupiedRoomService.CreateAsync(occupiedRoomDto, ct);
            reservation.OccupiedRooms.Add(occupiedRoomResult.Value);
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
}