using System.Linq.Expressions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Interfaces;
using RoomService.Models;

namespace RoomService.Services;

public class RoomService : IRoomService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RoomService> _logger;
    private readonly IDistributedCache _distributedCache;
    public RoomService(AppDbContext dbContext, IDistributedCache distributedCache, ILogger<RoomService> logger)
    {
        _dbContext = dbContext;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<Result<Guid>> CreateAsync(CreateRoomDto dto, CancellationToken ct)
    {
        var roomForCreate = new Room
        {
            Id = Guid.NewGuid(),
            Number = dto.Number,
            HotelBranchId = dto.HotelBranchId,
            IsDisabled = dto.IsDisabled,
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
        roomForUpdate.IsDisabled = dto.IsDisabled;
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
    public async Task<Result> EnableRoomAsync(Guid roomId, CancellationToken ct)
    {
        var roomToUpdate = await _dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId, ct);
        if (roomToUpdate == null)
            return Result.Fail("Room not found");

        roomToUpdate.Enable();

        _dbContext.Update(roomToUpdate);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Room)} with Id {roomId} has been enabled.");

        return Result.Ok();
    }

    public async Task<Result> DisableRoomAsync(Guid roomId, CancellationToken ct)
    {
        var roomToUpdate = await _dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId, ct);
        if (roomToUpdate == null)
            return Result.Fail("Room not found");

        roomToUpdate.Enable();

        _dbContext.Update(roomToUpdate);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Room)} with Id {roomId} has been disabled.");

        return Result.Ok();
    }

    public async Task<Result<IEnumerable<RoomResponseDto>>> GetAllAsync(CancellationToken ct)
    {
        const string keyForRedisCache = "rooms";
        var cachedRooms = await _distributedCache.GetStringAsync(keyForRedisCache, ct);

        if (!string.IsNullOrEmpty(cachedRooms))
        {
            var deserializedRooms = JsonConvert.DeserializeObject<IEnumerable<RoomResponseDto>>(cachedRooms);
            _logger.LogInformation($"Retrieved {deserializedRooms.Count()} Rooms from cache");
            return Result.Ok(deserializedRooms);
        }

        var rooms = await _dbContext
            .Rooms
            .Include(r => r.Type.Amenities)
            .Select(MapToRoomResponseDto)
            .ToListAsync(ct);

        if (rooms.Any())
        {
            await _distributedCache.SetStringAsync(keyForRedisCache, JsonConvert.SerializeObject(rooms), ct);
            _logger.LogInformation($"Cached list of Rooms");
        }

        _logger.LogInformation($"Retrieved {rooms.Count()} Rooms");

        return Result.Ok<IEnumerable<RoomResponseDto>>(rooms);
    }


    public async Task<Result<IEnumerable<RoomResponseDto>>> GetRoomsByTypeAsync(Guid roomTypeId, CancellationToken ct)
    {
        var keyForRedisCache = $"rooms_by_type_{roomTypeId.ToString()}";
        var cachedRooms = await _distributedCache.GetStringAsync(keyForRedisCache, ct);

        if (!string.IsNullOrEmpty(cachedRooms))
        {
            var deserializedRooms = JsonConvert.DeserializeObject<IEnumerable<RoomResponseDto>>(cachedRooms);
            _logger.LogInformation($"Retrieved {deserializedRooms.Count()} Rooms by Type from cache");
            return Result.Ok(deserializedRooms);
        }

        var rooms = await _dbContext
            .Rooms
            .Where(r => r.Type.Id == roomTypeId)
            .Select(MapToRoomResponseDto)
            .ToListAsync(ct);

        if (rooms.Any())
        {
            await _distributedCache.SetStringAsync(keyForRedisCache, JsonConvert.SerializeObject(rooms), ct);
            _logger.LogInformation($"Cached list of Rooms by Type");
        }

        _logger.LogInformation($"Retrieved {rooms.Count()} Rooms by Type");

        return Result.Ok<IEnumerable<RoomResponseDto>>(rooms);
    }

    
    private static readonly Expression<Func<Room, RoomResponseDto>> MapToRoomResponseDto = r => new RoomResponseDto
    {
        Id = r.Id,
        Number = r.Number,
        Type = r.Type,
        IsDisabled = r.IsDisabled
    };
}