using GuestOrderingService.DTO;
using GuestOrderingService.Features;
using Microsoft.AspNetCore.Mvc;

namespace GuestOrderingService.Controllers;

[Route("api/v1/reservations")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly ReservationFeature _reservationFeature;

    public ReservationController(ReservationFeature reservationFeature)
    {
        _reservationFeature = reservationFeature;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateReservationDto dto, CancellationToken ct)
    {
        var result = await _reservationFeature.CreateAsync(dto, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("amenity/{amenityId:guid}")]
    public async Task<IActionResult> GetAllByAsync(Guid amenityId, CancellationToken ct)
    {
        var result = await _reservationFeature.GetAllByAmenityIdAsync(amenityId, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("guest/{amenityId:guid}")]
    public async Task<IActionResult> GetAllByGuestIdAsync(Guid guestId, CancellationToken ct)
    {
        var result = await _reservationFeature.GetAllByGuestIdAsync(guestId, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
}