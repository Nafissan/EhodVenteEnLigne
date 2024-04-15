using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EhodBoutiqueEnLigne.Data;
using EhodBoutiqueEnLigne.Models;
using System;

namespace EhodBoutiqueEnLigne
{
    internal static class DbInitializerExtension
    {
        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app, IConfiguration config)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<EhodBDD>();
                context.Database.Migrate();
                var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                identityContext.Database.Migrate();
                SeedData.Initialize(services, config);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }

            return app;
        }
    }
}
