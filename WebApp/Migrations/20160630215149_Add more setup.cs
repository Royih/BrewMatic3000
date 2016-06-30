using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class Addmoresetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchSize",
                table: "BrewLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "MashWaterAmount",
                table: "BrewLogs",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SpargeWaterAmount",
                table: "BrewLogs",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchSize",
                table: "BrewLogs");

            migrationBuilder.DropColumn(
                name: "MashWaterAmount",
                table: "BrewLogs");

            migrationBuilder.DropColumn(
                name: "SpargeWaterAmount",
                table: "BrewLogs");
        }
    }
}
