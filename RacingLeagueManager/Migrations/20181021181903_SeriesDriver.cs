using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RacingLeagueManager.Migrations
{
    public partial class SeriesDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeriesDriver",
                columns: table => new
                {
                    DriverId = table.Column<Guid>(nullable: false),
                    SeriesId = table.Column<Guid>(nullable: false),
                    LeagueId = table.Column<Guid>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesDriver", x => new { x.DriverId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_SeriesDriver_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesDriver_LeagueDriver_LeagueId_DriverId",
                        columns: x => new { x.LeagueId, x.DriverId },
                        principalTable: "LeagueDriver",
                        principalColumns: new[] { "LeagueId", "DriverId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeriesEntryDriver_DriverId",
                table: "SeriesEntryDriver",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesDriver_SeriesId",
                table: "SeriesDriver",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesDriver_LeagueId_DriverId",
                table: "SeriesDriver",
                columns: new[] { "LeagueId", "DriverId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesEntryDriver_AspNetUsers_DriverId",
                table: "SeriesEntryDriver",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesEntryDriver_AspNetUsers_DriverId",
                table: "SeriesEntryDriver");

            migrationBuilder.DropTable(
                name: "SeriesDriver");

            migrationBuilder.DropIndex(
                name: "IX_SeriesEntryDriver_DriverId",
                table: "SeriesEntryDriver");
        }
    }
}
