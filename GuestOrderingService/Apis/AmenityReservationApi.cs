using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Apis;

public static class AmenityReservationApi
{
    public static IEndpointRouteBuilder MapAmenityReservationApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/amenity/reservation", GetAllAmenityReservationAsync);
        app.MapGet("/amenity/reservation/{id:guid}", GetAmenityReservationByIdAsync);
        app.MapPost("/amenity/reservation", CreateAmenityReservationAsync);
        app.MapDelete("/amenity/reservation{id:guid}", DeleteAmenityReservationAsync);

        return app;
    }

    private static async Task<Ok> CreateAmenityReservationAsync(
        CreateAmenityReservationDto dto, 
        AppDbContext dbContext, 
        CancellationToken ct)
    {
        var reservation = new AmenityReservation()
        {
            Id = Guid.NewGuid(),
            AmenityId = dto.AmenityId,
            DateTime = dto.DateTime,
            GuestId = dto.GuestId
        };

        await dbContext.AmenityReservations.AddAsync(reservation, ct);
        await dbContext.SaveChangesAsync(ct);
        return TypedResults.Ok();
    }
    
    private static async Task<Results<NoContent, NotFound>> DeleteAmenityReservationAsync(
        Guid id,
        AppDbContext dbContext, 
        CancellationToken ct)
    {
        var reservationForDelete = await dbContext
            .AmenityReservations
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (reservationForDelete == null)
        {
            return TypedResults.NotFound();
        }

        dbContext.AmenityReservations.Remove(reservationForDelete);
        await dbContext.SaveChangesAsync(ct);
        
        return TypedResults.NoContent();
    }
    private static async Task<Results<Ok<ResponseAmenityReservationDto>, NotFound>> GetAmenityReservationByIdAsync(Guid id, AppDbContext dbContext, CancellationToken ct)
    {
        var reservation = await dbContext
            .AmenityReservations
            .Select(r => new ResponseAmenityReservationDto()
            {
                Id = r.Id,
                AmenityId = r.AmenityId,
                DateTime = r.DateTime,
                GuestId = r.GuestId
            })
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (reservation == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(reservation);
    }

    private static async Task<Ok<IEnumerable<ResponseAmenityReservationDto>>> GetAllAmenityReservationAsync(
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var reservations = await dbContext
            .AmenityReservations
            .Select(r => new ResponseAmenityReservationDto()
            {
                Id = r.Id,
                AmenityId = r.AmenityId,
                DateTime = r.DateTime,
                GuestId = r.GuestId
            })
            .ToListAsync(ct);

        return TypedResults.Ok<IEnumerable<ResponseAmenityReservationDto>>(reservations);
    }
}