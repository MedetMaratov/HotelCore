using System.Linq.Expressions;
using FluentResults;
using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Features;

public class ServiceFeature
{
    private readonly AppDbContext _dbContext;

    public ServiceFeature(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> CreateAsync(CreateServiceDto dto, CancellationToken ct)
    {
        var service = new Service
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            WaitingTime = dto.WaitingTime
        };

        await _dbContext.Services.AddAsync(service, ct);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok(service.Id);
    }
    
    public async Task<Result> UpdateAsync(UpdateServiceDto serviceDto, CancellationToken ct)
    {
        var serviceToUpdate = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == serviceDto.Id, ct);
        if (serviceToUpdate == null)
        {
            return Result.Fail($"Item with id {serviceDto.Id} not found.");
        }

        serviceToUpdate.Name = serviceDto.Name;
        serviceToUpdate.Description = serviceDto.Description;
        serviceToUpdate.Price = serviceDto.Price;
        serviceToUpdate.WaitingTime = serviceDto.WaitingTime;

        _dbContext.Services.Update(serviceToUpdate);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
    {
        var serviceToDelete = await _dbContext.Services.SingleOrDefaultAsync(s => s.Id == id, ct);

        if (serviceToDelete == null)
        {
            return Result.Fail("Service not found");
        }

        _dbContext.Services.Remove(serviceToDelete);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }
    
    public async Task<Result<IEnumerable<ResponseServiceDto>>> GetAllAsync(CancellationToken ct)
    {
        var services = await _dbContext.Services
            .Select(MapToResponseServiceDto())
            .ToListAsync(ct);
        
        return Result.Ok<IEnumerable<ResponseServiceDto>>(services);
    }
    
    private static Expression<Func<Service, ResponseServiceDto>> MapToResponseServiceDto()
    {
        return s => new ResponseServiceDto()
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            Price = s.Price,
            WaitingTime = s.WaitingTime
        };
    }
}