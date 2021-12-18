using System.Collections.Generic;
using TicketsBooking.APIs.Setups.Factory;
using TicketsBooking.APIs.Setups.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TicketsBooking.APIs.Setups.Services
{
    public class SwaggerServiceSetup : IServiceSetup
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var swaggerOptions = new SwaggerSettings();
            configuration.Bind(nameof(swaggerOptions),swaggerOptions);
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.SwaggerDoc(swaggerOptions.Version,new OpenApiInfo()
                {
                    Title = swaggerOptions.Title,
                    Version = swaggerOptions.Version,
                    Description = swaggerOptions.Description
                });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,new OpenApiSecurityScheme()
                {
                    Description = "Jwt Authorization Header Using Bearer Scheme.\n" +
                                  "Enter 'Bearer' [space] and then your token in the text input below\n" +
                                  "Example: \"Bearer 123186168601681651casd8a6w1d68as1d86as4d\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
