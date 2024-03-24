using FluentResults;
using Microsoft.EntityFrameworkCore;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Exceptions;
using RoomService.Interfaces;
using RoomService.Models;

namespace RoomService.Services;

public class AmenityService : IAmenityService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AmenityService> _logger;

    public AmenityService(AppDbContext appDbContext, ILogger<AmenityService> logger)
    {
        _dbContext = appDbContext;
        _logger = logger;
    }

    public async Task<Result<Guid>> CreateAsync(CreateAmenityDto createAmenityDto, CancellationToken ct)
    {
        var amenity = new Amenity()
        {
            Id = Guid.NewGuid(),
            Title = createAmenityDto.Title,
            Description = createAmenityDto.Description
        };
        await _dbContext.Amenities.AddAsync(amenity, ct);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Amenity)} created with Id: {amenity.Id}");

        return Result.Ok(amenity.Id);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateAmenityDto updateAmenityDto, CancellationToken ct)
    {
        var amenityForUpdate = await _dbContext
            .Amenities
            .SingleOrDefaultAsync(a => a.Id == updateAmenityDto.Id, ct);

        if (amenityForUpdate == null)
            return Result.Fail<Guid>("Amenity not found");

        amenityForUpdate.Title = updateAmenityDto.Title;
        amenityForUpdate.Description = updateAmenityDto.Description;

        _dbContext.Update(amenityForUpdate);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Amenity)} updated with Id: {amenityForUpdate.Id}");

        return Result.Ok(amenityForUpdate.Id);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken ct)
    {
        var amenityForDelete = await _dbContext
                                   .Amenities
                                   .SingleOrDefaultAsync(a => a.Id == id, ct)
                               ?? throw new NotFoundException(nameof(Amenity), id);
        _dbContext.Amenities.Remove(amenityForDelete);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(Amenity)} deleted with Id: {amenityForDelete.Id}");
        return Result.Ok(amenityForDelete.Id);
    }

    public async Task<Result<IEnumerable<ResponseAmenityDto>>> GetAllAsync(CancellationToken ct)
    {
        var amenities = await _dbContext
            .Amenities
            .Select(a => new ResponseAmenityDto
            {
                Id = a.Id,
                Name = a.Title,
                Description = a.Description
            })
            .ToListAsync(ct);

        _logger.LogInformation($"Retrieved {amenities.Count} amenities");

        return Result.Ok<IEnumerable<ResponseAmenityDto>>(amenities);
    }
}