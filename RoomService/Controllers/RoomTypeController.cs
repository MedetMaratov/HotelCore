using Microsoft.AspNetCore.Mvc;
using RoomService.DTO;
using RoomService.Interfaces;

namespace RoomService.Controllers;

[Route("api/v1/room-types")]
[ApiController]
public class RoomTypeController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;

    public RoomTypeController(IRoomTypeService roomTypeService)
    {
        _roomTypeService = roomTypeService;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoomTypeDto roomTypeDto, 
        CancellationToken ct)
    {
        var createRoomTypeId = await _roomTypeService.CreateAsync(roomTypeDto, ct);
        return Ok();
    }

    [HttpPut]
    [Route("")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateRoomTypeDto roomTypeDto, 
        CancellationToken ct)
    {
        var updateRoomId = await _roomTypeService.UpdateAsync(roomTypeDto, ct);
        return Ok();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _roomTypeService.DeleteAsync(id, ct);
        return NoContent();
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IEnumerable<ResponseRoomTypeDto>>> GetAll(CancellationToken ct)
    {
        var roomTypes = await _roomTypeService.GetAllRoomTypesAsync(ct);
        return Ok(roomTypes);
    }
}