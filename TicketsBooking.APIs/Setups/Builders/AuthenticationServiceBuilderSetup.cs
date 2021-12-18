using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;

namespace TicketsBooking.APIs.Setups.Builders
{
    public static class AuthenticationAndAuthorizationBuilderSetup
    {
        public static void SetupApplicationAuthenticationAndAuthorization(this IApplicationBuilder app)
        {
            app.Use(async (httpContext, func) =>
            {
                var apiMode = httpContext.Request.Path.StartsWithSegments("/api");

                if (apiMode)
                {
                    //httpContext.Request.Headers[HeaderNames.Authorization];
                }
                await func();
            });

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
