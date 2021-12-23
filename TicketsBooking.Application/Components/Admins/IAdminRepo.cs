using System;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Admins
{
    [ScopedService]
    public interface IAdminRepo
    {
        Task<Admin> GetSingle(string email);
    }
}
