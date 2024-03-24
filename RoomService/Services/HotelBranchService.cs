using FluentResults;
using Microsoft.EntityFrameworkCore;
using RoomService.DataAccess;
using RoomService.DTO;
using RoomService.Exceptions;
using RoomService.Interfaces;
using RoomService.Models;

namespace RoomService.Services;

public class HotelBranchService : IHotelBranchService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<HotelBranchService> _logger;

    public HotelBranchService(AppDbContext dbContext, ILogger<HotelBranchService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid>> CreateAsync(CreateHotelBranchDto hotelBranchDto, CancellationToken ct)
    {
        var hotelBranch = new HotelBranch()
        {
            Id = Guid.NewGuid(),
            LocationId = hotelBranchDto.LocationId,
            Name = hotelBranchDto.Name
        };

        await _dbContext.HotelBranches.AddAsync(hotelBranch, ct);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(HotelBranch)} created with Id: {hotelBranch.Id}");

        return Result.Ok(hotelBranch.Id);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateHotelBranchDto hotelBranchDto, CancellationToken ct)
    {
        var hotelBranchForUpdate =
            await _dbContext.HotelBranches.SingleOrDefaultAsync(hb => hb.Id == hotelBranchDto.Id);

        if (hotelBranchForUpdate == null)
            return Result.Fail("Hotel branch not found");

        hotelBranchForUpdate.LocationId = hotelBranchDto.LocationId;
        hotelBranchForUpdate.Name = hotelBranchForUpdate.Name;

        _dbContext.Update(hotelBranchForUpdate);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(HotelBranch)} updated with Id: {hotelBranchForUpdate.Id}");

        return Result.Ok(hotelBranchForUpdate.Id);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid hotelBranchId, CancellationToken ct)
    {
        var hotelBranchForDelete =
            await _dbContext.HotelBranches.SingleOrDefaultAsync(hb => hb.Id == hotelBranchId, ct);

        if (hotelBranchForDelete == null)
            return Result.Fail("Hotel branch not found");

        _dbContext.HotelBranches.Remove(hotelBranchForDelete);
        await _dbContext.SaveChangesAsync(ct);

        _logger.LogInformation($"{nameof(HotelBranch)} deleted with Id: {hotelBranchId}");

        return Result.Ok(hotelBranchForDelete.Id);
    }

    public async Task<Result<IEnumerable<ResponseHotelBranchDto>>> GetAllAsync(CancellationToken ct)
    {
        var hotelBranches = await _dbContext
            .HotelBranches
            .Select(hb => new ResponseHotelBranchDto()
            {
                Id = hb.Id,
                LocationId = hb.LocationId,
                Name = hb.Name
            })
            .ToListAsync(ct);

        _logger.LogInformation($"Retrieved {hotelBranches.Count()} HotelBranches");

        return Result.Ok<IEnumerable<ResponseHotelBranchDto>>(hotelBranches);
    }

    public async Task<Result<IEnumerable<ResponseHotelBranchDto>>> GetAllByLocationAsync(
        string country,
        string city,
        CancellationToken ct)
    {
        var hotelBranches = await _dbContext
            .HotelBranches
            .Include(hb => hb.Location)
            .Where(hb => hb.Location.Country == country && hb.Location.City == city)
            .Select(hb => new ResponseHotelBranchDto()
            {
                Id = hb.Id,
                LocationId = hb.LocationId,
                Name = hb.Name
            })
            .ToListAsync(ct);

        _logger.LogInformation($"Retrieved {hotelBranches.Count()} HotelBranches by Location");

        return Result.Ok<IEnumerable<ResponseHotelBranchDto>>(hotelBranches);
    }
}