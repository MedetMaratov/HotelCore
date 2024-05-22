using System.Linq.Expressions;
using FluentResults;
using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Models.Amenity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Features;

public class AmenityFeature
{
    private readonly AppDbContext _dbContext;

    public AmenityFeature(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<Guid>> CreateAsync(CreateAmenityDto amenityDto, CancellationToken ct)
    {
        var amenity = new Amenity
        {
            Id = Guid.NewGuid(),
            Name = amenityDto.Name,
            Description = amenityDto.Description,
            CategoryId = amenityDto.CateroryId,
            ImagePath = amenityDto.ImagePath
        };

        await _dbContext.Amenities.AddAsync(amenity, ct);
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok(amenity.Id);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateAmenityDto amenityDto, CancellationToken ct)
    {
        var amenityForUpdate = await _dbContext
            .Amenities
            .FirstOrDefaultAsync(a => a.Id == amenityDto.Id, ct);

        if (amenityForUpdate == null)
        {
            return Result.Fail("Amenity not found");
        }

        amenityForUpdate.Name = amenityDto.Name;
        amenityForUpdate.Description = amenityDto.Description;
        amenityForUpdate.ImagePath = amenityDto.ImagePath;

        _dbContext.Amenities.Update(amenityForUpdate);
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok(amenityForUpdate.Id);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken ct)
    {
        var amenityForDelete = await _dbContext.Amenities.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (amenityForDelete == null)
        {
            return Result.Fail("Amenity not found");
        }

        _dbContext.Amenities.Remove(amenityForDelete);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok(amenityForDelete.Id);
    }
    
    public async Task<Result<IEnumerable<ResponseAmenityDto>>> GetAllAsync(CancellationToken ct)
    {
        var amenities = await _dbContext
            .Amenities
            .Select(MapToResponseAmenityDto())
            .ToListAsync(ct);

        return Result.Ok<IEnumerable<ResponseAmenityDto>>(amenities);
    }

    public async Task<Result<ResponseAmenityDto>> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var amenity = await _dbContext
            .Amenities
            .Select(MapToResponseAmenityDto())
            .SingleOrDefaultAsync(a => a.Id == id, ct);

        return amenity == null ? Result.Fail("Amenity not found") : Result.Ok(amenity);
    }
    
    private static Expression<Func<Amenity, ResponseAmenityDto>> MapToResponseAmenityDto()
    {
        return a => new ResponseAmenityDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            CateroryId = a.CategoryId,
            CategoryName = a.Category.Name,
            ImagePath = a.ImagePath
        };
    }

}