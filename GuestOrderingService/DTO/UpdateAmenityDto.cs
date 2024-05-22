namespace GuestOrderingService.DTO;

public class UpdateAmenityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CateroryId { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
}