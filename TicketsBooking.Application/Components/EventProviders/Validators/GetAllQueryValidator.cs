using FluentValidation;
using TicketsBooking.Application.Components.EventProviders.DTOs.Queries;

namespace TicketsBooking.Application.Components.EventProviders.Validators
{
    public class GetAllQueryValidator : AbstractValidator<GetAllEventProvidersQuery>
    {
        public GetAllQueryValidator()
        {
            RuleFor(c => c.pageNumber).NotNull().NotEmpty();
            RuleFor(c => c.pageSize).NotNull().NotEmpty();
            RuleFor(c => c.isVerified).NotNull();
        }
    }
}