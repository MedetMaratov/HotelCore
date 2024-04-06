using RoomService.Models;

namespace RoomService.DTO;

public class ResponsePhoneNumberDto
{
    public string Number { get; set; }
    public Guid HotelBranchId { get; set; }
    public HotelBranch HotelBranch { get; set; }
    public string Department { get; set; }
}