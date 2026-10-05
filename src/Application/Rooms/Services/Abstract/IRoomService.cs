using Domain.Rooms;

namespace Application.Rooms.Services.Abstract;

public interface IRoomService
{
    Task<IReadOnlyList<HotelRoom>> GetRooms(CancellationToken cancellationToken);
    Task<HotelRoom?> GetRoom(Guid id, CancellationToken cancellationToken);
    Task<HotelRoom> Add(string roomNumber, decimal pricePerNight, int capacity, bool isAvailable, CancellationToken cancellationToken);
    Task<HotelRoom?> Update(Guid id, string roomNumber, decimal pricePerNight, int capacity, bool isAvailable, CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}