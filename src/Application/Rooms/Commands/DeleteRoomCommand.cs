using Application.Common.Interfaces.Repositories;
using Application.Rooms.Exceptions;
using Domain.Rooms;
using LanguageExt;
using MediatR;

namespace Application.Rooms.Commands;

public record DeleteRoomCommand : IRequest<Either<RoomException, HotelRoom>>
{
    public required Guid RoomId { get; init; }
}

public class DeleteRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<DeleteRoomCommand, Either<RoomException, HotelRoom>>
{
    public async Task<Either<RoomException, HotelRoom>> Handle(
        DeleteRoomCommand request,
        CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);
        var room = await roomRepository.GetById(roomId, cancellationToken);

        return await room.MatchAsync(
            async p =>
            {
                try
                {
                    await roomRepository.Delete(p, cancellationToken);
                    return (Either<RoomException, HotelRoom>)p;
                }
                catch (Exception ex)
                {
                    return (Either<RoomException, HotelRoom>)new RoomUnhandledException(roomId, ex);
                }
            },
            () => (Either<RoomException, HotelRoom>)new RoomNotFoundException(roomId));
    }
}