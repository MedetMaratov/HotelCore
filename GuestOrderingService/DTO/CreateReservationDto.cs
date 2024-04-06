namespace GuestOrderingService.DTO;

public class CreateReservationDto
{
    public Guid GuestId { get; set; }
    public Guid AmenityScheduleId { get; set; }
    public int NumberOfPersons { get; set; }
}