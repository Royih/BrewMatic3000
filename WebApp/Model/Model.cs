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
        public DbSet<BrewTargetTemperature> TargetTemp { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./blog.db");
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

    public class BrewTargetTemperature
    {

        [Key]
        public int Id { get; set; }
        public float Target1 { get; set; }
        public float Target2 { get; set; }
    }
}

