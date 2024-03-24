namespace GuestOrderingService.DTO;

public class UpdateServiceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime WaitingTime { get; set; }
    public decimal Price { get; set; }
}