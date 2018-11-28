using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RacingLeagueManager.Data;

namespace RacingLeagueManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<RacingLeagueManagerContext>();

                    // TODO: A production app would not call Database.Migrate. 
                    // It's added to the preceding code to prevent the following exception when Update-Database has not been run:
                    // SqlException: Cannot open database "RazorPagesMovieContext-21" requested by the login. The login failed.Login failed for user 'user name'.
                    context.Database.Migrate();

                    SeedData.Initialize(services);
                }
                catch(Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseSetting("https_port", "8080")
                .UseStartup<Startup>();
    }
}
