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
        var result = await _occupiedRoomService.SetCheckInAsync(roomId, ct);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Reasons);
    }

    [HttpPut]
    [Route("check-out/{roomId:guid}")]
    public async Task<IActionResult> SetCheckOut(Guid roomId, CancellationToken ct)
    {
        var result = await _occupiedRoomService.SetCheckOutAsync(roomId, ct);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Reasons);
    }
    
    [HttpGet]
    [Route("hotel-branch/{hotelBranchId:guid}/occupied")]
    public async Task<IActionResult> GetOccupiedRoomsByHotelBranchAsync(Guid hotelBranchId, CancellationToken ct)
    {
        var result = await _occupiedRoomService.GetOccupiedRoomsByHotelBranchAsync(hotelBranchId, ct);
        if (result.IsSuccess)
            return Ok(result.Value);
        return BadRequest(result.Reasons);
    }
}