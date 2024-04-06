using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.Amenity;

namespace WebApp.Controllers;

public class AmenityController : Controller
{
    private readonly HttpClient _httpClient;

    public AmenityController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:44318/api/v1/");
    }

    public async Task<IActionResult> Index(string searchString, CancellationToken ct)
    {
        HttpResponseMessage response;
        if (string.IsNullOrEmpty(searchString))
        {
            response = await _httpClient.GetAsync("amenities", ct);

        }
        else
        {
            response = await _httpClient.GetAsync($"amenities/{searchString}", ct);
        }
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(ct);
        var amenities = JsonConvert.DeserializeObject<List<AmenityViewModel>>(responseContent);

        return View(amenities);
    }

    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAmenityViewModel amenityViewModel, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var requestContent = JsonContent.Create(amenityViewModel);
        var response = await _httpClient.PostAsync("amenities", requestContent, ct);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"amenities/{id}", ct);
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var amenityViewModel = await response.Content.ReadFromJsonAsync<EditAmenityViewModel>(cancellationToken: ct);
        if (amenityViewModel == null)
        {
            return NotFound();
        }

        return View(amenityViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditAmenityViewModel viewModel, CancellationToken ct)
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
        var response = await _httpClient.PutAsync($"amenities", requestContent, ct);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        return StatusCode((int)response.StatusCode);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteConfirmation(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"amenities/{id}", ct);
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var amenityViewModel = await response.Content.ReadFromJsonAsync<DeleteAmenityViewModel>(cancellationToken: ct);
        
        if (amenityViewModel == null)
        {
            return NotFound();
        }

        return View(amenityViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.DeleteAsync($"amenities/{id}", ct);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        return StatusCode((int)response.StatusCode);
    }
}

