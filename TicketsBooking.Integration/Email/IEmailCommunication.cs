using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Integration.Email.Models;

namespace TicketsBooking.Integration.Email
{
    [SingletonService]
    public interface IEmailCommunication
    {
        Task SendAsync(SendEmailModel email);
    }
}
