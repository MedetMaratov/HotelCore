namespace RoomService.DTO;

public class CreateRoomDto
{
    public string Number { get; set; }
    public Guid TypeId { get; set; }
    public Guid HotelBranchId { get; set; }
}