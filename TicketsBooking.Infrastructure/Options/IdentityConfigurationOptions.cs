namespace TicketsBooking.Infrastructure.Options
{
    public class IdentityConfigurationOptions
    {
        public Password Password { get; set; }
    }

    public class Password
    {
        public bool RequiredDigit { get; set; } = false;
        public bool RequireLowercase { get; set; } = false;
        public bool RequireNonAlphanumeric { get; set; } = false;
        public bool RequireUppercase { get; set; } = false;
        public int RequiredLength { get; set; } = 6;
        public int RequiredUniqueChars { get; set; } = 0;
    }
}