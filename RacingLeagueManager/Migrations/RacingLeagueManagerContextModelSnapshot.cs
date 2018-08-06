﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RacingLeagueManager.Data;

namespace RacingLeagueManager.Migrations
{
    [DbContext(typeof(RacingLeagueManagerContext))]
    partial class RacingLeagueManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.Driver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.League", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<bool>("IsPublic");

                    b.Property<string>("Name");

                    b.Property<Guid>("OwnerId");

                    b.HasKey("Id");

                    b.ToTable("League");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.LeagueDriver", b =>
                {
                    b.Property<Guid>("LeagueId");

                    b.Property<Guid>("DriverId");

                    b.Property<TimeSpan>("PreQualifiedTime");

                    b.Property<int>("RaceNumber");

                    b.Property<int?>("TrueSkillRating");

                    b.HasKey("LeagueId", "DriverId");

                    b.HasIndex("DriverId");

                    b.ToTable("LeagueDriver");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.Race", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Laps");

                    b.Property<DateTime>("RaceDate");

                    b.Property<Guid>("SeriesId");

                    b.Property<Guid>("TrackId");

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.HasIndex("TrackId");

                    b.ToTable("Race");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.RaceResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("DriverId");

                    b.Property<Guid>("LeagueId");

                    b.Property<int>("Place");

                    b.Property<int>("Points");

                    b.Property<Guid>("RaceId");

                    b.Property<Guid>("SeriesId");

                    b.HasKey("Id");

                    b.HasIndex("RaceId");

                    b.HasIndex("SeriesId", "LeagueId", "DriverId");

                    b.ToTable("RaceResult");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.Series", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<Guid>("LeagueId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.SeriesEntry", b =>
                {
                    b.Property<Guid>("SeriesId");

                    b.Property<Guid>("LeagueId");

                    b.Property<Guid>("DriverId");

                    b.Property<Guid>("CarId");

                    b.HasKey("SeriesId", "LeagueId", "DriverId");

                    b.HasIndex("CarId");

                    b.HasIndex("LeagueId", "DriverId");

                    b.ToTable("SeriesEntry");
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.Track", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Track");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("RacingLeagueManager.Data.Models.Driver")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("RacingLeagueManager.Data.Models.Driver")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RacingLeagueManager.Data.Models.Driver")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("RacingLeagueManager.Data.Models.Driver")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.LeagueDriver", b =>
                {
                    b.HasOne("RacingLeagueManager.Data.Models.Driver", "Driver")
                        .WithMany("LeagueDrivers")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RacingLeagueManager.Data.Models.League", "League")
                        .WithMany("LeagueDrivers")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.Race", b =>
                {
                    b.HasOne("RacingLeagueManager.Data.Models.Series", "Series")
                        .WithMany("Races")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RacingLeagueManager.Data.Models.Track", "Track")
                        .WithMany("Races")
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.RaceResult", b =>
                {
                    b.HasOne("RacingLeagueManager.Data.Models.Race", "Race")
                        .WithMany("Results")
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RacingLeagueManager.Data.Models.SeriesEntry", "SeriesEntry")
                        .WithMany("Results")
                        .HasForeignKey("SeriesId", "LeagueId", "DriverId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.Series", b =>
                {
                    b.HasOne("RacingLeagueManager.Data.Models.League", "League")
                        .WithMany("Series")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RacingLeagueManager.Data.Models.SeriesEntry", b =>
                {
                    b.HasOne("RacingLeagueManager.Data.Models.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RacingLeagueManager.Data.Models.Series", "Series")
                        .WithMany("Entries")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RacingLeagueManager.Data.Models.LeagueDriver", "LeagueDriver")
                        .WithMany("SeriesEntries")
                        .HasForeignKey("LeagueId", "DriverId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
