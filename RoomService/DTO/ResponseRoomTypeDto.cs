using RoomService.Models;

namespace RoomService.DTO;

public class ResponseRoomTypeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int MaxCapacity { get; set; }
    public decimal NightlyRate { get; set; }
    public List<string> ImagePathes { get; set; }
    public ICollection<string> Amenities { get; set; } = new List<string>();
}