using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Apis;

public static class ServiceApi
{
    public static IEndpointRouteBuilder MapServiceApi(this IEndpointRouteBuilder app)
    {
        // Routes for querying hotel services
        app.MapGet("/services", GetAllServicesAsync);
        app.MapGet("/services/by", GetAllServicesByIdsAsync);
        app.MapGet("/services/{id:guid}", GetServiceByIdAsync);
        app.MapGet("/services/by/{name:minlength(1)}", GetServicesByNameAsync);

        // Routes for modifying catalog items.
        app.MapPost("/services", CreateServiceAsync);
        app.MapPut("/services", UpdateServiceAsync);
        app.MapDelete("/services", DeleteServiceAsync);

        return app;
    }
    
    private static async Task<Ok<IEnumerable<ResponseServiceDto>>> GetAllServicesAsync(
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var services = await dbContext.Services
            .Select(s => new ResponseServiceDto()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                WaitingTime = s.WaitingTime
            })
            .ToListAsync(ct);
        return TypedResults.Ok<IEnumerable<ResponseServiceDto>>(services);
    }
    
    private static async Task<Ok<IEnumerable<ResponseServiceDto>>> GetAllServicesByIdsAsync(
        [FromQuery] Guid[] ids,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var services = await dbContext.Services
            .Where(s => ids.Contains(s.Id))
            .Select(s => new ResponseServiceDto()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                WaitingTime = s.WaitingTime
            })
            .ToListAsync(ct);
        return TypedResults.Ok<IEnumerable<ResponseServiceDto>>(services);
    }
    
    private static async Task<Results<Ok<ResponseServiceDto>, NotFound>> GetServiceByIdAsync(Guid id,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var service = await dbContext.Services
            .SingleOrDefaultAsync(s => s.Id == id, ct);

        if (service == null)
        {
            return TypedResults.NotFound();
        }

        var responseDto = new ResponseServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Description = service.Description,
            Price = service.Price,
            WaitingTime = service.WaitingTime
        };

        return TypedResults.Ok(responseDto);
    }
    
    private static async Task<Ok<IEnumerable<ResponseServiceDto>>> GetServicesByNameAsync(
        [FromQuery] string name,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var services = await dbContext.Services
            .Where(s => s.Name == name)
            .Select(s => new ResponseServiceDto()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price,
                WaitingTime = s.WaitingTime
            })
            .ToListAsync(ct);
        return TypedResults.Ok<IEnumerable<ResponseServiceDto>>(services);
    }

    private static async Task<Ok> CreateServiceAsync(
        [FromBody] CreateServiceDto serviceDto,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var service = new Service()
        {
            Id = Guid.NewGuid(),
            Name = serviceDto.Name,
            Description = serviceDto.Description,
            Price = serviceDto.Price,
            WaitingTime = serviceDto.WaitingTime
        };
        await dbContext.Services.AddAsync(service, ct);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound<string>>> UpdateServiceAsync(
        [FromBody] UpdateServiceDto serviceDto,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var serviceToUpdate = await dbContext.Services.SingleOrDefaultAsync(s => s.Id == serviceDto.Id, ct);
        if (serviceToUpdate == null)
        {
            return TypedResults.NotFound($"Item with id {serviceDto.Id} not found.");
        }

        serviceToUpdate.Name = serviceDto.Name;
        serviceToUpdate.Description = serviceDto.Description;
        serviceToUpdate.Price = serviceDto.Price;
        serviceToUpdate.WaitingTime = serviceDto.WaitingTime;

        dbContext.Services.Update(serviceToUpdate);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }

    private static async Task<Results<NoContent, NotFound>> DeleteServiceAsync(
        [FromQuery] Guid id,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var serviceToDelete = await dbContext.Services.SingleOrDefaultAsync(s => s.Id == id, ct);

        if (serviceToDelete == null)
        {
            return TypedResults.NotFound();
        }

        dbContext.Services.Remove(serviceToDelete);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.NoContent();
    }
}