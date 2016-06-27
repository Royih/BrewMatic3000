
using System.Collections.Generic;
using WebApp.Model.BrewGuide;

namespace WebApp.BusinessLogic
{
    public static class Steps
    {
        public static IEnumerable<StepDto> GetAllSteps(float strikeTemp, float spargeTemp, float mashTemp, float mashOutTemp)
        {
            yield return new StepDto()
            {
                Order = 1,
                Name = "Initial",
                ShowTimer = false,
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Start Warmup"
            };
            yield return new StepDto()
            {
                Order = 2,
                Name = "Warmup",
                ShowTimer = true,
                GetTargetMashTemp = strikeTemp,
                GetTargetSpargeTemp = spargeTemp,
                CompleteButtonText = "Start adding grain"
            };
            yield return new StepDto()
            {
                Order = 3,
                Name = "Add grain",
                ShowTimer = true,
                GetTargetMashTemp = mashTemp,
                GetTargetSpargeTemp = spargeTemp,
                CompleteButtonText = "Start Mash-timer"
            };
            yield return new StepDto()
            {
                Order = 4,
                Name = "Mash",
                ShowTimer = true,
                GetTargetMashTemp = mashTemp,
                GetTargetSpargeTemp = spargeTemp,
                CompleteButtonText = "Start mash-out"
            };
            yield return new StepDto()
            {
                Order = 5,
                Name = "Mash out",
                ShowTimer = true,
                GetTargetMashTemp = mashOutTemp,
                GetTargetSpargeTemp = spargeTemp,
                CompleteButtonText = "Start sparge"
            };
            yield return new StepDto()
            {
                Order = 6,
                Name = "Sparge",
                ShowTimer = true,
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Sparge complete"
            };
            yield return new StepDto()
            {
                Order = 7,
                Name = "Boil warmup",
                ShowTimer = true,
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Start Boil-timer"
            };
            yield return new StepDto()
            {
                Order = 8,
                Name = "Boil",
                ShowTimer = true,
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Start Cool-down"
            };
            yield return new StepDto()
            {
                Order = 9,
                Name = "Cooldown",
                ShowTimer = true,
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0,
                CompleteButtonText = "Brew complete"
            };
            yield return new StepDto()
            {
                Order = 10,
                Name = "Complete",
                ShowTimer = false,
                GetTargetMashTemp = 0,
                GetTargetSpargeTemp = 0
            };
        }
    }
}