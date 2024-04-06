using RoomService.Models;

namespace RoomService.DTO;

public class CreateRoomTypeDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<ImageForRoomType> Images { get; set; }
    public int MaxCapacity { get; set; }
    public decimal NightlyRate { get; set; }
    public ICollection<Guid> Amenities { get; set; } = new List<Guid>();}