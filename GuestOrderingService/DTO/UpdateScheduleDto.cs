namespace GuestOrderingService.DTO;

public class UpdateScheduleDto
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Capacity { get; set; }
}