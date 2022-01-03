using System;
using FluentValidation;
using TicketsBooking.Application.Components.Events.DTOs.Commands;
using TicketsBooking.Application.Components.Events.DTOs.Queries;

namespace TicketsBooking.Application.Components.Events.Validators
{
    public class GetAllEventQueryValidator : AbstractValidator<GetAllEventsQuery>
    {
        public GetAllEventQueryValidator()
        {
            RuleFor(c => c.PageNumber).NotNull().NotEmpty();
            RuleFor(c => c.PageSize).NotNull().NotEmpty();
            RuleFor(c => c.Accepted).NotNull();
        }
    }
}
