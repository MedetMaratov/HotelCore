using GuestOrderingService.DataAccess;
using GuestOrderingService.DTO;
using GuestOrderingService.Enums;
using GuestOrderingService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuestOrderingService.Apis;

public static class OrderApi
{
    public static IEndpointRouteBuilder MapOrderApi(this IEndpointRouteBuilder app)
    {
        // Routes for querying orders
        app.MapGet("/orders", GetAllAsync);
        app.MapGet("/orders/{customerId:guid}", GetAllByGuestIdAsync);

        // Routes for modifying orders
        app.MapPost("/orders", CreateAsync);
        app.MapPut("/orders/cancel", CancelOrderAsync);
        app.MapPut("/orders/complete", CompleteOrderAsync);

        return app;
    }

    private static async Task<Ok<IEnumerable<ResponseOrderDto>>> GetAllAsync(AppDbContext dbContext,
        CancellationToken ct)
    {
        var orders = await dbContext
            .Orders
            .Select(o => new ResponseOrderDto()
            {
                Id = o.Id,
                CompleteTime = o.CompleteTime,
                CustomerId = o.CustomerId,
                RequestTime = o.RequestTime,
                RoomId = o.RoomId,
                ServiceId = o.ServiceId,
                Status = OrderStatus.InProgress
            })
            .ToListAsync(ct);

        return TypedResults.Ok<IEnumerable<ResponseOrderDto>>(orders);
    }

    private static async Task<Ok<IEnumerable<ResponseOrderDto>>> GetAllByGuestIdAsync(
        Guid customerId,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var orders = await dbContext
            .Orders
            .Where(o => o.CustomerId == customerId)
            .Select(o => new ResponseOrderDto()
            {
                Id = o.Id,
                CompleteTime = o.CompleteTime,
                CustomerId = o.CustomerId,
                RequestTime = o.RequestTime,
                RoomId = o.RoomId,
                ServiceId = o.ServiceId,
                Status = o.Status
            })
            .ToListAsync(ct);

        return TypedResults.Ok<IEnumerable<ResponseOrderDto>>(orders);
    }

    private static async Task<Ok> CreateAsync(
        [FromBody] CreateOrderDto createServiceDto,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var order = new Order()
        {
            Id = Guid.NewGuid(),
            CustomerId = createServiceDto.CustomerId,
            RoomId = createServiceDto.RoomId,
        };

        await dbContext.Orders.AddAsync(order, ct);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound, BadRequest<string>>> CancelOrderAsync(
        Guid orderIdToCancel,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var orderToCancel = await dbContext.Orders.SingleOrDefaultAsync(o => o.Id == orderIdToCancel, ct);

        if (orderToCancel == null)
        {
            return TypedResults.NotFound();
        }

        if (orderToCancel.Status == OrderStatus.Completed)
        {
            return TypedResults.BadRequest("This order has already been completed");
        }

        orderToCancel.Cancel();

        dbContext.Orders.Update(orderToCancel);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, NotFound, BadRequest<string>>> CompleteOrderAsync(
        Guid orderIdToComplete,
        AppDbContext dbContext,
        CancellationToken ct)
    {
        var orderToComplete = await dbContext.Orders.SingleOrDefaultAsync(o => o.Id == orderIdToComplete, ct);

        if (orderToComplete == null)
        {
            return TypedResults.NotFound();
        }

        if (orderToComplete.Status == OrderStatus.Canceled)
        {
            return TypedResults.BadRequest("This order has been canceled");
        }

        orderToComplete.Complete();

        dbContext.Orders.Update(orderToComplete);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}