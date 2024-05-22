namespace GuestOrderingService.DTO;

public class ResponseAmenityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CateroryId { get; set; }
    public string CategoryName { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
}