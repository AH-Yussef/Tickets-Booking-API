using System;
using FluentValidation;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Commands;

namespace TicketsBooking.Application.Components.EventProviders.Validators
{
    public class SetAcceptedCommandValidator : AbstractValidator<SetAcceptedCommand>
    {
        public SetAcceptedCommandValidator()
        {
            RuleFor(c => c.ID).NotNull().NotEmpty();
            RuleFor(c => c.Accepted).NotNull();
        }
    }
}