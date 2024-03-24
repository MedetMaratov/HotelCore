namespace GuestOrderingService.Models;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime WaitingTime { get; set; }
    public decimal Price { get; set; }
}