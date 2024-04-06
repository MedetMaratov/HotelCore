using FluentResults;
using RoomService.DTO;

namespace RoomService.Interfaces;

public interface IHotelBranchService
{
    Task<Result<Guid>> CreateAsync(CreateHotelBranchDto hotelBranchDto, CancellationToken ct);
    Task<Result<Guid>> UpdateAsync(UpdateHotelBranchDto hotelBranchDto, CancellationToken ct);
    Task<Result<Guid>> DeleteAsync(Guid hotelBranchId, CancellationToken ct);
    Task<Result<IEnumerable<ResponseHotelBranchDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<ResponseHotelBranchDto>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Result<IEnumerable<ResponseHotelBranchDto>>> GetAllByLocationAsync(string country, string city, CancellationToken ct);
}