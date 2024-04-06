namespace RoomService.Models;

public class Room
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public Guid TypeId { get; set; }
    public RoomType Type { get; set; }
    public bool IsDisabled { get; set; }
    public Guid HotelBranchId { get; set; }
    public HotelBranch? HotelBranch { get; set; }

    public void Disable()
    {
        IsDisabled = true;
    }
    public void Enable()
    {
        IsDisabled = false;
    }
}