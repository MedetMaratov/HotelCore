using RoomService.Enums;
using RoomService.Models;

namespace RoomService.DTO;

public class ResponseReservationDto
{
    public Guid Id { get; set; }
    public DateTime DateIn { get; set; }
    public DateTime DateOut { get; set; }
    public ReservationMethod MadeBy { get; set; }
    public ReservationStatus Status { get; set; }
    public Guid HotelBranchId { get; set; }
    public HotelBranch HotelBranch { get; set; }
    public Guid ReservationCreatorId { get; set; }
}