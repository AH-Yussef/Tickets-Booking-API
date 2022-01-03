using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Components.Customers.DTOs.Query;

namespace TicketsBooking.Application.Components.Customers.Validators
{
    public class GetAllQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllQueryValidator()
        {
            RuleFor(c => c.pageNumber).NotNull().NotEmpty();
            RuleFor(c => c.pageSize).NotNull().NotEmpty();
            RuleFor(c => c.searchTarget).NotNull().NotEmpty();
        }
    }
}
