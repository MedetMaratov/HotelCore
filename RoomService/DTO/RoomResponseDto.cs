using RoomService.Models;

namespace RoomService.DTO;

public class RoomResponseDto
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public Guid RoomTypeId { get; set; }
    public string RoomTypeTitle { get; set; }
    public Guid HotelBranchId { get; set; }
    public string HotelBranchTitle { get; set; }
    public bool IsDisabled { get; set; }
}