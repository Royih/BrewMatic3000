using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApp.Model;

namespace WebApp.Migrations
{
    [DbContext(typeof(BrewMaticContext))]
    partial class BrewMaticContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901");

            modelBuilder.Entity("WebApp.Model.BrewLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BoilTimeInMinutes");

                    b.Property<float>("MashOutTemp");

                    b.Property<float>("MashTemp");

                    b.Property<int>("MashTimeInMinutes");

                    b.Property<string>("Name");

                    b.Property<float>("SpargeTemp");

                    b.Property<float>("StrikeTemp");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("BrewLogs");
                });

            modelBuilder.Entity("WebApp.Model.BrewLogStep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrewId");

                    b.Property<string>("CompleteButtonText");

                    b.Property<DateTime?>("CompleteTime");

                    b.Property<float>("GetTargetMashTemp");

                    b.Property<float>("GetTargetSpargeTemp");

                    b.Property<string>("Instructions");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("Id");

                    b.HasIndex("BrewId");

                    b.ToTable("BrewLogSteps");
                });

            modelBuilder.Entity("WebApp.Model.BrewTargetTemperature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Target1");

                    b.Property<float>("Target2");

                    b.HasKey("Id");

                    b.ToTable("TargetTemp");
                });

            modelBuilder.Entity("WebApp.Model.BrewTempLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Heater1Percentage");

                    b.Property<float>("Heater2Percentage");

                    b.Property<float>("Temp1");

                    b.Property<float>("Temp2");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("TempLogs");
                });

            modelBuilder.Entity("WebApp.Model.BrewLogStep", b =>
                {
                    b.HasOne("WebApp.Model.BrewLog")
                        .WithMany()
                        .HasForeignKey("BrewId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
