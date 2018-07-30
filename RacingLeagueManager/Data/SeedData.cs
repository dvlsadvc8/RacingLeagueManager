using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using RacingLeagueManager.Data.Models;
using System.IO;
using System.Text;

namespace RacingLeagueManager.Data
{
    public static class SeedData
    {
        private static IServiceProvider m_ServiceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            m_ServiceProvider = serviceProvider;

            using (var context = new RacingLeagueManagerContext(
                serviceProvider.GetRequiredService<DbContextOptions<RacingLeagueManagerContext>>()))
            {
                // look for any tracks
                if (context.Track.Any())
                {
                    return; // db has been seeded
                }

                context.Track.AddRange(
                    new Track
                    {
                        Name = "Catalyuna GP"
                    },
                    new Track
                    {
                        Name = "Catalyuna Short"
                    },
                    new Track
                    {
                        Name = "Sonoma Full"
                    }
                );

                context.SaveChanges();
            }

            GenerateDGML();
        }

        private static void GenerateDGML()
        {
            using (var dbContext = m_ServiceProvider.GetRequiredService<RacingLeagueManagerContext>())
            {
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Entities.dgml", dbContext.AsDgml(), Encoding.UTF8);
            }
        }
    }
}
