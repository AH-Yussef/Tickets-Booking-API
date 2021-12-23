using System;
using System.Threading.Tasks;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Components.Admins
{
    public interface IAdminRepo
    {
        Task<Admin> GetSingle(string email);
    }
}
