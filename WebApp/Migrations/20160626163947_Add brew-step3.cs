using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class Addbrewstep3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrewLogSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    BrewId = table.Column<int>(nullable: false),
                    CompleteButtonText = table.Column<string>(nullable: true),
                    GetTargetMashTemp = table.Column<float>(nullable: false),
                    GetTargetSpargeTemp = table.Column<float>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ShowTimer = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewLogSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrewLogSteps_Brews_BrewId",
                        column: x => x.BrewId,
                        principalTable: "Brews",
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
        }
    }
}
