using RoomService.Models;

namespace RoomService.DTO;

public class UpdateRoomTypeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; }
    public int MaxCapacity { get; set; }
    public decimal NightlyRate { get; set; }
    public ICollection<Guid> Amenities { get; set; }
}