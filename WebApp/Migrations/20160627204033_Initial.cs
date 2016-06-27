using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrewLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    BoilTimeInMinutes = table.Column<int>(nullable: false),
                    MashOutTemp = table.Column<float>(nullable: false),
                    MashTemp = table.Column<float>(nullable: false),
                    MashTimeInMinutes = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SpargeTemp = table.Column<float>(nullable: false),
                    StrikeTemp = table.Column<float>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TargetTemp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Target1 = table.Column<float>(nullable: false),
                    Target2 = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetTemp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Heater1Percentage = table.Column<float>(nullable: false),
                    Heater2Percentage = table.Column<float>(nullable: false),
                    Temp1 = table.Column<float>(nullable: false),
                    Temp2 = table.Column<float>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrewLogSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    BrewId = table.Column<int>(nullable: false),
                    CompleteButtonText = table.Column<string>(nullable: true),
                    CompleteTime = table.Column<DateTime>(nullable: true),
                    GetTargetMashTemp = table.Column<float>(nullable: false),
                    GetTargetSpargeTemp = table.Column<float>(nullable: false),
                    Instructions = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewLogSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrewLogSteps_BrewLogs_BrewId",
                        column: x => x.BrewId,
                        principalTable: "BrewLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrewLogSteps_BrewId",
                table: "BrewLogSteps",
                column: "BrewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrewLogSteps");

            migrationBuilder.DropTable(
                name: "TargetTemp");

            migrationBuilder.DropTable(
                name: "TempLogs");

            migrationBuilder.DropTable(
                name: "BrewLogs");
        }
    }
}
