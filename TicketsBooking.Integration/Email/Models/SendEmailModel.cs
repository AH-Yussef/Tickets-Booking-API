namespace TicketsBooking.Integration.Email.Models
{
    public class SendEmailModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
