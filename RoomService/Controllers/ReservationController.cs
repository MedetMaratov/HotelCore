using Microsoft.AspNetCore.Mvc;
using RoomService.DTO;
using RoomService.Interfaces;

namespace RoomService.Controllers;

[Route("api/v1")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    [Route("reservation")]
    public async Task<IActionResult> Reserve(
        CreateReservationDto createReservationDto, 
        CancellationToken ct)
    {
        var reservation = await _reservationService.ReserveAsync(createReservationDto, ct);
        return Ok();
    }
}