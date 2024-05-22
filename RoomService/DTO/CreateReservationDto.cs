using RoomService.Enums;

namespace RoomService.DTO;

public class CreateReservationDto
{
    public Guid HotelBranchId { get; set; }
    public RoomForReserveDto RoomForReserveDtos { get; set; }
    public DateTime DateIn { get; set; }
    public DateTime DateOut { get; set; }
    public ReservationMethod ReservationMethod { get; set; }
    public string ReservatorFullName { get; set; }
    public string? ReservatorContactEmail { get; set; }
    public string? ReservatorContactPhoneNumber { get; set; }
    public IEnumerable<string> GuestNames { get; set; } 
}