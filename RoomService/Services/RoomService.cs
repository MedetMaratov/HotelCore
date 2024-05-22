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
        var existingRoom =
            await _dbContext.Rooms.AsNoTracking().SingleOrDefaultAsync(
                r => r.HotelBranchId == dto.HotelBranchId && r.Number == dto.Number, ct);
        if (existingRoom != null)
        {
            return Result.Fail("Room with same number exist in this branch");
        }
        
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
        await _distributedCache.RemoveAsync("rooms", ct);
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
        roomForUpdate.TypeId = dto.TypeId;

        _dbContext.Update(roomForUpdate);
        await _dbContext.SaveChangesAsync(ct);
        await _distributedCache.RemoveAsync("rooms", ct);

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
        await _distributedCache.RemoveAsync("rooms", ct);

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
        await _distributedCache.RemoveAsync("rooms", ct);

        _logger.LogInformation($"{nameof(Room)} with Id {roomId} has been enabled.");

        return Result.Ok();
    }

    public async Task<Result> DisableRoomAsync(Guid roomId, CancellationToken ct)
    {
        var roomToUpdate = await _dbContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId, ct);
        if (roomToUpdate == null)
            return Result.Fail("Room not found");

        roomToUpdate.Disable();

        _dbContext.Update(roomToUpdate);
        await _dbContext.SaveChangesAsync(ct);
        await _distributedCache.RemoveAsync("rooms", ct);

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
            .OrderBy(r => r.Number)
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
        var rooms = await _dbContext
            .Rooms
            .Where(r => r.Type.Id == roomTypeId)
            .Select(MapToRoomResponseDto)
            .OrderBy(r => r.Number)
            .ToListAsync(ct);

        _logger.LogInformation($"Retrieved {rooms.Count()} Rooms by Type");

        return Result.Ok<IEnumerable<RoomResponseDto>>(rooms);
    }

    public async Task<Result<IEnumerable<RoomResponseDto>>> GetRoomsByHotelBranchAsync(Guid hotelBranchId,
        CancellationToken ct)
    {
        var rooms = await _dbContext
            .Rooms
            .Where(r => r.HotelBranchId == hotelBranchId)
            .OrderBy(r => r.Number)
            .Select(MapToRoomResponseDto)
            .ToListAsync(ct);


        _logger.LogInformation($"Retrieved {rooms.Count()} Rooms by hotel branch");

        return Result.Ok<IEnumerable<RoomResponseDto>>(rooms);
    }

    public async Task<Result<IEnumerable<RoomResponseDto>>> GetRoomsByHotelBranchAndNumberAsync(Guid hotelBranchId, string number, CancellationToken ct)
    {
        var rooms = await _dbContext
            .Rooms
            .Where(r => r.HotelBranchId == hotelBranchId && r.Number == number)
            .OrderBy(r => r.Number)
            .Select(MapToRoomResponseDto)
            .ToListAsync(ct);


        _logger.LogInformation($"Retrieved {rooms.Count()} Rooms by hotel branch and number");

        return Result.Ok<IEnumerable<RoomResponseDto>>(rooms);
    }

    public async Task<Result<RoomDetailsDto>> GetRoomDetailsByIdAsync(Guid id, CancellationToken ct)
    {
        var isOccupied = _dbContext.OccupiedRooms.Select(or => or.RoomId).Contains(id);

        var room = await _dbContext
            .Rooms
            .Select(r => new RoomDetailsDto()
            {
                Id = r.Id,
                Number = r.Number,
                RoomTypeId = r.TypeId,
                RoomTypeTitle = r.Type.Title,
                HotelBranchId = r.HotelBranchId,
                HotelBranchTitle = r.HotelBranch.Name,
                IsOccupied = isOccupied,
                IsDisabled = r.IsDisabled
            })
            .SingleOrDefaultAsync(r => r.Id == id, ct);
        return room is null ? Result.Fail("Room not found") : Result.Ok(room);
    }


    private static readonly Expression<Func<Room, RoomResponseDto>> MapToRoomResponseDto = r => new RoomResponseDto
    {
        Id = r.Id,
        Number = r.Number,
        RoomTypeId = r.TypeId,
        RoomTypeTitle = r.Type.Title,
        HotelBranchId = r.HotelBranchId,
        HotelBranchTitle = r.HotelBranch.Name,
        IsDisabled = r.IsDisabled
    };
}