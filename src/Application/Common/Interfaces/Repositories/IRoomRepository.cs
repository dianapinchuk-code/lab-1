using Domain.Rooms;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<Option<HotelRoom>> GetById(RoomId id, CancellationToken cancellationToken);
    Task<Option<HotelRoom>> GetByRoomNumber(string roomNumber, CancellationToken cancellationToken);
    Task<HotelRoom> Add(HotelRoom room, CancellationToken cancellationToken);
    Task<HotelRoom> Update(HotelRoom room, CancellationToken cancellationToken);
    Task Delete(HotelRoom room, CancellationToken cancellationToken);
}