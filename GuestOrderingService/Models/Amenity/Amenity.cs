namespace GuestOrderingService.Models.Amenity;

public class Amenity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Schedule> Schedules { get; set; }
}