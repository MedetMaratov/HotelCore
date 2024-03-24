namespace RoomService.Models;

public class Amenity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public ICollection<RoomType> RoomTypes { get; set; }
}