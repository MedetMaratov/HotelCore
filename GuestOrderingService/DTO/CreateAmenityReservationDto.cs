namespace GuestOrderingService.DTO;

public class CreateAmenityReservationDto
{
    public Guid GuestId { get; set; }
    public Guid AmenityId { get; set; }
    public DateTime DateTime { get; set; }
}