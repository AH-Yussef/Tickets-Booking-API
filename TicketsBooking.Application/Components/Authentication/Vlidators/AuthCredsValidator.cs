using FluentValidation;

namespace TicketsBooking.Application.Components.Authentication.Vlidators
{
    public class AuthCredsValidator : AbstractValidator<AuthCreds>
    {
        public AuthCredsValidator()
        {
            RuleFor(creds => creds.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(creds => creds.Password).NotNull().NotEmpty();                                         
        }
    }
}
