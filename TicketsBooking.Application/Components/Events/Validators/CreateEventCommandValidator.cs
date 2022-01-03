using System;
using FluentValidation;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
namespace TicketsBooking.Application.Components.Events.Validators
{
    public class CreateEventCommandValidator : AbstractValidator<CreateNewEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(c => c.Title).NotNull().NotEmpty();
            RuleFor(c => c.Description).NotNull().NotEmpty();
            RuleFor(c => c.AllTickets).NotNull().NotNull();
            RuleFor(c => c.BoughtTickets).NotNull().NotEmpty();
            RuleFor(c => c.ReservationDueDate).NotNull().NotEmpty();
            RuleFor(c => c.Location).NotNull().NotEmpty();
            RuleFor(c => c.Category).NotNull().NotEmpty();
            RuleFor(c => c.SubCategory).NotNull().NotEmpty();
            RuleFor(c => c.Accepted).NotNull().NotEmpty();
            RuleFor(c => c.Participants).NotNull().NotEmpty();
        }
    }
}
