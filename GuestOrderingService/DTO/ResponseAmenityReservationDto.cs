namespace GuestOrderingService.DTO;

public class ResponseAmenityReservationDto
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public Guid AmenityId { get; set; }
    public DateTime DateTime { get; set; }
}