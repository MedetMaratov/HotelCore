using RoomService.Models;

namespace RoomService.DTO;

public class RequestPhoneNumberDto
{
    public string Number { get; set; }
    public Guid HotelBranchId { get; set; }
    public HotelBranch HotelBranch { get; set; }
    public string Department { get; set; }
}