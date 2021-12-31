using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using TicketsBooking.Integration.Email.Models;

namespace TicketsBooking.Integration.Email
{
    [TransientService]
    public interface IMailService
    {
        Task SendEmailAsync(MailModel mailModel);
    }
}
