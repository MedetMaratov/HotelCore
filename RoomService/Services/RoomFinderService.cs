using FluentResults;
using Microsoft.EntityFrameworkCore;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Interfaces;

namespace RoomService.Services;

public class RoomFinderService : IRoomFinderService
{
    private readonly AppDbContext _dbContext;

    public RoomFinderService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IEnumerable<Guid>>> GetRoomIdsForReservationAsync(RoomsForReserveDto roomsForReserveDto,
        CancellationToken ct)
    {
        var roomIds = await _dbContext
            .Rooms
            .Where(r => r.Type.Id == roomsForReserveDto.RoomTypeId &&
                        !_dbContext.OccupiedRooms
                            .Where(or =>
                                or.Reservation.DateIn.CompareTo(roomsForReserveDto.ReservationStart) >= 0 &&
                                or.Reservation.DateOut.CompareTo(roomsForReserveDto.ReservationEnd) <= 0)
                            .Select(or => or.RoomId)
                            .Contains(r.Id))
            .Take(roomsForReserveDto.Quantity)
            .Select(r => r.Id)
            .ToListAsync(ct);

        if (roomIds.Count < roomsForReserveDto.Quantity)
        {
            return Result.Fail<IEnumerable<Guid>>("Not enough available rooms");
        }

        return Result.Ok<IEnumerable<Guid>>(roomIds);
    }
}