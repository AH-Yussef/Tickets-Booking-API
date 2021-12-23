using System;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;

namespace TicketsBooking.Application.Components.Admins
{
    [ScopedService]
    public interface IAdminService
    {
        Task<OutputResponse<AuthedUserResult>> Authenticate(AuthCreds authCreds);
    }
}
