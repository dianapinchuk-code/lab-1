using Domain.Rooms;

namespace Application.Common.Interfaces;

public interface IRoomRepository
{
    Task<IReadOnlyList<HotelRoom>> GetAll(CancellationToken cancellationToken);
    Task<HotelRoom?> GetById(Guid id, CancellationToken cancellationToken);
    Task<HotelRoom?> GetByRoomNumber(string roomNumber, CancellationToken cancellationToken);
    Task<HotelRoom> Add(HotelRoom room, CancellationToken cancellationToken);
    Task<HotelRoom> Update(HotelRoom room, CancellationToken cancellationToken);
    Task Delete(HotelRoom room, CancellationToken cancellationToken);
}