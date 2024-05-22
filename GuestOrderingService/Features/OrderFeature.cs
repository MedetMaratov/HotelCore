using System.Linq.Expressions;
using FluentResults;
using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Enums;
using GuestOrderingService.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Features;

public class OrderFeature
{
    private readonly AppDbContext _dbContext;

    public OrderFeature(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> CreateAsync(CreateOrderDto createServiceDto, CancellationToken ct)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            ServiceId = createServiceDto.ServiceId,
            CustomerId = createServiceDto.CustomerId,
            RoomId = createServiceDto.RoomId,
        };

        await _dbContext.Orders.AddAsync(order, ct);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok(order.Id);
    }

    public async Task<Result> CancelAsync(Guid orderIdToCancel, CancellationToken ct)
    {
        var orderToCancel = await _dbContext.Orders.SingleOrDefaultAsync(o => o.Id == orderIdToCancel, ct);

        if (orderToCancel == null)
        {
            return Result.Fail("Order not found");
        }

        if (orderToCancel.Status == OrderStatus.Completed)
        {
            return Result.Fail("This order has already been completed");
        }

        orderToCancel.Cancel();

        _dbContext.Orders.Update(orderToCancel);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result> CompleteAsync(Guid orderIdToComplete, CancellationToken ct)
    {
        var orderToComplete = await _dbContext.Orders.SingleOrDefaultAsync(o => o.Id == orderIdToComplete, ct);

        if (orderToComplete == null)
        {
            return Result.Fail("Order not found");
        }

        if (orderToComplete.Status == OrderStatus.Canceled)
        {
            return Result.Fail("This order has been canceled");
        }

        orderToComplete.Complete();

        _dbContext.Orders.Update(orderToComplete);
        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }

    public async Task<Result<IEnumerable<ResponseOrderDto>>> GetAllAsync(CancellationToken ct)
    {
        var orders = await _dbContext
            .Orders
            .OrderBy(o => o.Status)
            .Select(MapToResponseOrderDto)
            .ToListAsync(ct);

        return Result.Ok<IEnumerable<ResponseOrderDto>>(orders);
    }

    public async Task<Result<IEnumerable<ResponseOrderDto>>> GetAllByGuestIdAsync(Guid customerId, CancellationToken ct)
    {
        var orders = await _dbContext
            .Orders
            .Where(o => o.CustomerId == customerId)
            .OrderBy(o => o.Status)
            .Select(MapToResponseOrderDto)
            .ToListAsync(ct);

        return Result.Ok<IEnumerable<ResponseOrderDto>>(orders);
    }

    private static readonly Expression<Func<Order, ResponseOrderDto>> MapToResponseOrderDto = o =>
        new ResponseOrderDto
        {
            Id = o.Id,
            CompleteTime = o.CompleteTime,
            CustomerId = o.CustomerId,
            RequestTime = o.RequestTime,
            RoomId = o.RoomId,
            ServiceId = o.ServiceId,
            ServiceName = o.Service.Name,
            Status = o.Status.ToString()
        };
}