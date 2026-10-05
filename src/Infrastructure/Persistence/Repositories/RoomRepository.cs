using Application.Common.Interfaces;
using Domain.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoomRepository(ApplicationDbContext context) : IRoomRepository
{
    public async Task<IReadOnlyList<HotelRoom>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Rooms.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<HotelRoom?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context.Rooms.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<HotelRoom?> GetByRoomNumber(string roomNumber, CancellationToken cancellationToken)
    {
        return await context.Rooms.FirstOrDefaultAsync(x => x.RoomNumber == roomNumber, cancellationToken);
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