using Application.Common.Interfaces;
using Application.Rooms.Services.Abstract;
using Domain.Rooms;

namespace Application.Rooms.Services.Implementation;

public class RoomService(IRoomRepository roomRepository) : IRoomService
{
    public async Task<IReadOnlyList<HotelRoom>> GetRooms(CancellationToken cancellationToken)
    {
        return await roomRepository.GetAll(cancellationToken);
    }

    public async Task<HotelRoom?> GetRoom(Guid id, CancellationToken cancellationToken)
    {
        return await roomRepository.GetById(id, cancellationToken);
    }

    public async Task<HotelRoom> Add(string roomNumber, decimal pricePerNight, int capacity, bool isAvailable, CancellationToken cancellationToken)
    {
        var existingRoom = await roomRepository.GetByRoomNumber(roomNumber, cancellationToken);
        if (existingRoom is not null)
        {
            throw new ArgumentException($"Room with number '{roomNumber}' already exists");
        }

        var room = HotelRoom.New(Guid.NewGuid(), roomNumber, pricePerNight, capacity, isAvailable);
        return await roomRepository.Add(room, cancellationToken);
    }

    public async Task<HotelRoom?> Update(Guid id, string roomNumber, decimal pricePerNight, int capacity, bool isAvailable, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetById(id, cancellationToken);
        if (room is null)
        {
            return null;
        }

        var roomWithSameNumber = await roomRepository.GetByRoomNumber(roomNumber, cancellationToken);
        if (roomWithSameNumber is not null && roomWithSameNumber.Id != id)
        {
            throw new ArgumentException($"Room with number '{roomNumber}' already exists");
        }

        room.UpdateDetails(roomNumber, pricePerNight, capacity, isAvailable);
        return await roomRepository.Update(room, cancellationToken);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetById(id, cancellationToken);
        if (room is null)
        {
            return false;
        }

        await roomRepository.Delete(room, cancellationToken);
        return true;
    }
}