using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Application.Components.Authentication;
using TicketsBooking.Application.Components.Customers.DTOs.Command;
using TicketsBooking.Application.Components.Customers.DTOs.Query;
using TicketsBooking.Application.Components.Customers.DTOs.Results;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Customers
{
    [ScopedService]
    public interface ICustomerRepo
    {
        Task<Customer> Register(RegisterCustomerCommand command);
        Task<List<Customer>> GetAll(GetAllUsersQuery query);
        Task<Customer> GetSingleByName(string name);
        Task<Customer> GetSingleByEmail(string email);
        // when a user registers an email is sent to his email with an approving link
        // when he clicks on it the account gets approved and he can log in next time
        Task<bool> Approve(string Email);
        // will be implemented in phase 3.....
        Task<Customer> UpdateInfo();
        Task<bool> Delete(string Email);
    }
}
