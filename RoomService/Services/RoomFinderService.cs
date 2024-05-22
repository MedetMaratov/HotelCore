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

    public Guid GetRoomIdsForReservationAsync(RoomForReserveDto roomForReserveDto)
    {
        var room = _dbContext
            .Rooms
            .Where(r => r.Type.Id == roomForReserveDto.RoomTypeId)
            .FirstOrDefaultAsync(r => !_dbContext.OccupiedRooms
                .Any(or => or.Reservation.DateIn.CompareTo(roomForReserveDto.ReservationStart) >= 0 &&
                           or.Reservation.DateOut.CompareTo(roomForReserveDto.ReservationEnd) <= 0));

        return room.Result.Id;
    }
}