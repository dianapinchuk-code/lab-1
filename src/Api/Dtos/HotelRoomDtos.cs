using System.ComponentModel.DataAnnotations;
using Domain.Rooms;

namespace Api.Dtos;

public record RoomDto(
    Guid Id,
    string RoomNumber,
    decimal PricePerNight,
    int Capacity,
    bool IsAvailable,
    DateTime CreatedAt)
{
    public static RoomDto FromDomainModel(HotelRoom room)
        => new(room.Id, room.RoomNumber, room.PricePerNight, room.Capacity, room.IsAvailable, room.CreatedAt);
}

public record CreateRoomDto(
    [Required, MaxLength(50)] string RoomNumber,
    [Range(0.01, 100_000)] decimal PricePerNight,
    [Range(1, 20)] int Capacity,
    bool IsAvailable);

public record UpdateRoomDto(
    [Required, MaxLength(50)] string RoomNumber,
    [Range(0.01, 100_000)] decimal PricePerNight,
    [Range(1, 20)] int Capacity,
    bool IsAvailable);