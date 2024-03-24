using FluentResults;
using Microsoft.EntityFrameworkCore;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Enums;
using RoomService.Exceptions;
using RoomService.Interfaces;
using RoomService.Models;

namespace RoomService.Services;

public class OccupiedRoomService : IOccupiedRoomService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<OccupiedRoomService> _logger;


    public OccupiedRoomService(AppDbContext dbContext, ILogger<OccupiedRoomService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<OccupiedRoom>> CreateAsync(CreateOccupiedRoomDto occupiedRoomDto, CancellationToken ct)
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

        await _dbContext.OccupiedRooms.AddAsync(occupiedRoom, ct);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(OccupiedRoom)} created with Id: {occupiedRoom.Id}");

        return Result.Ok(occupiedRoom);
    }

    public async Task<Result<Guid>> SetCheckInAsync(Guid roomId, CancellationToken ct)
    {
        var room = await _dbContext
            .OccupiedRooms
            .SingleOrDefaultAsync(r => r.Id == roomId, ct);

        if (room == null)
            return Result.Fail("Room not find");

        room.CheckIn = DateTime.Now;
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

        room.CheckOut = DateTime.Now;
        room.Status = OccupiedRoomStatus.Left;

        _dbContext.Update(room);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"Check-Out set for OccupiedRoom with Id: {room.Id}");

        return Result.Ok(room.Id);
    }
}