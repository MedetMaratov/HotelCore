namespace RoomService.Models;

public class Room
{
    public Guid Id { get; set; }
    public required string Number { get; set; }
    public Guid TypeId { get; set; }
    public RoomType Type { get; set; }
    public Guid HotelBranchId { get; set; }
    public HotelBranch? HotelBranch { get; set; }
}