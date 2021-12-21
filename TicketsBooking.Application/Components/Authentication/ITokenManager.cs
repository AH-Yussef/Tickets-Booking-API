using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Authentication
{
    [ScopedService]
    public interface ITokenManager
    {
        public string GenerateToken(User user, string role);
    }
}
