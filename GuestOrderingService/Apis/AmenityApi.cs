using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Apis;

public static class AmenityApi
{
    public static IEndpointRouteBuilder MapAmenityApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/amenities", GetAllAmenitiesAsync);
        app.MapGet("/amenities/{id:guid}", GetAmenityByIdAsync);
        app.MapPost("/amenities", CreateAmenityAsync);
        app.MapPut("/amenities", UpdateAmenityAsync);
        app.MapDelete("/amenities{id:guid}", DeleteAmenityAsync);

        return app;
    }

    private static async Task<Ok<IEnumerable<ResponseAmenityDto>>> GetAllAmenitiesAsync(
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var amenities = await dbContext
            .Amenities
            .Select(a => new ResponseAmenityDto()
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            })
            .ToListAsync(ct);

        return TypedResults.Ok<IEnumerable<ResponseAmenityDto>>(amenities);
    }

    private static async Task<Results<Ok<ResponseAmenityDto>, NotFound>> GetAmenityByIdAsync(
        Guid id,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var amenity = await dbContext
            .Amenities
            .Select(a => new ResponseAmenityDto()
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            })
            .FirstOrDefaultAsync(a => a.Id == id, ct);

        if (amenity == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(amenity);
    }

    private static async Task<Ok> CreateAmenityAsync(
        CreateAmenityDto amenityDto,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var amenity = new Amenity()
        {
            Id = Guid.NewGuid(),
            Name = amenityDto.Name,
            Description = amenityDto.Description
        };

        await dbContext.Amenities.AddAsync(amenity, ct);
        await dbContext.SaveChangesAsync(ct);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound>> UpdateAmenityAsync(
        UpdateAmenityDto amenityDto,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var amenityForUpdate = await dbContext
            .Amenities
            .FirstOrDefaultAsync(a => a.Id == amenityDto.Id, ct);

        if (amenityForUpdate == null)
        {
            return TypedResults.NotFound();
        }

        amenityForUpdate.Name = amenityDto.Name;
        amenityForUpdate.Description = amenityDto.Description;
        
        await dbContext.SaveChangesAsync(ct);
        return TypedResults.Ok();
    }

    private static async Task<Results<NoContent, NotFound>> DeleteAmenityAsync(
        Guid id,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var amenityForDelete = await dbContext.Amenities.FirstOrDefaultAsync(a => a.Id == id, ct);
        if (amenityForDelete == null)
        {
            return TypedResults.NotFound();
        }

        dbContext.Amenities.Remove(amenityForDelete);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }
}