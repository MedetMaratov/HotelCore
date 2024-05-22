using GuestOrderingService.DTO;
using GuestOrderingService.Features;
using Microsoft.AspNetCore.Mvc;

namespace GuestOrderingService.Controllers;

[Route("api/v1/services")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly ServiceFeature _serviceFeature;

    public ServiceController(ServiceFeature serviceFeature)
    {
        _serviceFeature = serviceFeature;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateServiceDto dto, CancellationToken ct)
    {
        var result = await _serviceFeature.CreateAsync(dto, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateServiceDto dto, CancellationToken ct)
    {
        var result = await _serviceFeature.UpdateAsync(dto, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result);
    }

    [HttpDelete]    
    [Route("{serviceId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid serviceId, CancellationToken ct)
    {
        var result = await _serviceFeature.DeleteAsync(serviceId, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken ct)
    {
        var result = await _serviceFeature.GetAllAsync(ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var result = await _serviceFeature.GetByIdAsync(id, ct);
        if (result.IsFailed)
            return BadRequest(result.Reasons);
        return Ok(result.Value);
    }
}