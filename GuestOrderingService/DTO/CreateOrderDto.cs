using GuestOrderingService.Enums;
using GuestOrderingService.Models;

namespace GuestOrderingService.DTO;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public Guid RoomId { get; set; }
}