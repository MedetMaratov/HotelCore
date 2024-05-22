namespace GuestOrderingService.DTO;

public class CreateServiceDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeOnly WaitingTime { get; set; }
    public string ImagePath { get; set; }
    public decimal Price { get; set; }
}