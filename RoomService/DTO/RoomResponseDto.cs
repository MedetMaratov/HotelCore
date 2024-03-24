using RoomService.Models;

namespace RoomService.DTO;

public class RoomResponseDto
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public RoomType Type { get; set; }
    public Guid HotelBranchId { get; set; }
}