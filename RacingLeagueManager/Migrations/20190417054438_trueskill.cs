using Microsoft.EntityFrameworkCore.Migrations;

namespace RacingLeagueManager.Migrations
{
    public partial class trueskill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrueSkillRanked",
                table: "Series",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "TrueSkillConservativeRating",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TrueSkillMean",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TrueSkillStandardDeviation",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrueSkillRanked",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "TrueSkillConservativeRating",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrueSkillMean",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrueSkillStandardDeviation",
                table: "AspNetUsers");
        }
    }
}
