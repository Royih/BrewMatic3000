using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class Addedbrewlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brews",
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
                    table.PrimaryKey("PK_Brews", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brews");
        }
    }
}
