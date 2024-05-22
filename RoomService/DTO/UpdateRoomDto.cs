using RoomService.Models;

namespace RoomService.DTO;

public class UpdateRoomDto
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public bool IsDisabled { get; set; }
    public Guid TypeId { get; set; }
}