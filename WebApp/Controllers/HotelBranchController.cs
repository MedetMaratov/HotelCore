using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.HotelBranch;

namespace WebApp.Controllers;

public class HotelBranchController : Controller
{
    private readonly HttpClient _httpClient;

    public HotelBranchController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:44318/api/v1/");
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync("hotel-branch", ct);
        
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(ct);
        var hotelBranches = JsonConvert.DeserializeObject<List<HotelBranchListItemViewModel>>(responseContent);

        return View(hotelBranches);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateHotelBranchViewModel viewModel, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var requestContent = JsonContent.Create(viewModel);
        var response = await _httpClient.PostAsync("hotel-branch", requestContent, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode);
        }

        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"hotel-branch/{id}", ct);
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var hotelBranchDto = await response.Content.ReadFromJsonAsync<HotelBranchListItemViewModel>(cancellationToken: ct);
        if (hotelBranchDto == null)
        {
            return NotFound();
        }
        var hotelBranchViewModel = new EditHotelBranchViewModel
        {
            Id = hotelBranchDto.Id,
            Name = hotelBranchDto.Name,
            Country = hotelBranchDto.Location.Country,
            City = hotelBranchDto.Location.City,
            Street = hotelBranchDto.Location.Street,
            HouseNumber = hotelBranchDto.Location.HouseNumber
        };
        
        return View(hotelBranchViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditHotelBranchViewModel viewModel, CancellationToken ct)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var requestContent = JsonContent.Create(viewModel);
        var response = await _httpClient.PutAsync($"hotel-branch", requestContent, ct);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        return StatusCode((int)response.StatusCode);
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteConfirmation(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"hotel-branch/{id}", ct);
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var hotelBranchViewModel = await response.Content.ReadFromJsonAsync<DeleteHotelBranchViewModel>(cancellationToken: ct);
        
        if (hotelBranchViewModel == null)
        {
            return NotFound();
        }

        return View(hotelBranchViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.DeleteAsync($"hotel-branch/{id}", ct);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return StatusCode((int)response.StatusCode);
    }
    
    [HttpGet]
    public IActionResult Details()
    {
        throw new NotImplementedException();
    }
}