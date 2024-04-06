using FluentResults;
using RoomService.DTO;

namespace RoomService.Interfaces;

public interface IRoomTypeService
{
    Task<Result<Guid>> CreateAsync(CreateRoomTypeDto roomTypeDto, CancellationToken ct);
    Task<Result<Guid>> UpdateAsync(UpdateRoomTypeDto roomType, CancellationToken ct);
    Task<Result<Guid>> DeleteAsync(Guid roomId, CancellationToken ct);
    Task<Result<IEnumerable<ResponseRoomTypeDto>>> GetAllRoomTypesAsync(CancellationToken ct);
    Task<Result<ResponseRoomTypeDto>> GetRoomTypeByIdAsync(Guid id, CancellationToken ct);
}