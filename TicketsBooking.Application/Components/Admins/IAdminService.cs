using System;
using System.Threading.Tasks;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;

namespace TicketsBooking.Application.Components.Admins
{
    public interface IAdminService
    {
        Task<OutputResponse<AuthedUserResult>> Authenticate(AuthCreds authCreds);
    }
}
