using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.RoomType;

namespace WebApp.Controllers;

public class RoomTypeController : Controller
{
    private readonly HttpClient _httpClient;

    public RoomTypeController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:44318/api/v1/");
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync("room-types", ct);
        
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(ct);
        var roomTypes = JsonConvert.DeserializeObject<List<RoomTypeListItemViewModel>>(responseContent);

        return View(roomTypes);
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"room-types/{id}", ct);
        
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(ct);
        var roomType = JsonConvert.DeserializeObject<RoomTypeDetailsViewModel>(responseContent);

        return View(roomType);
    }
    
    public IActionResult Create()
    {
        throw new NotImplementedException();
    }

    public IActionResult DeleteConfirmation(Guid id)
    {
        throw new NotImplementedException();
    }

    public IActionResult Edit(Guid id)
    {
        throw new NotImplementedException();
    }
}