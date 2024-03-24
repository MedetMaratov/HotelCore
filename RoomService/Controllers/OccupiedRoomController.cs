using Microsoft.AspNetCore.Mvc;
using RoomService.Interfaces;

namespace RoomService.Controllers;

[Route("api/v1/occupied-room")]
[ApiController]
public class OccupiedRoomController : ControllerBase
{
    private readonly IOccupiedRoomService _occupiedRoomService;

    public OccupiedRoomController(IOccupiedRoomService occupiedRoomService)
    {
        _occupiedRoomService = occupiedRoomService;
    }

    [HttpPut]
    [Route("check-in/{roomId:guid}")]
    public async Task<IActionResult> SetCheckIn(Guid roomId, CancellationToken ct)
    {
        var id = await _occupiedRoomService.SetCheckInAsync(roomId, ct);
        return Ok();
    }

    [HttpPut]
    [Route("check-out/{roomId:guid}")]
    public async Task<IActionResult> SetCheckOut(Guid roomId, CancellationToken ct)
    {
        var id = await _occupiedRoomService.SetCheckOutAsync(roomId, ct);
        return Ok();
    }
}