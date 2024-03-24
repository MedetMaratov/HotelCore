namespace WebApp.Models;

public class AmenitiesListViewModel
{
    public ICollection<AmenityListItemViewModel> Amenities { get; set; } = new List<AmenityListItemViewModel>();
}