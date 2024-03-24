using Microsoft.AspNetCore.Mvc;
using RoomService.DTO;
using RoomService.Interfaces;

namespace RoomService.Controllers;

[Route("api/v1/rooms")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct)
    {
        var rooms = await _roomService.GetAllAsync(ct);
        return Ok(rooms);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetAllAsync(Guid id, CancellationToken ct)
    {
        var rooms = await _roomService.GetRoomsByTypeAsync(id, ct);
        return Ok(rooms);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoomDto roomDto, CancellationToken ct)
    {
        var createdRoomId = await _roomService.CreateAsync(roomDto, ct);
        return Ok();
    }

    [HttpPut]
    [Route("")]
    public async Task<IActionResult> UpdateRoomDto([FromBody] UpdateRoomDto roomDto, CancellationToken ct)
    {
        var updatedRoomId = await _roomService.UpdateAsync(roomDto, ct);
        return Ok();
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteRoomAsync(Guid id, CancellationToken ct)
    {
        await _roomService.DeleteAsync(id, ct);
        return NoContent();
    }
}