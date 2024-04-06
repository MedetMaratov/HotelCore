namespace RoomService.DTO;

public class RoomsForReserveDto
{
    public Guid RoomTypeId { get; set; }
    public DateTime ReservationStart { get; set; }
    public DateTime ReservationEnd { get; set; }
    public int Quantity { get; set; }
}