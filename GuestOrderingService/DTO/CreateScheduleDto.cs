namespace GuestOrderingService.DTO;

public class CreateScheduleDto
{
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid AmenityId { get; set; }
    public int? Capacity { get; set; }
}