using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApp.Model;

namespace WebApp.Migrations
{
    [DbContext(typeof(BrewMaticContext))]
    [Migration("20160626162037_Add brew-step")]
    partial class Addbrewstep
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20896");

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

                    b.ToTable("Brews");
                });

            modelBuilder.Entity("WebApp.Model.BrewStatusLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Heater1Percentage");

                    b.Property<float>("Heater2Percentage");

                    b.Property<float>("Temp1");

                    b.Property<float>("Temp2");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("Logs");
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
        }
    }
}
