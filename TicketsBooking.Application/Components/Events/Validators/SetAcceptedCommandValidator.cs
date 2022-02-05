using System;
using FluentValidation;
using TicketsBooking.Application.Components.Events.DTOs.Commands;

namespace TicketsBooking.Application.Components.Events.Validators
{
    public class SetAcceptedCommandValidator : AbstractValidator<SetAcceptedCommand>
    {
        public SetAcceptedCommandValidator()
        {
            RuleFor(c => c.ID).NotNull();
            RuleFor(c => c.Accepted).NotEmpty();
        }
    }
}