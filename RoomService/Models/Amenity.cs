namespace RoomService.Models;

public class Amenity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<RoomType> RoomTypes { get; } = new List<RoomType>();
}