using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsBooking.Application.Components.Customers.DTOs.Command;

namespace TicketsBooking.Application.Components.Customers.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(c => c.Email).NotNull().NotEmpty();
            RuleFor(c => c.Name).NotNull().NotEmpty();
            RuleFor(c => c.Password).NotNull().NotEmpty();
        }
    }
}
