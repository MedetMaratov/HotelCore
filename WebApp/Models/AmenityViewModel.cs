namespace WebApp.Models;

public class AmenityViewModel
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}