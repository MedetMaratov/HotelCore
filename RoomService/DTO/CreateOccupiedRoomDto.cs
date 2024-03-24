namespace RoomService.DTO;

public class CreateOccupiedRoomDto
{
    public Guid RoomId { get; set; }
    public Guid ReservationId { get; set; }
}