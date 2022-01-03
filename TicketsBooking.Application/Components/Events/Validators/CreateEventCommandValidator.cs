using System;
using FluentValidation;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
namespace TicketsBooking.Application.Components.Events.Validators
{
    public class CreateEventCommandValidator : AbstractValidator<CreateNewEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(c => c.Title).NotNull();
            RuleFor(c => c.Description).NotNull();
            RuleFor(c => c.AllTickets).NotNull();
            RuleFor(c => c.SingleTicketPrice).NotNull();
            RuleFor(c => c.ReservationDueDate).NotNull();
            RuleFor(c => c.Location).NotNull();
            RuleFor(c => c.Category).NotNull();
            RuleFor(c => c.SubCategory).NotNull();
            RuleFor(c => c.Participants).NotNull();
        }
    }
}
