using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.Amenity;
using WebApp.Models.RoomType;

namespace WebApp.Controllers;

[Authorize(Policy = "RequireAdminRole")]
public class RoomTypeAdminController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public RoomTypeAdminController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:44318/api/v1/");
        _webHostEnvironment = webHostEnvironment;
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

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync("amenities", ct);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(ct);
        var amenities = JsonConvert.DeserializeObject<List<AmenityViewModel>>(responseContent);

        var roomTypeViewModel = new RoomTypeCreateViewModel()
        {
            Amenities = amenities
        };
        return View(roomTypeViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoomTypeCreateViewModel viewModel, CancellationToken ct)
    {
        if (!ModelState.IsValid) return BadRequest();
        var createDto = new CreateRoomTypeDto()
        {
            Title = viewModel.Title,
            Description = viewModel.Description,
            Amenities = viewModel.SelectedAmenities,
            NightlyRate = viewModel.NightlyRate,
            MaxCapacity = viewModel.MaxCapacity,
            Images = new List<string>()
        };
        var files = HttpContext.Request.Form.Files;
        var webRootPath = _webHostEnvironment.WebRootPath;
        var upload = webRootPath + @"/images/roomType/";
        foreach (var file in files)
        {
            var fileName = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);
            await using(var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                await file.CopyToAsync(fileStream, ct);
                createDto.Images.Add(fileName+extension);
            }
        }
        
        var requestContent = JsonContent.Create(createDto);
        var response = await _httpClient.PostAsync("room-types", requestContent, ct);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var roomTypesResponse = await _httpClient.GetAsync($"room-types/{id}", ct);
        if (!roomTypesResponse.IsSuccessStatusCode) return StatusCode((int)roomTypesResponse.StatusCode);

        var responseContent = await roomTypesResponse.Content.ReadAsStringAsync(ct);
        
        var roomType = JsonConvert.DeserializeObject<RoomTypeEditViewModel>(responseContent);
        var amenitiesResponse = await _httpClient.GetAsync($"amenities", ct);
        if (!amenitiesResponse.IsSuccessStatusCode) return StatusCode((int)amenitiesResponse.StatusCode);

        var amenitiesResponseContent = await amenitiesResponse.Content.ReadAsStringAsync(ct);
        
        var amenities = JsonConvert.DeserializeObject<List<AmenityViewModel>>(amenitiesResponseContent);
        roomType.AvailableAmenities = amenities;
        return View(roomType);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, RoomTypeEditViewModel viewModel, CancellationToken ct)
    {
        if (id != viewModel.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var editDto = new EditRoomTypeDto()
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            Description = viewModel.Description,
            Images = new List<string>(),
            MaxCapacity = viewModel.MaxCapacity,
            NightlyRate = viewModel.NightlyRate,
            Amenities = viewModel.SelectedAmenities
        };
        
        var files = HttpContext.Request.Form.Files;
        if (files.Count > 0)
        {
            editDto.Images = new List<string>();
            var webRootPath = _webHostEnvironment.WebRootPath;
            var upload = webRootPath + @"/images/roomType/";
            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(file.FileName);
                await using (var fileStream =
                             new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream, ct);
                    editDto.Images.Add(fileName + extension);
                }
            }
        }

        var requestContent = JsonContent.Create(editDto);
        var response = await _httpClient.PutAsync("room-types", requestContent, ct);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        return StatusCode((int)response.StatusCode);
    }

    public async Task<IActionResult> DeleteConfirmation(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"room-types/{id}", ct);

        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(ct);
        var roomType = JsonConvert.DeserializeObject<RoomTypeDetailsViewModel>(responseContent);

        return View(roomType);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var response = await _httpClient.DeleteAsync($"room-types/{id}", ct);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        return StatusCode((int)response.StatusCode);
    }
}