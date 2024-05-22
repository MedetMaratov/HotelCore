using GuestOrderingService.Enums;
using GuestOrderingService.Models;
using GuestOrderingService.Models.Service;

namespace GuestOrderingService.DTO;

public class ResponseOrderDto
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime RequestTime { get; set; }
    public string Status { get; set; }
    public DateTime? CompleteTime { get; set; }
}