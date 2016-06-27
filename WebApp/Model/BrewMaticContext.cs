using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebApp.Model
{
    public class BrewMaticContext : DbContext
    {
        public DbSet<BrewStatusLog> Logs { get; set; }
        public DbSet<BrewLog> Brews { get; set; }
        public DbSet<BrewLogStep> BrewLogSteps { get; set; }
        public DbSet<BrewTargetTemperature> TargetTemp { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./brew.db");
        }
    }



    public class BrewStatusLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public float Temp1 { get; set; }
        public float Temp2 { get; set; }

        public float Heater1Percentage { get; set; }

        public float Heater2Percentage { get; set; }
    }

    public class BrewLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Name { get; set; }
        public float MashTemp { get; set; }
        public float StrikeTemp { get; set; }
        public float SpargeTemp { get; set; }
        public float MashOutTemp { get; set; }
        public int MashTimeInMinutes { get; set; }
        public int BoilTimeInMinutes { get; set; }
    }

    public class BrewLogStep
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BrewLog")]
        public int BrewId { get; set; }
        public virtual BrewLog BrewLog { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public bool ShowTimer { get; set; }
        public float GetTargetMashTemp { get; set; }
        public float GetTargetSpargeTemp { get; set; }
        public string CompleteButtonText { get; set; }

    }

    public class BrewTargetTemperature
    {

        [Key]
        public int Id { get; set; }
        public float Target1 { get; set; }
        public float Target2 { get; set; }
    }
}

