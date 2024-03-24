using RoomService.Models;

namespace RoomService.DTO;

public class UpdateRoomDto
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public RoomType Type { get; set; }
}