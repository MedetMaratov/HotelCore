using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Enums;
using RoomService.Interfaces;
using RoomService.Models;

namespace RoomService.Services;

public class OccupiedRoomService : IOccupiedRoomService
{
    private readonly AppDbContext _dbContext;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<OccupiedRoomService> _logger;
    
    public OccupiedRoomService(AppDbContext dbContext, IDistributedCache distributedCache, ILogger<OccupiedRoomService> logger)
    {
        _dbContext = dbContext;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public OccupiedRoom Create(CreateOccupiedRoomDto occupiedRoomDto, CancellationToken ct)
    {
        var occupiedRoom = new OccupiedRoom()
        {
            Id = Guid.NewGuid(),
            RoomId = occupiedRoomDto.RoomId,
            ReservationId = occupiedRoomDto.ReservationId,
            CheckIn = null,
            CheckOut = null,
            Status = OccupiedRoomStatus.Expected,
        };
        
        return occupiedRoom;
    }

    public async Task<Result<Guid>> SetCheckInAsync(Guid roomId, CancellationToken ct)
    {
        var room = await _dbContext
            .OccupiedRooms
            .SingleOrDefaultAsync(r => r.Id == roomId, ct);

        if (room == null)
            return Result.Fail("Room not find");

        room.CheckIn = DateTime.Now.ToUniversalTime();
        room.Status = OccupiedRoomStatus.Reside;

        _dbContext.Update(room);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"Check-In set for {nameof(OccupiedRoom)} with Id: {room.Id}");

        return Result.Ok(room.Id);
    }

    public async Task<Result<Guid>> SetCheckOutAsync(Guid roomId, CancellationToken ct)
    {
        var room = await _dbContext
            .OccupiedRooms
            .SingleOrDefaultAsync(r => r.Id == roomId, ct);

        if (room == null)
            return Result.Fail("Room not find");

        room.CheckOut = DateTime.Now.ToUniversalTime();
        room.Status = OccupiedRoomStatus.Left;

        _dbContext.Update(room);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"Check-Out set for OccupiedRoom with Id: {room.Id}");

        return Result.Ok(room.Id);
    }
    
    public async Task<Result<IEnumerable<ResponseOccupiedRoomDto>>> GetOccupiedRoomsByHotelBranchAsync(
        Guid hotelBranchId, CancellationToken ct)
    {
        var rooms = await _dbContext
            .OccupiedRooms
            .Where(or => or.Room.HotelBranchId == hotelBranchId)
            .OrderBy(or => or.Reservation.DateIn)
            .Select(or => new ResponseOccupiedRoomDto()
            {
                Id = or.Id,
                HotelBranchId = or.Reservation.HotelBranchId,
                RoomId = or.Room.Id,
                RoomNumber = or.Room.Number
            })
            .ToListAsync(ct);


        _logger.LogInformation($"Retrieved {rooms.Count()} Rooms by hotel branch");

        return Result.Ok<IEnumerable<ResponseOccupiedRoomDto>>(rooms);
    }
}