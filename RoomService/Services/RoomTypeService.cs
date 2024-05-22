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
    private const string KeyForRedisCache = "room_types";

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
            NightlyRate = roomTypeDto.NightlyRate,
            Description = roomTypeDto.Description,
            MaxCapacity = roomTypeDto.MaxCapacity,
        };
        roomType.Images.AddRange(roomTypeDto.Images.Select(i => new ImageForRoomType()
        {
            Path = i,
            RoomType = roomType,
            RoomTypeId = roomType.Id
        }));

        var amenitiesForRoomType = await _dbContext
            .Amenities
            .Where(a => roomTypeDto.Amenities.Contains(a.Id))
            .ToListAsync(ct);
        roomType.Amenities = amenitiesForRoomType;

        await _dbContext.AddAsync(roomType, ct);
        await _dbContext.SaveChangesAsync(ct);
        await _distributedCache.RemoveAsync(KeyForRedisCache, ct);
        _logger.LogInformation($"{nameof(RoomType)} created with Id: {roomType.Id}");

        return Result.Ok(roomType.Id);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateRoomTypeDto updateDto, CancellationToken ct)
    {
        var roomType = await _dbContext
            .RoomsTypes
            .Include(roomType => roomType.Amenities)
            .Include(roomType => roomType.Images)
            .SingleOrDefaultAsync(rt => rt.Id == updateDto.Id, ct);

        if (roomType == null)
            return Result.Fail("Room type not found");

        roomType.Title = updateDto.Title;
        roomType.Description = updateDto.Description;
        roomType.MaxCapacity = updateDto.MaxCapacity;

        foreach (var existingAmenity in roomType.Amenities.ToList()
                     .Where(existingAmenity => !updateDto.Amenities.Contains(existingAmenity.Id)))
        {
            roomType.Amenities.Remove(existingAmenity);
        }

        var amenitiesToAdd = await _dbContext
            .Amenities
            .Where(a => updateDto.Amenities.Contains(a.Id))
            .ToListAsync(ct);

        foreach (var newAmenity in amenitiesToAdd.Where(
                     newAmenity => roomType.Amenities.All(a => a.Id != newAmenity.Id)))
        {
            roomType.Amenities.Add(newAmenity);
        }

        roomType.NightlyRate = updateDto.NightlyRate;
        _dbContext.ImagesForRoomTypes.RemoveRange(roomType.Images);
        roomType.Images = new List<ImageForRoomType>();
        roomType.Images.AddRange(updateDto.Images.Select(i => new ImageForRoomType()
        {
            Path = i,
            RoomType = roomType,
            RoomTypeId = roomType.Id
        }));

        _dbContext.Update(roomType);
        await _dbContext.SaveChangesAsync(ct);
        await _distributedCache.RemoveAsync(KeyForRedisCache, ct);

        _logger.LogInformation($"{nameof(RoomType)} updated with Id: {roomType.Id}");

        return Result.Ok(roomType.Id);
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
        await _distributedCache.RemoveAsync(KeyForRedisCache, ct);

        _logger.LogInformation($"{nameof(RoomType)} deleted with Id: {roomId}");
        return Result.Ok(roomTypeForDelete.Id);
    }

    public async Task<Result<IEnumerable<ResponseRoomTypeDto>>> GetAllRoomTypesAsync(CancellationToken ct)
    {
        var cachedRoomTypes = await _distributedCache.GetStringAsync(KeyForRedisCache, ct);
        IEnumerable<ResponseRoomTypeDto> roomTypes;
        if (string.IsNullOrEmpty(cachedRoomTypes))
        {
            roomTypes = await _dbContext
                .RoomsTypes
                .Select(MapToResponseRoomTypeDto)
                .ToListAsync(ct);
            if (roomTypes.Any())
            {
                await _distributedCache.SetStringAsync(KeyForRedisCache, JsonConvert.SerializeObject(roomTypes), ct);
                _logger.LogInformation($"Cached list of RoomTypes");
            }

            return Result.Ok(roomTypes);
        }

        roomTypes = JsonConvert.DeserializeObject<IEnumerable<ResponseRoomTypeDto>>(cachedRoomTypes);
        return Result.Ok(roomTypes);
    }

    public async Task<Result<IEnumerable<ResponseRoomTypeDto>>> GetAvailableRoomTypesAsync(
        Guid hotelBranchId,
        DateTime reservationStart,
        DateTime reservationEnd,
        int numberOfGuests,
        CancellationToken ct)
    {
        if (reservationStart >= reservationEnd)
        {
            return Result.Fail("Check-in date must be before check-out date.");
        }

        if (reservationStart < DateTime.Today)
        {
            return Result.Fail("Check-in date must not be earlier than today");
        }
        var roomTypes = await _dbContext
            .RoomsTypes
            .Where(rt => rt.MaxCapacity >= numberOfGuests)
            .Select(MapToResponseRoomTypeDto)
            .ToListAsync(ct);
        return roomTypes;
    }
    
    public async Task<Result<ResponseRoomTypeDto>> GetRoomTypeByIdAsync(Guid id, CancellationToken ct)
    {
        var roomType = await _dbContext
            .RoomsTypes
            .Select(MapToResponseRoomTypeDto)
            .SingleOrDefaultAsync(rt => rt.Id == id, ct);

        return Result.Ok(roomType);
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
            Amenities = rt.Amenities.Select(a => a.Title).ToList()
        };
}