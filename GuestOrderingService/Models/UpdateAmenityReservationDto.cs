namespace GuestOrderingService.Models;

public class UpdateAmenityReservationDto
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public Guid AmenityId { get; set; }
    public DateTime DateTime { get; set; }
}