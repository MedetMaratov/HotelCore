using GuestOrderingService.Enums;
using GuestOrderingService.Models;

namespace GuestOrderingService.DTO;

public class ResponseOrderDto
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime RequestTime { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime? CompleteTime { get; set; }
}