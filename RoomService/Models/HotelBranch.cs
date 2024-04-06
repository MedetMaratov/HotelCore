namespace RoomService.Models;

public class HotelBranch
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
}