using Microsoft.AspNetCore.Builder;

namespace TicketsBooking.APIs.Setups.Builders
{
    public static class SwaggerBuilderSetup
    {
        public static void SetupSwagger(this IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketsBooking Application");
                c.InjectStylesheet("/swagger-ui/swagger.dark.css");
            });
        }
    }
}
