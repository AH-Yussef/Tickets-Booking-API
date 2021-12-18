using Microsoft.AspNetCore.Builder;

namespace TicketsBooking.APIs.Setups.Factory
{
    public interface IApplicationSetup
    {
        void SetupApplication(IApplicationBuilder app);
    }
}
