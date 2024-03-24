using RoomService.Models;

namespace RoomService.DTO;

public class UpdateRoomTypeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public int MaxCapacity { get; set; }
    public decimal NightlyRate { get; set; }
    public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}