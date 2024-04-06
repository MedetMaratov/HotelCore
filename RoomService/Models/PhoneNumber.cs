using System.ComponentModel.DataAnnotations;

namespace RoomService.Models;

public class PhoneNumber
{
    [Key]
    public string Number { get; set; }
    public Guid HotelBranchId { get; set; }
    public HotelBranch HotelBranch { get; set; }
    public string Department { get; set; }
}