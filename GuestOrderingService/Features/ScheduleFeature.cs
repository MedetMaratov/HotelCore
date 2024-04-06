using System.Linq.Expressions;
using FluentResults;
using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Models.Amenity;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Features;

public class ScheduleFeature
{
    private readonly AppDbContext _dbContext;

    public ScheduleFeature(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> CreateAsync(CreateScheduleDto dto, CancellationToken ct)
    {
        var schedule = new Schedule
        {
            Id = Guid.NewGuid(),
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            AmenityId = dto.AmenityId,
            Capacity = dto.Capacity,
            RemainingCapacity = dto.Capacity
        };

        await _dbContext.Schedules.AddAsync(schedule, ct);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok(schedule.Id);
    }
    
    public async Task<Result<Guid>> UpdateAsync(UpdateScheduleDto dto, CancellationToken ct)
    {
        var scheduleToUpdate = await _dbContext.Schedules.SingleOrDefaultAsync(s => s.Id == dto.Id, ct);

        if (scheduleToUpdate is null)
        {
            return Result.Fail("Schedule not found");
        }
        
        scheduleToUpdate.StartTime = dto.StartTime;
        scheduleToUpdate.EndTime = dto.EndTime;
        scheduleToUpdate.Capacity = dto.Capacity;

        _dbContext.Schedules.Update(scheduleToUpdate);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok(scheduleToUpdate.Id);
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
    {
        var scheduleToDelete = await _dbContext.Schedules.SingleOrDefaultAsync(s => s.Id == id, ct);

        if (scheduleToDelete is null)
        {
            return Result.Fail("Schedule not found");
        }

        _dbContext.Schedules.Remove(scheduleToDelete);
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok();
    }

    public async Task<Result<IEnumerable<ResponseScheduleDto>>> GetAllByAmenityIdAsync(Guid amenityId, CancellationToken ct)
    {
        var amenities = await _dbContext
            .Schedules
            .Where(s => s.AmenityId == amenityId)
            .Select(MapToDtoProjection())
            .ToListAsync(ct);

        return Result.Ok<IEnumerable<ResponseScheduleDto>>(amenities);
    }

    private static Expression<Func<Schedule, ResponseScheduleDto>> MapToDtoProjection()
    {
        return s  => new ResponseScheduleDto()
        {
            Id = s.Id,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Capacity = s.Capacity,
            AmenityId = s.AmenityId,
            RemainingCapacity = s.RemainingCapacity
        };
    }
}