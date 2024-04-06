using GuestOrderingService.DTO;
using GuestOrderingService.Features;
using Microsoft.AspNetCore.Mvc;

namespace GuestOrderingService.Controllers;

[Route("api/v1/schedules")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly ScheduleFeature _scheduleFeature;

    public ScheduleController(ScheduleFeature scheduleFeature)
    {
        _scheduleFeature = scheduleFeature;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateScheduleDto dto, CancellationToken ct)
    {
        var result = await _scheduleFeature.CreateAsync(dto, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateScheduleDto dto, CancellationToken ct)
    {
        var result = await _scheduleFeature.UpdateAsync(dto, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result);
    }

    [HttpDelete]    
    [Route("{scheduleId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid scheduleId, CancellationToken ct)
    {
        var result = await _scheduleFeature.DeleteAsync(scheduleId, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return NoContent();
    }

    [HttpGet]
    [Route("/amenity/{amenityId:guid}")]
    public async Task<IActionResult> GetAllByAmenityIdAsync(Guid amenityId, CancellationToken ct)
    {
        var result = await _scheduleFeature.GetAllByAmenityIdAsync(amenityId,ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
}