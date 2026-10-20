using FluentValidation;

namespace Application.Rooms.Commands;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50);

        RuleFor(x => x.PricePerNight)
            .InclusiveBetween(0.01m, 100_000m);

        RuleFor(x => x.Capacity)
            .InclusiveBetween(1, 20);
    }
}