namespace RoomService.DTO;

public class RoomForReserveDto
{
    public Guid RoomTypeId { get; set; }
    public DateTime ReservationStart { get; set; }
    public DateTime ReservationEnd { get; set; }
}