using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Common.Responses;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Authentication.DTOs;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Application.Components.Customers.DTOs.Query;
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Customers
{
    [ScopedService]
    public interface ICustomerService
    {
        Task<OutputResponse<AuthedUserResult>> Authenticate(AuthCreds authCreds);
        Task<OutputResponse<bool>> Register(RegisterCustomerCommand command);
        Task<OutputResponse<List<GetAllUserListedResult>>> GetAll(GetAllUsersQuery query);
        Task<OutputResponse<GetSingleUserResult>> GetSingle(string Email);
        Task<OutputResponse<bool>> Approve(AcceptCustomerCommand command);
        Task<OutputResponse<bool>> Delete(string Email);
    }
}
