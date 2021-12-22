using System;
using FluentValidation;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;

namespace TicketsBooking.Application.Components.EventProviders.Validators
{
    public class SetVerifiedCommandValidator: AbstractValidator<SetVerifiedCommand>
    {
        public SetVerifiedCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty();
            RuleFor(c => c.Verified).NotNull().NotEmpty();
        }
    }
}
