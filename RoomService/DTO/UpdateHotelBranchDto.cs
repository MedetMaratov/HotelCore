using RoomService.Models;

namespace RoomService.DTO;

public class UpdateHotelBranchDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string? HouseNumber { get; set; }
}