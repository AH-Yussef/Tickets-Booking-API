using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.ComponentModel;
using MailKit.Net.Smtp;
using MimeKit;
using TicketsBooking.Integration.Email.Models;

// this is a first trial
namespace TicketsBooking.Integration.Email
{
	internal class SendEmailLogic : IEmailCommunication
	{
		MimeMessage message = new MimeMessage();

		MailboxAddress from = new MailboxAddress("Admin", "admin@example.com");

        public Task SendAsync(SendEmailModel email)
        {
            throw new NotImplementedException();
        }
        //Console.WriteLine("This is C#");

    }
}
