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
        // when a user registers an email is sent to his email with an approving link
        // when he clicks on it the account gets approved and he can log in next time
        Task<OutputResponse<bool>> Approve(string Email);
        //will be implemented in phase 3...
        Task<OutputResponse<Customer>> UpdateInfo();
        Task<OutputResponse<bool>> Delete(string Email);

        //Task<OutputResponse<bool>> SetIntrests(Email,List<string> intrests);
    }
}
