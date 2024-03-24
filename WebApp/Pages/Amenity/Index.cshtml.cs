// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Newtonsoft.Json;
// using WebApp.Models;
//
// namespace WebApp.Pages.Amenity
// {
//     
//     public class IndexModel : PageModel
//     {
//         public AmenitiesListViewModel? Amenities { get; set; } = new AmenitiesListViewModel();
//         
//         public async Task OnGet()
//         {
//             using var httpClient = new HttpClient();
//             try
//             {
//                 const string apiUrl = $"http://localhost:5056/api/v1/amenities";
//                 var response = await httpClient.GetAsync(apiUrl);
//
//                 if (response.IsSuccessStatusCode)
//                 {
//                     var jsonContent = await response.Content.ReadAsStringAsync();
//                     Amenities = JsonConvert.DeserializeObject<AmenitiesListViewModel>(jsonContent);
//                 }
//                 else
//                 {
//                     
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Amenities = new AmenitiesListViewModel()
//                 {
//                     Amenities = new List<AmenityListItemViewModel>()
//                     {
//                         new AmenityListItemViewModel()
//                         {
//                             Id = Guid.NewGuid(),
//                             Name = "WI-FI"
//                         },
//                         new AmenityListItemViewModel()
//                         {
//                             Id = Guid.NewGuid(),
//                             Name = "SPA"
//                         }
//                     }
//                 };
//             }
//         }
//     }
// }
