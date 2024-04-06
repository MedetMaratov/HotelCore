using System.Linq.Expressions;
using FluentResults;
using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Models.Amenity;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Features;

public class ReservationFeature
{
    private readonly AppDbContext _dbContext;

    public ReservationFeature(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> CreateAsync(CreateReservationDto dto, CancellationToken ct)
    {
        var schedule = await _dbContext.Schedules.SingleOrDefaultAsync(s => s.Id == dto.AmenityScheduleId, ct);

        if (schedule is null)
        {
            return Result.Fail("Schedule not found");
        }

        if (schedule.RemainingCapacity < dto.NumberOfPersons)
        {
            return Result.Fail("No empty seats left");
        }

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            GuestId = dto.GuestId,
            AmenityScheduleId = dto.AmenityScheduleId,
            NumberOfPersons = dto.NumberOfPersons
        };
        schedule.RemainingCapacity -= reservation.NumberOfPersons;

        await _dbContext.Reservations.AddAsync(reservation, ct);
        _dbContext.Schedules.Update(schedule);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result<IEnumerable<ResponseReservationDto>>> GetAllByAmenityIdAsync(Guid amenityId,
        CancellationToken ct)
    {
        var reservations = await _dbContext
            .Reservations
            .Where(r => r.Schedule.AmenityId == amenityId)
            .Select(ReservationToDtoProjection)

            .ToListAsync(ct);

        return Result.Ok<IEnumerable<ResponseReservationDto>>(reservations);
    }
    
    public async Task<Result<IEnumerable<ResponseReservationDto>>> GetAllByGuestIdAsync(Guid guestId,
        CancellationToken ct)
    {
        var reservations = await _dbContext
            .Reservations
            .Where(r => r.GuestId == guestId)
            .Select(ReservationToDtoProjection)
            .ToListAsync(ct);

        return Result.Ok<IEnumerable<ResponseReservationDto>>(reservations);
    }
    
    private static readonly Expression<Func<Reservation, ResponseReservationDto>> ReservationToDtoProjection = r => new ResponseReservationDto()
    {
        Id = r.Id,
        StartTime = r.Schedule.StartTime,
        EndTime = r.Schedule.EndTime,
        AmenityName = r.Schedule.Amenity.Name,
        NumberOfPersons = r.NumberOfPersons,
        GuestId = r.GuestId
    };
}