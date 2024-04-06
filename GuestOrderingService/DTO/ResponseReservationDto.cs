namespace GuestOrderingService.DTO;

public class ResponseReservationDto
{
    public Guid Id { get; set; }
    public Guid GuestId { get; set; }
    public Guid AmenityScheduleId { get; set; }
    public string AmenityName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int NumberOfPersons { get; set; }
}