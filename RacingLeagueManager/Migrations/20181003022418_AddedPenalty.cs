using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RacingLeagueManager.Migrations
{
    public partial class AddedPenalty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Penalty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RaceResultId = table.Column<Guid>(nullable: false),
                    Seconds = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penalty_RaceResult_RaceResultId",
                        column: x => x.RaceResultId,
                        principalTable: "RaceResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Penalty_RaceResultId",
                table: "Penalty",
                column: "RaceResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Penalty");
        }
    }
}
