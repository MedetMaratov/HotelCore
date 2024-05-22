using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.Models.HotelBranch;
using WebApp.Models.RoomType;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;


    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:44318/api/v1/");
    }

    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync("hotel-branch", ct);

        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(ct);
        var hotelBranches = JsonConvert.DeserializeObject<List<HotelBranchListItemViewModel>>(responseContent);
        var viewModel = new HomeViewModel()
        {
            HotelBranches = hotelBranches
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> AvailableRoomTypes(HomeViewModel homeViewModel, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync(
            $"room-types/available/{homeViewModel.SelectedHotelBranchId}/{homeViewModel.ReservationStart.ToString("yyyy-MM-dd")}/{homeViewModel.ReservationEnd.ToString("yyyy-MM-dd")}/{homeViewModel.NumberOfGuests}",
            ct);

        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(ct);
        var roomTypes = JsonConvert.DeserializeObject<List<RoomTypeDetailsViewModel>>(responseContent);
        var availableRoomTypes = new AvailableRoomTypesForReservation()
        {
            Rooms = roomTypes,
            NumberOfGuests = homeViewModel.NumberOfGuests,
            ReservationStart = homeViewModel.ReservationStart,
            ReservationEnd = homeViewModel.ReservationEnd,
            SelectedHotelBranchId = homeViewModel.SelectedHotelBranchId
        };
        return View(availableRoomTypes);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}