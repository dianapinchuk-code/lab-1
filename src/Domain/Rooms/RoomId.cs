namespace Domain.Rooms;

public record RoomId(Guid Value)
{
    public static RoomId Empty() => new(Guid.Empty);
    public static RoomId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}