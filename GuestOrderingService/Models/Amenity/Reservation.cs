namespace GuestOrderingService.Models.Amenity;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public Guid AmenityScheduleId { get; set; }
    public Schedule Schedule { get; set; }
    public int NumberOfPersons { get; set; }
}