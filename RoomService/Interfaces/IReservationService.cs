using FluentResults;
using RoomService.DTO;
using RoomService.Models;

namespace RoomService.Interfaces;

public interface IReservationService
{
    Task<Result<Guid>> ReserveAsync(CreateReservationDto createReservationDto, CancellationToken ct);
    Task<Result<IEnumerable<ResponseReservationDto>>> GetAllAsync(CancellationToken ct);
}