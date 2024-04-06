using GuestOrderingService.Enums;

namespace GuestOrderingService.Models.Service;

public class Order
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public Service? Service { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime RequestTime { get; private set; } = DateTime.Now;
    public OrderStatus Status { get; private set; } = OrderStatus.InProgress;
    public DateTime? CompleteTime { get; private set; } = null;
    public DateTime? CancelTime { get; private set; } = null;

    public void Cancel()
    {
        this.Status = OrderStatus.Canceled;
        this.CancelTime = DateTime.Now;
    }

    public void Complete()
    {
        this.Status = OrderStatus.Completed;
        this.CompleteTime = DateTime.Now;;
    }
}