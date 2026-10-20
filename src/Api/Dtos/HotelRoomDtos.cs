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
        => new(room.Id.Value, room.RoomNumber, room.PricePerNight, room.Capacity, room.IsAvailable, room.CreatedAt);
}

public record CreateRoomDto(
    string RoomNumber,
    decimal PricePerNight,
    int Capacity,
    bool IsAvailable);

public record UpdateRoomDto(
    string RoomNumber,
    decimal PricePerNight,
    int Capacity,
    bool IsAvailable);