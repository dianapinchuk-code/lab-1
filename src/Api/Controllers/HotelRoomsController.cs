using Api.Dtos;
using Application.Rooms.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("rooms")]
[ApiController]
public class HotelRoomsController(IRoomService roomService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoomDto>>> GetRooms(CancellationToken cancellationToken)
    {
        var rooms = await roomService.GetRooms(cancellationToken);
        return Ok(rooms.Select(RoomDto.FromDomainModel).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoomDto>> GetRoom(Guid id, CancellationToken cancellationToken)
    {
        var room = await roomService.GetRoom(id, cancellationToken);
        if (room is null)
        {
            return NotFound();
        }

        return RoomDto.FromDomainModel(room);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] CreateRoomDto request, CancellationToken cancellationToken)
    {
        try
        {
            var room = await roomService.Add(request.RoomNumber, request.PricePerNight, request.Capacity, request.IsAvailable, cancellationToken);
            var dto = RoomDto.FromDomainModel(room);
            return CreatedAtAction(nameof(GetRoom), new { id = dto.Id }, dto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RoomDto>> UpdateRoom(Guid id, [FromBody] UpdateRoomDto request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await roomService.Update(id, request.RoomNumber, request.PricePerNight, request.Capacity, request.IsAvailable, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(RoomDto.FromDomainModel(updated));
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoom(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await roomService.Delete(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}