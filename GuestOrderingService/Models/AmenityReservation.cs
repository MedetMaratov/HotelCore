namespace GuestOrderingService.Models;

public class AmenityReservation
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; }
    public DateTime DateTime { get; set; }
}