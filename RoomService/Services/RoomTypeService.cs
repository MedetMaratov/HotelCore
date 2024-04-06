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

public class RoomTypeService : IRoomTypeService
{
    private readonly AppDbContext _dbContext;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<RoomTypeService> _logger;

    public RoomTypeService(AppDbContext dbContext, IDistributedCache distributedCache, ILogger<RoomTypeService> logger)
    {
        _dbContext = dbContext;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<Result<Guid>> CreateAsync(CreateRoomTypeDto roomTypeDto, CancellationToken ct)
    {
        var roomType = new RoomType
        {
            Id = Guid.NewGuid(),
            Title = roomTypeDto.Title,
            Images = roomTypeDto.Images,
            NightlyRate = roomTypeDto.NightlyRate,
            Description = roomTypeDto.Description,
            MaxCapacity = roomTypeDto.MaxCapacity,
        };

        var amenitiesForRoomType = await _dbContext
            .Amenities
            .Where(a => roomTypeDto.Amenities.Contains(a.Id))
            .ToListAsync(ct);
        roomType.Amenities = amenitiesForRoomType;

        await _dbContext.AddAsync(roomType, ct);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(RoomType)} created with Id: {roomType.Id}");

        return Result.Ok(roomType.Id);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateRoomTypeDto updateDto, CancellationToken ct)
    {
        var roomTypeForUpdate = await _dbContext
            .RoomsTypes
            .SingleOrDefaultAsync(rt => rt.Id == updateDto.Id, ct);

        if (roomTypeForUpdate == null)
            return Result.Fail("Room type not found");

        UpdateRoomType(roomTypeForUpdate, updateDto);

        _dbContext.Update(roomTypeForUpdate);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(RoomType)} updated with Id: {roomTypeForUpdate.Id}");

        return Result.Ok(roomTypeForUpdate.Id);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid roomId, CancellationToken ct)
    {
        var roomTypeForDelete = await _dbContext
            .RoomsTypes
            .Include(rt => rt.Amenities)
            .SingleOrDefaultAsync(rt => rt.Id == roomId, ct);

        if (roomTypeForDelete == null)
            return Result.Fail("Room type not find");

        _dbContext.RoomsTypes.Remove(roomTypeForDelete);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(RoomType)} deleted with Id: {roomId}");
        return Result.Ok(roomTypeForDelete.Id);
    }

    public async Task<Result<IEnumerable<ResponseRoomTypeDto>>> GetAllRoomTypesAsync(CancellationToken ct)
    {
        const string keyForRedisCache = "room_types";
        var cachedRoomTypes = await _distributedCache.GetStringAsync(keyForRedisCache, ct);
        IEnumerable<ResponseRoomTypeDto> roomTypes;
        if (string.IsNullOrEmpty(cachedRoomTypes))
        {
            roomTypes = await _dbContext
                .RoomsTypes
                .Select(MapToResponseRoomTypeDto)
                .ToListAsync(ct);
            if (roomTypes.Any())
            {
                await _distributedCache.SetStringAsync(keyForRedisCache, JsonConvert.SerializeObject(roomTypes), ct);
                _logger.LogInformation($"Cached list of RoomTypes");
            }
            
            return Result.Ok(roomTypes);
        }

        roomTypes = JsonConvert.DeserializeObject<IEnumerable<ResponseRoomTypeDto>>(cachedRoomTypes);
        return Result.Ok(roomTypes);
    }
    
    public async Task<Result<ResponseRoomTypeDto>> GetRoomTypeByIdAsync(Guid id, CancellationToken ct)
    {
        string keyForRedisCache = $"room_types-{id}";
        var cachedRoomTypes = await _distributedCache.GetStringAsync(keyForRedisCache, ct);
        ResponseRoomTypeDto? roomType;
        if (string.IsNullOrEmpty(cachedRoomTypes))
        {
            roomType = await _dbContext
                .RoomsTypes
                .Select(MapToResponseRoomTypeDto)
                .SingleOrDefaultAsync(rt => rt.Id == id, ct);
            if (roomType is not null)
            {
                await _distributedCache.SetStringAsync(keyForRedisCache, JsonConvert.SerializeObject(roomType), ct);
                _logger.LogInformation($"RoomTypes cached");
            }
            
            return Result.Ok(roomType);
        }

        roomType = JsonConvert.DeserializeObject<ResponseRoomTypeDto>(cachedRoomTypes);
        return Result.Ok(roomType);
    }

    private void UpdateRoomType(RoomType roomType, UpdateRoomTypeDto updateDto)
    {
        roomType.Title = updateDto.Title;
        roomType.Description = updateDto.Description;
        roomType.MaxCapacity = updateDto.MaxCapacity;
        roomType.Amenities = updateDto.Amenities;
        roomType.NightlyRate = updateDto.NightlyRate;
        roomType.Images = updateDto.Images;
    }

    private static readonly Expression<Func<RoomType, ResponseRoomTypeDto>> MapToResponseRoomTypeDto = rt =>
        new ResponseRoomTypeDto
        {
            Id = rt.Id,
            Title = rt.Title,
            Description = rt.Description,
            MaxCapacity = rt.MaxCapacity,
            NightlyRate = rt.NightlyRate,
            ImagePathes = rt.Images.Select(i => i.Path).ToList(),
            Amenities = rt.Amenities
        };
}