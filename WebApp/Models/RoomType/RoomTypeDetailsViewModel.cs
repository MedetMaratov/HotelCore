using WebApp.Models.Amenity;

namespace WebApp.Models.RoomType;

public class RoomTypeDetailsViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int MaxCapacity { get; set; }
    public decimal NightlyRate { get; set; }
    public List<string> ImagePathes { get; set; }
    public ICollection<AmenityViewModel> Amenities { get; set; } = new List<AmenityViewModel>();
}