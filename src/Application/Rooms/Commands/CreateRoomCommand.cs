using Application.Common.Interfaces.Repositories;
using Application.Rooms.Exceptions;
using Domain.Rooms;
using LanguageExt;
using MediatR;

namespace Application.Rooms.Commands;

public record CreateRoomCommand : IRequest<Either<RoomException, HotelRoom>>
{
    public required string RoomNumber { get; init; }
    public required decimal PricePerNight { get; init; }
    public required int Capacity { get; init; }
    public required bool IsAvailable { get; init; }
}

public class CreateRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<CreateRoomCommand, Either<RoomException, HotelRoom>>
{
    public async Task<Either<RoomException, HotelRoom>> Handle(
        CreateRoomCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await roomRepository.GetByRoomNumber(request.RoomNumber, cancellationToken);
        return await existing.MatchAsync(
            p => (Either<RoomException, HotelRoom>)new RoomAlreadyExistsException(p.Id),
            () => CreateEntity(request, cancellationToken));
    }

    private async Task<Either<RoomException, HotelRoom>> CreateEntity(
        CreateRoomCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var room = HotelRoom.New(RoomId.New(), request.RoomNumber, request.PricePerNight, request.Capacity, request.IsAvailable);
            return await roomRepository.Add(room, cancellationToken);
        }
        catch (Exception exception)
        {
            return new RoomUnhandledException(RoomId.Empty(), exception);
        }
    }
}