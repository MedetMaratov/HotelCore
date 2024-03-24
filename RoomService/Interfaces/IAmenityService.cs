using FluentResults;
using RoomService.DTO;

namespace RoomService.Interfaces;

public interface IAmenityService
{
    Task<Result<Guid>> CreateAsync(CreateAmenityDto createAmenityDto, CancellationToken ct);
    Task<Result<Guid>> UpdateAsync(UpdateAmenityDto updateAmenityDto, CancellationToken ct);
    Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken ct);
    Task<Result<IEnumerable<ResponseAmenityDto>>> GetAllAsync(CancellationToken ct);
}