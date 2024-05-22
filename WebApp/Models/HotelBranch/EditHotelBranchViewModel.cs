namespace WebApp.Models.HotelBranch;

public class EditHotelBranchViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string? HouseNumber { get; set; }
    public string Contacts { get; set; }
}