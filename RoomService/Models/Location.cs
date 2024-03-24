namespace RoomService.Models;

public class Location
{
    public Guid Id { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public string? HouseNumber { get; set; }
}