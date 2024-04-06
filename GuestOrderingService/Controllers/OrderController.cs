using GuestOrderingService.DTO;
using GuestOrderingService.Features;
using Microsoft.AspNetCore.Mvc;

namespace GuestOrderingService.Controllers;

[Route("api/v1/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderFeature _orderFeature;

    public OrderController(OrderFeature orderFeature)
    {
        _orderFeature = orderFeature;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateOrderDto dto, CancellationToken ct)
    {
        var result = await _orderFeature.CreateAsync(dto, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
    
    [HttpPut]
    [Route("/cancel")]
    public async Task<IActionResult> CancelAsync(Guid orderId, CancellationToken ct)
    {
        var result = await _orderFeature.CancelAsync(orderId, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result);
    }
    
    [HttpPut]
    [Route("/complete")]
    public async Task<IActionResult> CompleteAsync(Guid orderId, CancellationToken ct)
    {
        var result = await _orderFeature.CompleteAsync(orderId, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct)
    {
        var result = await _orderFeature.GetAllAsync(ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("{guestId:guid}")]
    public async Task<IActionResult> GetAllAsync(Guid guestId, CancellationToken ct)
    {
        var result = await _orderFeature.GetAllByGuestIdAsync(guestId, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
}