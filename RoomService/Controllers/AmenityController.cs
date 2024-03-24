using Microsoft.AspNetCore.Mvc;
using RoomService.DTO;
using RoomService.Interfaces;

namespace RoomService.Controllers;

[Route("api/v1/amenities")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IAmenityService _amenityService;

    public AmenityController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAmenityDto amenityDto, CancellationToken ct)
    {
        var createdAmenityId = await _amenityService.CreateAsync(amenityDto, ct);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAmenityDto amenityDto, CancellationToken ct)
    {
        var updatedAmenityDto = await _amenityService.UpdateAsync(amenityDto, ct);
        return Ok();
    }

    [HttpDelete]
    [Route("id")]
    public async Task<IActionResult> Delete([FromQuery] Guid id, CancellationToken ct)
    {
        await _amenityService.DeleteAsync(id, ct);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseAmenityDto>>> GetAll(CancellationToken ct)
    {
        var amenities = await _amenityService.GetAllAsync(ct);
        return Ok(amenities);
    }
}