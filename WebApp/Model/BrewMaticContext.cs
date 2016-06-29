using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace WebApp.Model
{
    public class BrewMaticContext : DbContext
    {
        public DbSet<BrewTempLog> TempLogs { get; set; }
        public DbSet<BrewLog> BrewLogs { get; set; }
        public DbSet<BrewLogStep> BrewLogSteps { get; set; }
        public DbSet<BrewTargetTemperature> TargetTemp { get; set; }
        public DbSet<BrewStepTemplate> BrewStepTemplates { get; set; }
        public DbSet<DataCaptureDefinition> DataCaptureDefinitions { get; set; }
        public DbSet<DataCaptureFloatValue> DataCaptureFloatValues { get; set; }
        public DbSet<DataCaptureStringValue> DataCaptureStringValues { get; set; }
        public DbSet<DataCaptureIntValue> DataCaptureIntValues { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./brew.db");
        }
        public static void Seed()
        {
            using (var db = new BrewMaticContext())
            {
                db.Database.EnsureCreated();
                if (db.TempLogs.Count() == 0)
                {
                    var newLog = new BrewTempLog
                    {
                        Temp1 = 0,
                        Temp2 = 0,
                        Heater1Percentage = 0,
                        Heater2Percentage = 0,
                        TimeStamp = DateTime.Now
                    };
                    db.TempLogs.Add(newLog);

                    var initialStep = new BrewStepTemplate
                    {
                        Name = "Initial",
                        CompleteButtonText = "Start Warmup",
                        Instructions = "Get ready for brewing"
                    };
                    db.Add(initialStep);
                    db.Add(new BrewStepTemplate
                    {
                        Name = "Warmup",
                        CompleteButtonText = "Start adding grain",
                        Instructions = "Wait and relax",
                        Target1TempFrom = "strikeTemp",
                        Target2TempFrom = "spargeTemp"
                    });
                    db.Add(new BrewStepTemplate
                    {
                        Name = "Add grain",
                        CompleteButtonText = "Start Mash-timer",
                        Instructions = "Add grain to water in the mash kettle",
                        Target1TempFrom = "mashTemp",
                        Target2TempFrom = "spargeTemp"
                    });
                    db.Add(new BrewStepTemplate
                    {
                        Name = "Mash",
                        CompleteButtonText = "Start mash-out",
                        Instructions = "Wait for the timer to reach zero. Stir the mash a few times. Pay attention to the temperature.",
                        CompleteTimeAdd = "mashTimeInMinutes",
                        Target1TempFrom = "mashTemp",
                        Target2TempFrom = "spargeTemp"
                    });
                    db.Add(new BrewStepTemplate
                    {
                        Name = "Mash out",
                        CompleteButtonText = "Start sparge",
                        Instructions = "Wait for the temperature to reach the critical 75.6�C. ",
                        Target1TempFrom = "mashOutTemp",
                        Target2TempFrom = "spargeTemp"
                    });
                    db.Add(new BrewStepTemplate
                    {
                        Name = "Sparge",
                        CompleteButtonText = "Sparge complete",
                        Instructions = "Add water to the top of the mash kettle.  Transfer wort from the bottom of the mash kettle to the boil kettle."
                    });
                    var boilWarmupStep = new BrewStepTemplate
                    {
                        Name = "Boil warmup",
                        CompleteButtonText = "Start Boil-timer",
                        Instructions = "Wait for the wort to boil. Sample OG (before boil). Note the volume of wort before boil. Take the Yiest out of the fridge now."
                    };
                    db.Add(boilWarmupStep);
                    db.Add(new BrewStepTemplate
                    {
                        Name = "Boil",
                        CompleteButtonText = "Start Cool-down",
                        Instructions = "Let the wort boil until timer reaches zero. Add hops according to the hop bill. Add yiest nutrition. Add Whirl-flock (15 minutes before end). ",
                        CompleteTimeAdd = "boilTimeInMinutes"
                    });
                    var cooldownStep = new BrewStepTemplate
                    {
                        Name = "Cooldown",
                        CompleteButtonText = "Brew complete",
                        Instructions = "Cool the wort to 18-20�C. Use whirlpool to gather remains of hop and grain. Clean the yiest tank now. "
                    };
                    db.Add(cooldownStep);
                    var completeStep = new BrewStepTemplate
                    {
                        Name = "Complete",
                        Instructions = "Transfer to yiest tank(bucket). Sample the OG. Note the volume of wort. Add o2. Pitch yiest. Be happy. "
                    };
                    db.Add(completeStep);

                    db.Add(new DataCaptureDefinition
                    {
                        BrewStepTemplate = initialStep,
                        Label = "Water in Mash kettle",
                        ValueType = "float",
                        Optional = false,
                        Units = "l"
                    });
                    db.Add(new DataCaptureDefinition
                    {
                        BrewStepTemplate = initialStep,
                        Label = "Water in Sparge kettle",
                        ValueType = "float",
                        Optional = false,
                        Units = "l"
                    });
                    db.Add(new DataCaptureDefinition
                    {
                        BrewStepTemplate = boilWarmupStep,
                        Label = "OG before boil",
                        ValueType = "int",
                        Optional = false,
                        Units = "SG"
                    });
                    db.Add(new DataCaptureDefinition
                    {
                        BrewStepTemplate = cooldownStep,
                        Label = "Wort before boil",
                        ValueType = "float",
                        Optional = false,
                        Units = "l"
                    });
                    db.Add(new DataCaptureDefinition
                    {
                        BrewStepTemplate = cooldownStep,
                        Label = "OG after boil",
                        ValueType = "int",
                        Optional = false,
                        Units = "SG"
                    });
                    db.Add(new DataCaptureDefinition
                    {
                        BrewStepTemplate = completeStep,
                        Label = "Wort after boil",
                        ValueType = "float",
                        Optional = false,
                        Units = "l"
                    });
                    db.Add(new DataCaptureDefinition
                    {
                        BrewStepTemplate = completeStep,
                        Label = "FG",
                        ValueType = "int",
                        Optional = false,
                        Units = "SG"
                    });

                }
                //db.Database.Migrate();

                // Seed code
                db.SaveChanges();
            }
        }

    }

    public class BrewTempLog
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
        public DateTime? CompleteTime { get; set; }
        public float TargetMashTemp { get; set; }
        public float TargetSpargeTemp { get; set; }
        public string CompleteButtonText { get; set; }
        public string Instructions { get; set; }
    }

    public class BrewStepTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompleteButtonText { get; set; }
        public string Instructions { get; set; }
        public string Target1TempFrom { get; set; }
        public string Target2TempFrom { get; set; }
        public string CompleteTimeAdd { get; set; }
    }

    public class BrewTargetTemperature
    {

        [Key]
        public int Id { get; set; }
        public float Target1 { get; set; }
        public float Target2 { get; set; }
    }

    public class DataCaptureDefinition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BrewStepTemplate")]
        public int BrewStepTemplateId { get; set; }
        public virtual BrewStepTemplate BrewStepTemplate { get; set; }
        public string Label { get; set; }
        public string ValueType { get; set; }
        public bool Optional { get; set; }
        public string Units {get; set;}
    }

    public class DataCaptureFloatValue
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BrewLogStep")]
        public int BrewLogStepId { get; set; }
        public virtual BrewLogStep BrewLogStep { get; set; }
        public string Label { get; set; }
        public bool Optional { get; set; }
        public float? Value { get; set; }
        public string Units {get; set;}
    }

    public class DataCaptureStringValue
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BrewLogStep")]
        public int BrewLogStepId { get; set; }
        public virtual BrewLogStep BrewLogStep { get; set; }
        public string Label { get; set; }
        public bool Optional { get; set; }
        public string Value { get; set; }
        public string Units {get; set;}
    }

    public class DataCaptureIntValue
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BrewLogStep")]
        public int BrewLogStepId { get; set; }
        public virtual BrewLogStep BrewLogStep { get; set; }
        public string Label { get; set; }
        public bool Optional { get; set; }
        public int? Value { get; set; }
        public string Units {get; set;}
    }

}

