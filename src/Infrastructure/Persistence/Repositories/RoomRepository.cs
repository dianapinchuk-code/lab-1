using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Rooms;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoomRepository(ApplicationDbContext context) : IRoomRepository, IRoomQueries
{
    public async Task<IReadOnlyList<HotelRoom>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Rooms.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Option<HotelRoom>> GetById(RoomId id, CancellationToken cancellationToken)
    {
        var entity = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity ?? Option<HotelRoom>.None;
    }

    public async Task<Option<HotelRoom>> GetByRoomNumber(string roomNumber, CancellationToken cancellationToken)
    {
        var entity = await context.Rooms.FirstOrDefaultAsync(x => x.RoomNumber == roomNumber, cancellationToken);
        return entity ?? Option<HotelRoom>.None;
    }

    public async Task<HotelRoom> Add(HotelRoom room, CancellationToken cancellationToken)
    {
        await context.Rooms.AddAsync(room, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return room;
    }

    public async Task<HotelRoom> Update(HotelRoom room, CancellationToken cancellationToken)
    {
        context.Rooms.Update(room);
        await context.SaveChangesAsync(cancellationToken);
        return room;
    }

    public async Task Delete(HotelRoom room, CancellationToken cancellationToken)
    {
        context.Rooms.Remove(room);
        await context.SaveChangesAsync(cancellationToken);
    }
}