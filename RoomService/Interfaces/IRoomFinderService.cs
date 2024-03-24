using RoomService.DTO;

namespace RoomService.Interfaces;

public interface IRoomFinderService
{
    Task<IEnumerable<Guid>> GetRoomIdsForReservationAsync(RoomsForReserveDto[] roomsForReserveDto, CancellationToken ct);
}