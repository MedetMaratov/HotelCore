using FluentResults;
using RoomService.DTO;
using RoomService.Models;

namespace RoomService.Interfaces;

public interface IOccupiedRoomService
{
    OccupiedRoom Create(CreateOccupiedRoomDto occupiedRoomDto, CancellationToken ct);
    Task<Result<Guid>> SetCheckInAsync(Guid roomId, CancellationToken ct);
    Task<Result<Guid>> SetCheckOutAsync(Guid roomId, CancellationToken ct);
}