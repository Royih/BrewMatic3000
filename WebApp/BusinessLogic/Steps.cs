
using System;
using System.Collections.Generic;
using WebApp.Model.BrewGuide;

namespace WebApp.BusinessLogic
{
    /*public static class Steps2
    {
        public static IEnumerable<StepDto> GetAllSteps(float strikeTemp, float spargeTemp, float mashTemp, float mashOutTemp, int? mashTimeInMinutes, int? boilTimeInMinutes)
        {
            yield return new StepDto()
            {
                Order = 1,
                Name = "Initial",
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Start Warmup", 
                Instructions = "Get ready for brewing"
            };
            yield return new StepDto()
            {
                Order = 2,
                Name = "Warmup",
                GetTargetMashTemp = strikeTemp,
                GetTargetSpargeTemp = spargeTemp,
                CompleteButtonText = "Start adding grain",
                Instructions = "Wait and relax"
            };
            yield return new StepDto()
            {
                Order = 3,
                Name = "Add grain",
                GetTargetMashTemp = mashTemp,
                GetTargetSpargeTemp = spargeTemp,
                CompleteButtonText = "Start Mash-timer",
                Instructions = "Add grain to water in the mash kettle."
            };
            yield return new StepDto()
            {
                Order = 4,
                Name = "Mash",
                CompleteTime = DateTime.Now.AddMinutes(mashTimeInMinutes ?? 0),
                GetTargetMashTemp = mashTemp,
                GetTargetSpargeTemp = spargeTemp,
                CompleteButtonText = "Start mash-out",
                Instructions = "Wait for the timer to reach zero. Stir the mash a few times. Pay attention to the temperature. "
            };
            yield return new StepDto()
            {
                Order = 5,
                Name = "Mash out",
                GetTargetMashTemp = mashOutTemp,
                GetTargetSpargeTemp = spargeTemp,
                CompleteButtonText = "Start sparge",
                Instructions = "Wait for the temperature to reach the critical 75.6�C. "
            };
            yield return new StepDto()
            {
                Order = 6,
                Name = "Sparge",
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Sparge complete",
                Instructions = "Add water to the top of the mash kettle.  Transfer wort from the bottom of the mash kettle to the boil kettle. "
            };
            yield return new StepDto()
            {
                Order = 7,
                Name = "Boil warmup",
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Start Boil-timer",
                Instructions = "Wait for the wort to boil. Sample OG (before boil). Note the volume of wort before boil. Take the Yiest out of the fridge now. "
            };
            yield return new StepDto()
            {
                Order = 8,
                Name = "Boil",
                CompleteTime = DateTime.Now.AddMinutes(boilTimeInMinutes??0),
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Start Cool-down",
                Instructions = "Let the wort boil until timer reaches zero. Add hops according to the hop bill. Add yiest nutrition. Add Whirl-flock (15 minutes before end). "
            };
            yield return new StepDto()
            {
                Order = 9,
                Name = "Cooldown",
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Brew complete",
                Instructions = "Cool the wort to 18-20�C. Use whirlpool to gather remains of hop and grain. Clean the yiest tank now. "
            };
            yield return new StepDto()
            {
                Order = 10,
                Name = "Complete",
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                Instructions = "Transfer to yiest tank(bucket). Sample the OG. Note the volume of wort. Add o2. Pitch yiest. Be happy. "
            };
        }
    }*/
}