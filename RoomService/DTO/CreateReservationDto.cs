using RoomService.Enums;

namespace RoomService.DTO;

public class CreateReservationDto
{
    public Guid ReservationCreatorId { get; set; }
    public Guid HotelBranchId { get; set; }
    public RoomsForReserveDto[] RoomsForReserveDtos { get; set; }
    public DateTime DateIn { get; set; }
    public DateTime DateOut { get; set; }
    public ReservationMethod ReservationMethod { get; set; }
}