using Microsoft.AspNetCore.Mvc;
using RoomService.DTO;
using RoomService.Interfaces;

namespace RoomService.Controllers;

[Route("api/v1/hotel-branch")]
[ApiController]
public class HotelBranchController : Controller
{
    private readonly IHotelBranchService _hotelBranchService;

    public HotelBranchController(IHotelBranchService hotelBranchService)
    {
        _hotelBranchService = hotelBranchService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateHotelBranchDto hotelBranchDto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest();
        await _hotelBranchService.CreateAsync(hotelBranchDto, ct);
        return Ok();

    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromBody] UpdateHotelBranchDto hotelBranchDto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest();
        await _hotelBranchService.UpdateAsync(hotelBranchDto, ct);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(Guid hotelBranchId, CancellationToken ct)
    {
        await _hotelBranchService.DeleteAsync(hotelBranchId, ct);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseHotelBranchDto>>> GetAllAsync(CancellationToken ct)
    {
        var hotelBranches = await _hotelBranchService.GetAllAsync(ct);
        return Ok(hotelBranches);
    }

    [HttpGet]
    [Route("{country}/{city}")]
    public async Task<ActionResult<IEnumerable<ResponseHotelBranchDto>>> GetAllByLocationAsync(
        string country,
        string city, 
        CancellationToken ct)
    {
        var hotelBranches = await _hotelBranchService.GetAllByLocationAsync(country, city, ct);
        return Ok(hotelBranches);
    }
}