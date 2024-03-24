using RoomService.Enums;

namespace RoomService.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public DateTime DateIn { get; set; }
    public DateTime DateOut { get; set; }
    public ReservationMethod MadeBy { get; set; }
    public ReservationStatus Status { get; set; }
    public Guid HotelBranchId { get; set; }
    public HotelBranch HotelBranch { get; set; }
    public Guid ReservationCreatorId { get; set; }
    public ICollection<OccupiedRoom> OccupiedRooms { get; set; } = new HashSet<OccupiedRoom>();
}