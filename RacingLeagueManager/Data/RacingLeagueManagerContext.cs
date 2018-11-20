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
    public class RacingLeagueManagerContext : IdentityDbContext<Driver, IdentityRole<Guid>, Guid>//, Role, Guid> //
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
        public DbSet<Models.SeriesEntryDriver> SeriesEntryDriver { get; set; }
        public DbSet<Models.Team> Team { get; set; }
        public DbSet<Models.Rule> Rule { get; set; }
        public DbSet<Models.Penalty> Penalty { get; set; }
        public DbSet<Models.SeriesDriver> SeriesDriver { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<Models.LeagueDriver>()
                .HasKey(ld => new { ld.LeagueId, ld.DriverId });

            builder.Entity<Models.LeagueDriver>()
                .Property(l => l.Status)
                .HasDefaultValue("Pending");


            builder.Entity<Models.SeriesEntryDriver>()
                .HasKey(s => new { s.LeagueId, s.DriverId, s.SeriesEntryId });

            builder.Entity<Models.SeriesEntryDriver>()
                .HasOne(sed => sed.Driver)
                .WithMany(d => d.SeriesEntryDrivers)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Models.SeriesDriver>()
                .HasKey(s => new { s.DriverId, s.SeriesId });

            builder.Entity<Models.SeriesDriver>()
                .HasOne(s => s.LeagueDriver)
                .WithMany(ld => ld.SeriesDrivers)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Models.LeagueDriver>()
                .HasMany(ld => ld.SeriesDrivers)
                .WithOne(s => s.LeagueDriver)
                .OnDelete(DeleteBehavior.Restrict);
                

            //builder.Entity<SeriesEntry>()
            //    .HasOne(se => se.LeagueDriver)
            //    .WithMany(ld => ld.SeriesEntries)
            //    .HasForeignKey(se => new { se.LeagueId, se.DriverId })
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SeriesEntry>()
                .HasOne(se => se.Series)
                .WithMany(s => s.SeriesEntries)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Series>()
                .HasOne(s => s.Owner)
                .WithMany(o => o.OwnedSeries)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<League>()
            //    .HasOne(l => l.Owner)
            //    .WithOne(o => o.OwnedLeague)
            //    .HasForeignKey<Driver>(l => l)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Driver>()
                .HasOne(d => d.OwnedLeague)
                .WithOne(o => o.Owner)
                .HasForeignKey<League>(l => l.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SeriesEntry>()
                .HasOne(s => s.Team)
                .WithMany(t => t.SeriesEntries)
                .OnDelete(DeleteBehavior.Restrict);



            //builder.Entity<RaceResult>()
            //    .HasOne(r => r.SeriesEntry)
            //    .WithMany(se => se.RaceResults)
            //    .HasForeignKey(r => new { r.SeriesId, r.LeagueId, r.DriverId })
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<RaceResult>()
            //    .HasOne(r => r.Race)
            //    .WithMany(r => r.Results)
            //    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
