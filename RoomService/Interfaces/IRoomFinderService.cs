using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RoomService.DTO;

namespace RoomService.Interfaces;

public interface IRoomFinderService
{
    Guid GetRoomIdsForReservationAsync(RoomForReserveDto roomForReserveDto);
}