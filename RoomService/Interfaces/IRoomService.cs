using FluentResults;
using RoomService.DTO;

namespace RoomService.Interfaces;

public interface IRoomService
{
    Task<Result<Guid>> CreateAsync(CreateRoomDto dto, CancellationToken ct);
    Task<Result<Guid>> UpdateAsync(UpdateRoomDto dto, CancellationToken ct);
    Task<Result<Guid>> DeleteAsync(Guid roomId, CancellationToken ct);
    Task<Result<IEnumerable<RoomResponseDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<IEnumerable<RoomResponseDto>>> GetRoomsByTypeAsync(Guid roomTypeId, CancellationToken ct);
}