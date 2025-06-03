using FluentValidation;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceCommandValidator()
    {
        RuleFor(command => command.TicketTypeId).NotEmpty();
        RuleFor(command => command.Price).GreaterThan(decimal.Zero);
    }
}