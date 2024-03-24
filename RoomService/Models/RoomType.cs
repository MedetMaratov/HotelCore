namespace RoomService.Models;

public class RoomType
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public int MaxCapacity { get; set; }
    public decimal NightlyRate { get; set; }
    public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}