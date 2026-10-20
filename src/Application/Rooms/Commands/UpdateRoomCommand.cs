using Application.Common.Interfaces.Repositories;
using Application.Rooms.Exceptions;
using Domain.Rooms;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Rooms.Commands;

public record UpdateRoomCommand : IRequest<Either<RoomException, HotelRoom>>
{
    public required Guid RoomId { get; init; }
    public required string RoomNumber { get; init; }
    public required decimal PricePerNight { get; init; }
    public required int Capacity { get; init; }
    public required bool IsAvailable { get; init; }
}

public class UpdateRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<UpdateRoomCommand, Either<RoomException, HotelRoom>>
{
    public async Task<Either<RoomException, HotelRoom>> Handle(
        UpdateRoomCommand request,
        CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);
        var room = await roomRepository.GetById(roomId, cancellationToken);

        return await room.MatchAsync(
            p => CheckDuplicates(p.Id, request.RoomNumber, cancellationToken)
                .BindAsync(_ => UpdateEntity(p, request, cancellationToken)),
            () => (Either<RoomException, HotelRoom>)new RoomNotFoundException(roomId));
    }

    private async Task<Either<RoomException, Unit>> CheckDuplicates(
        RoomId currentRoomId,
        string roomNumber,
        CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetByRoomNumber(roomNumber, cancellationToken);
        return room.Match<Either<RoomException, Unit>>(
            p => p.Id == currentRoomId ? Unit.Default : new RoomAlreadyExistsException(p.Id),
            () => Unit.Default);
    }

    private async Task<Either<RoomException, HotelRoom>> UpdateEntity(
        HotelRoom room,
        UpdateRoomCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            room.UpdateDetails(request.RoomNumber, request.PricePerNight, request.Capacity, request.IsAvailable);
            return await roomRepository.Update(room, cancellationToken);
        }
        catch (Exception exception)
        {
            return new RoomUnhandledException(room.Id, exception);
        }
    }
}