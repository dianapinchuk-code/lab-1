namespace Domain.Rooms;

public class HotelRoom
{
    public RoomId Id { get; }
    public DateTime CreatedAt { get; }
    public string RoomNumber { get; private set; }
    public decimal PricePerNight { get; private set; }
    public int Capacity { get; private set; }
    public bool IsAvailable { get; private set; }

    private HotelRoom(RoomId id, string roomNumber, decimal pricePerNight, int capacity, bool isAvailable, DateTime createdAt)
    {
        Id = id;
        RoomNumber = roomNumber;
        PricePerNight = pricePerNight;
        Capacity = capacity;
        IsAvailable = isAvailable;
        CreatedAt = createdAt;
    }

    public static HotelRoom New(RoomId id, string roomNumber, decimal pricePerNight, int capacity, bool isAvailable)
        => new(id, roomNumber, pricePerNight, capacity, isAvailable, DateTime.UtcNow);

    public void UpdateDetails(string roomNumber, decimal pricePerNight, int capacity, bool isAvailable)
    {
        RoomNumber = roomNumber;
        PricePerNight = pricePerNight;
        Capacity = capacity;
        IsAvailable = isAvailable;
    }
}