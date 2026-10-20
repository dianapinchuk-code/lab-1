using Api.Dtos;
using Api.Modules.Errors;
using Application.Common.Interfaces.Queries;
using Application.Rooms.Commands;
using Domain.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("rooms")]
[ApiController]
public class HotelRoomsController(
    ISender sender,
    IRoomQueries roomQueries) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetRooms(CancellationToken cancellationToken)
    {
        var rooms = await roomQueries.GetAll(cancellationToken);
        return Ok(rooms.Select(RoomDto.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomDto>> GetRoom(Guid id, CancellationToken cancellationToken)
    {
        var room = await roomQueries.GetById(new RoomId(id), cancellationToken);
        return room.Match<ActionResult<RoomDto>>(
            p => RoomDto.FromDomainModel(p),
            () => NotFound());
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> CreateRoom(
        [FromBody] CreateRoomDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateRoomCommand
        {
            RoomNumber = request.RoomNumber,
            PricePerNight = request.PricePerNight,
            Capacity = request.Capacity,
            IsAvailable = request.IsAvailable
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<RoomDto>>(
            p => CreatedAtAction(nameof(GetRoom), new { id = p.Id.Value }, RoomDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RoomDto>> UpdateRoom(
        Guid id,
        [FromBody] UpdateRoomDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateRoomCommand
        {
            RoomId = id,
            RoomNumber = request.RoomNumber,
            PricePerNight = request.PricePerNight,
            Capacity = request.Capacity,
            IsAvailable = request.IsAvailable
        };

        var result = await sender.Send(command, cancellationToken);
        return result.Match<ActionResult<RoomDto>>(
            p => Ok(RoomDto.FromDomainModel(p)),
            e => e.ToObjectResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoom(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteRoomCommand { RoomId = id }, cancellationToken);
        return result.Match<IActionResult>(
            _ => NoContent(),
            e => e.ToObjectResult());
    }
}