namespace GuestOrderingService.Models.Amenity;

public class Amenity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
}