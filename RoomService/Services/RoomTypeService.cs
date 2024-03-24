using FluentResults;
using Microsoft.EntityFrameworkCore;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Interfaces;
using RoomService.Models;

namespace RoomService.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RoomTypeService> _logger;

    public RoomTypeService(AppDbContext dbContext, ILogger<RoomTypeService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid>> CreateAsync(CreateRoomTypeDto roomTypeDto, CancellationToken ct)
    {
        var roomType = new RoomType()
        {
            Id = Guid.NewGuid(),
            Title = roomTypeDto.Title,
            ImagePath = roomTypeDto.ImagePath,
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

        roomTypeForUpdate.Title = updateDto.Title;
        roomTypeForUpdate.Description = updateDto.Description;
        roomTypeForUpdate.MaxCapacity = updateDto.MaxCapacity;
        roomTypeForUpdate.Amenities = updateDto.Amenities;
        roomTypeForUpdate.NightlyRate = updateDto.NightlyRate;
        roomTypeForUpdate.ImagePath = updateDto.ImagePath;

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
        var roomTypes = await _dbContext
            .RoomsTypes
            .Select(rt => new ResponseRoomTypeDto()
            {
                Id = rt.Id,
                Title = rt.Title,
                Description = rt.Description,
                MaxCapacity = rt.MaxCapacity,
                NightlyRate = rt.NightlyRate,
                Amenities = rt.Amenities,
            }).ToListAsync(ct);

        _logger.LogInformation($"Retrieved {roomTypes.Count} RoomTypes");

        return Result.Ok<IEnumerable<ResponseRoomTypeDto>>(roomTypes);
    }
}