using RoomService.Models;

namespace RoomService.DTO;

public class ResponseHotelBranchDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Location Location { get; set; }
    public string Contacts { get; set; }
}