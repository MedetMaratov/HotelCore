namespace RoomService.DTO;

public class UpdateHotelBranchDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid LocationId { get; set; }
}