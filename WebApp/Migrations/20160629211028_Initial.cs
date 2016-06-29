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
                name: "BrewStepTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    CompleteButtonText = table.Column<string>(nullable: true),
                    CompleteTimeAdd = table.Column<string>(nullable: true),
                    Instructions = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Target1TempFrom = table.Column<string>(nullable: true),
                    Target2TempFrom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrewStepTemplates", x => x.Id);
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
                    Instructions = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    TargetMashTemp = table.Column<float>(nullable: false),
                    TargetSpargeTemp = table.Column<float>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "DataCaptureDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    BrewStepTemplateId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    Units = table.Column<string>(nullable: true),
                    ValueType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCaptureDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataCaptureDefinitions_BrewStepTemplates_BrewStepTemplateId",
                        column: x => x.BrewStepTemplateId,
                        principalTable: "BrewStepTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataCaptureFloatValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    BrewLogStepId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    Units = table.Column<string>(nullable: true),
                    Value = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCaptureFloatValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataCaptureFloatValues_BrewLogSteps_BrewLogStepId",
                        column: x => x.BrewLogStepId,
                        principalTable: "BrewLogSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataCaptureIntValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    BrewLogStepId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    Units = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCaptureIntValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataCaptureIntValues_BrewLogSteps_BrewLogStepId",
                        column: x => x.BrewLogStepId,
                        principalTable: "BrewLogSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataCaptureStringValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    BrewLogStepId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Optional = table.Column<bool>(nullable: false),
                    Units = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataCaptureStringValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataCaptureStringValues_BrewLogSteps_BrewLogStepId",
                        column: x => x.BrewLogStepId,
                        principalTable: "BrewLogSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrewLogSteps_BrewId",
                table: "BrewLogSteps",
                column: "BrewId");

            migrationBuilder.CreateIndex(
                name: "IX_DataCaptureDefinitions_BrewStepTemplateId",
                table: "DataCaptureDefinitions",
                column: "BrewStepTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DataCaptureFloatValues_BrewLogStepId",
                table: "DataCaptureFloatValues",
                column: "BrewLogStepId");

            migrationBuilder.CreateIndex(
                name: "IX_DataCaptureIntValues_BrewLogStepId",
                table: "DataCaptureIntValues",
                column: "BrewLogStepId");

            migrationBuilder.CreateIndex(
                name: "IX_DataCaptureStringValues_BrewLogStepId",
                table: "DataCaptureStringValues",
                column: "BrewLogStepId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetTemp");

            migrationBuilder.DropTable(
                name: "TempLogs");

            migrationBuilder.DropTable(
                name: "DataCaptureDefinitions");

            migrationBuilder.DropTable(
                name: "DataCaptureFloatValues");

            migrationBuilder.DropTable(
                name: "DataCaptureIntValues");

            migrationBuilder.DropTable(
                name: "DataCaptureStringValues");

            migrationBuilder.DropTable(
                name: "BrewStepTemplates");

            migrationBuilder.DropTable(
                name: "BrewLogSteps");

            migrationBuilder.DropTable(
                name: "BrewLogs");
        }
    }
}
