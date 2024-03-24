using FluentResults;
using Microsoft.EntityFrameworkCore;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Interfaces;
using RoomService.Models;

namespace RoomService.Services;

public class RoomService : IRoomService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RoomService> _logger;

    public RoomService(AppDbContext dbContext, ILogger<RoomService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid>> CreateAsync(CreateRoomDto dto, CancellationToken ct)
    {
        var roomForCreate = new Room
        {
            Id = Guid.NewGuid(),
            Number = dto.Number,
            HotelBranchId = dto.HotelBranchId,
            TypeId = dto.TypeId
        };

        await _dbContext.Rooms.AddAsync(roomForCreate, ct);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Room)} created with Id: {roomForCreate.Id}");

        return Result.Ok(roomForCreate.Id);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateRoomDto dto, CancellationToken ct)
    {
        var roomForUpdate = await _dbContext
            .Rooms
            .SingleOrDefaultAsync(r => r.Id == dto.Id, ct);
        if (roomForUpdate == null)
            return Result.Fail("Room not found");

        roomForUpdate.Number = dto.Number;
        roomForUpdate.Type = dto.Type;

        _dbContext.Update(roomForUpdate);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Room)} updated with Id: {roomForUpdate.Id}");

        return Result.Ok(roomForUpdate.Id);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid roomId, CancellationToken ct)
    {
        var roomForDelete = await _dbContext
            .Rooms
            .SingleOrDefaultAsync(r => r.Id == roomId, ct);

        if (roomForDelete == null)
            return Result.Fail("Room not found");

        _dbContext.Rooms.Remove(roomForDelete);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Room)} deleted with Id: {roomId}");

        return Result.Ok(roomForDelete.Id);
    }

    public async Task<Result<IEnumerable<RoomResponseDto>>> GetAllAsync(CancellationToken ct)
    {
        var rooms = await _dbContext
            .Rooms
            .Include(r => r.Type.Amenities)
            .Select(r =>
                new RoomResponseDto()
                {
                    Id = r.Id,
                    Number = r.Number,
                    Type = r.Type,
                })
            .ToListAsync(ct);

        _logger.LogInformation($"Retrieved {rooms.Count} Rooms");

        return Result.Ok<IEnumerable<RoomResponseDto>>(rooms);
    }

    public async Task<Result<IEnumerable<RoomResponseDto>>> GetRoomsByTypeAsync(Guid roomTypeId, CancellationToken ct)
    {
        var rooms = await _dbContext
            .Rooms
            .Where(r => r.Type.Id == roomTypeId)
            .Select(r => new RoomResponseDto()
            {
                Id = r.Id,
                Number = r.Number,
                Type = r.Type,
                HotelBranchId = r.HotelBranchId
            })
            .ToListAsync(ct);

        _logger.LogInformation($"Retrieved {rooms.Count} Rooms by Type");

        return Result.Ok<IEnumerable<RoomResponseDto>>(rooms);
    }
}