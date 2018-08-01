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

                #region Tracks

                context.Track.AddRange(
                    new Track
                    {
                        Name = "Bathurst Mount Panorama Circuit"
                    },
                    new Track
                    {
                        Name = "Bernese Alps Festival Circuit"
                    },
                    new Track
                    {
                        Name = "Bernese Alps Stadtplatz Circuit"
                    },
                    new Track
                    {
                        Name = "Bernese Alps Club Circuit"
                    },
                    new Track
                    {
                        Name = "Bernese Alps Festival Circuit Reverse"
                    },
                    new Track
                    {
                        Name = "Bernese Alps Stadtplatz Circuit Reverse"
                    },
                    new Track
                    {
                        Name = "Bernese Alps Club Circuit Reverse"
                    }
                );

                context.SaveChanges();

                #endregion

                #region Cars

                context.Car.AddRange(
                    new Car
                    {
                        Name = "Aston Martin V12 Vantage GT3 (2017)"
                    },
                    new Car
                    {
                        Name = "Audi R8 LMS Ultra (2014)"
                    },
                    new Car
                    {
                        Name = "Bentley Continental GT3 (2017)"
                    },
                    new Car
                    {
                        Name = "BMW M6 GTLM (2017)"
                    },
                    new Car
                    {
                        Name = "Chevrolet Corvette C7.R (2014)"
                    },
                    new Car
                    {
                        Name = "Dodge Viper GTS-R (2014)"
                    },
                    new Car
                    {
                        Name = "Jaguar XK GT3 (2014)"
                    },
                    new Car
                    {
                        Name = "Lamborghini Huracán LP620-2 Super Trofeo (2015)"
                    },
                    new Car
                    {
                        Name = "McLaren 12C GT3 (2014)"
                    },
                    new Car
                    {
                        Name = "Mercedes-Benz SLS AMG GT3 (2014)"
                    },
                    new Car
                    {
                        Name = "Nissan GT-R (2015)"
                    },
                    new Car
                    {
                        Name = "Porsche 911 RSR (2017)"
                    }
                );

                context.SaveChanges();

                #endregion
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
