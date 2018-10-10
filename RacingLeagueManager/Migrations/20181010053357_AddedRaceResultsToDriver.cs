using Microsoft.EntityFrameworkCore.Migrations;

namespace RacingLeagueManager.Migrations
{
    public partial class AddedRaceResultsToDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResultType",
                table: "RaceResult",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultType",
                table: "RaceResult");
        }
    }
}
