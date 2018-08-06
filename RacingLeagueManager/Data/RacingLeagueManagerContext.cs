using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RacingLeagueManager.Data.Models;

namespace RacingLeagueManager.Data
{
    public class RacingLeagueManagerContext : IdentityDbContext<Driver, IdentityRole<Guid>, Guid>
    {
        public RacingLeagueManagerContext (DbContextOptions<RacingLeagueManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Models.League> League { get; set; }
        public DbSet<Models.Track> Track { get; set; }
        public DbSet<Models.Car> Car { get; set; }
        public DbSet<Models.SeriesEntry> SeriesEntry { get; set; }
        public DbSet<Models.Race> Race { get; set; }
        public DbSet<Models.Series> Series { get; set; }
        public DbSet<Models.LeagueDriver> LeagueDriver { get; set; }
        public DbSet<Models.RaceResult> RaceResult { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<Models.LeagueDriver>()
                .HasKey(ld => new { ld.LeagueId, ld.DriverId });

            builder.Entity<Models.SeriesEntry>()
                .HasKey(se => new { se.SeriesId, se.LeagueId, se.DriverId });

            builder.Entity<SeriesEntry>()
                .HasOne(se => se.LeagueDriver)
                .WithMany(ld => ld.SeriesEntries)
                .HasForeignKey(se => new { se.LeagueId, se.DriverId })
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SeriesEntry>()
                .HasOne(se => se.Series)
                .WithMany(s => s.Entries)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RaceResult>()
                .HasOne(r => r.SeriesEntry)
                .WithMany(se => se.Results)
                .HasForeignKey(r => new { r.SeriesId, r.LeagueId, r.DriverId })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RaceResult>()
                .HasOne(r => r.Race)
                .WithMany(r => r.Results)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
