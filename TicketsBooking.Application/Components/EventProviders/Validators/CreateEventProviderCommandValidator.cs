using System;
using FluentValidation;
using TicketsBooking.Application.Components.EventProviders.DTOs.Commands;

namespace TicketsBooking.Application.Components.EventProviders.Validators
{
    public class CreateEventProviderCommandValidator : AbstractValidator<CreateEventProviderCommand>
    {
        public CreateEventProviderCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty();
            RuleFor(c => c.Password).NotNull().NotEmpty();
            RuleFor(c => c.Bio).NotNull().NotNull();
            RuleFor(c => c.Email).NotNull().NotEmpty();
        }
    }
}