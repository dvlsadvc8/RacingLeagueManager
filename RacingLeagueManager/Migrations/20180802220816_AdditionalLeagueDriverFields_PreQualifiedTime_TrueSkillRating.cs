using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RacingLeagueManager.Migrations
{
    public partial class AdditionalLeagueDriverFields_PreQualifiedTime_TrueSkillRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "PreQualifiedTime",
                table: "LeagueDriver",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "TrueSkillRating",
                table: "LeagueDriver",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreQualifiedTime",
                table: "LeagueDriver");

            migrationBuilder.DropColumn(
                name: "TrueSkillRating",
                table: "LeagueDriver");
        }
    }
}
