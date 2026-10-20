using Domain.Rooms;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IRoomQueries
{
    Task<IReadOnlyList<HotelRoom>> GetAll(CancellationToken cancellationToken);
    Task<Option<HotelRoom>> GetById(RoomId id, CancellationToken cancellationToken);
}