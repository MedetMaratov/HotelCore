namespace RoomService.Models;

public class Location
{
    public Guid Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string? HouseNumber { get; set; }
}