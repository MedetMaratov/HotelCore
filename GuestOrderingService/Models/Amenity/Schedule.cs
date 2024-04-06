namespace GuestOrderingService.Models.Amenity;

public class Schedule
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid AmenityId { get; set; }
    public Amenity Amenity { get; set; }
    public int? Capacity { get; set; }
    public int? RemainingCapacity { get; set; }
}