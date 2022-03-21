using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TicketsBooking.Application.Components.Purchases.DTOs.Commands;

namespace TicketsBooking.Application.Components.Purchases.Validators
{
    public class CreatePurchaseCommandValidator : AbstractValidator<CreateNewPurchaseCommand>
    {
        public CreatePurchaseCommandValidator()
        {
            RuleFor(c => c.EventID).NotNull().NotEmpty();
            RuleFor(c => c.CustomerID).NotNull().NotEmpty();
            RuleFor(c => c.TicketsCount).NotNull().NotEmpty();
        }
    }
}
