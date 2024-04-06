using GuestOrderingService.DTO;
using GuestOrderingService.Features;
using Microsoft.AspNetCore.Mvc;

namespace GuestOrderingService.Controllers;

[Route("api/v1/amenities")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly AmenityFeature _amenityFeature;

    public AmenityController(AmenityFeature amenityFeature)
    {
        _amenityFeature = amenityFeature;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateAmenityDto dto, CancellationToken ct)
    {
        var result = await _amenityFeature.CreateAsync(dto, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateAmenityDto dto, CancellationToken ct)
    {
        var result = await _amenityFeature.UpdateAsync(dto, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result);
    }

    [HttpDelete]    
    [Route("{amenityId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid amenityId, CancellationToken ct)
    {
        var result = await _amenityFeature.DeleteAsync(amenityId, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct)
    {
        var result = await _amenityFeature.GetAllAsync(ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("/{id:guid}")]
    public async Task<IActionResult> GetAllByAsync(Guid id, CancellationToken ct)
    {
        var result = await _amenityFeature.GetByIdAsync(id, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
}