using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace TicketsBooking.APIs.Setups.Factory
{
    public static class ApplicationSetupExtenstion
    {
        public static void InstallApplicationBuildersInAssembly(this IApplicationBuilder app)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes
                .Where(x => typeof(IApplicationSetup).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IApplicationSetup>().ToList();
            installers.ForEach(installer => installer.SetupApplication(app));
        }
    }
}
