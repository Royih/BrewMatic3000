using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Model;
using WebApp.Model.BrewGuide;

namespace WebApp.BusinessLogic
{
    public class BrewLogRepository
    {
        private BrewMaticContext _db;

        public BrewLogRepository(BrewMaticContext db)
        {
            _db = db;
        }


        public BrewTargetTemperature GetTargetTemp()
        {
            var t = _db.TargetTemp.FirstOrDefault();
            if (t != null)
            {
                return t;
            }
            t = new BrewTargetTemperature();
            t.Target1 = 20;
            t.Target2 = 20;
            _db.TargetTemp.Add(t);
            return t;

        }

        public async Task<CurrentBrewDto> GetBrewDto(int brewId)
        {
            var t = await GetBrew(brewId);

            return new CurrentBrewDto
            {
                Setup = new SetupDto
                {
                    Name = t.BrewLog.Name,
                    MashTemp = t.BrewLog.MashTemp,
                    StrikeTemp = t.BrewLog.StrikeTemp,
                    SpargeTemp = t.BrewLog.SpargeTemp,
                    MashOutTemp = t.BrewLog.MashOutTemp,
                    MashTimeInMinutes = t.BrewLog.MashTimeInMinutes,
                    BoilTimeInMinutes = t.BrewLog.BoilTimeInMinutes
                },
                CurrentStep = new StepDto
                {
                    Order = t.Order,
                    Name = t.Name,
                    StartTime = t.StartTime,
                    ShowTimer = t.ShowTimer,
                    GetTargetMashTemp = t.GetTargetMashTemp,
                    GetTargetSpargeTemp = t.GetTargetSpargeTemp,
                    CompleteButtonText = t.CompleteButtonText
                }
            };
        }

        public async Task<BrewLogStep> GetBrew(int brewId)
        {
            return await _db.BrewLogSteps.Include(x => x.BrewLog).OrderByDescending(x => x.Order).FirstOrDefaultAsync(x => x.BrewId == brewId);
        }

        public BrewLogStep InitializeNewBrew(SetupDto value)
        {
            var l = new BrewLog
            {
                TimeStamp = DateTime.Now,
                Name = value.Name,
                MashTemp = value.MashTemp,
                StrikeTemp = value.StrikeTemp,
                SpargeTemp = value.SpargeTemp,
                MashOutTemp = value.MashOutTemp,
                MashTimeInMinutes = value.MashTimeInMinutes,
                BoilTimeInMinutes = value.BoilTimeInMinutes
            };
            _db.Add(l);

            var firstStep = Steps.GetAllSteps(value.StrikeTemp, value.SpargeTemp, value.MashTemp, value.MashOutTemp).OrderBy(x => x.Order).FirstOrDefault();

            var brewStep = AddStep(l, firstStep);

            return brewStep;
        }

        private BrewLogStep AddStep(BrewLog log, StepDto step)
        {
            var brewStep = new BrewLogStep
            {
                BrewLog = log,
                Order = step.Order,
                Name = step.Name,
                StartTime = DateTime.Now,
                ShowTimer = step.ShowTimer,
                GetTargetMashTemp = step.GetTargetMashTemp,
                GetTargetSpargeTemp = step.GetTargetSpargeTemp,
                CompleteButtonText = step.CompleteButtonText
            };
            _db.Add(brewStep);

            ApplyStepTemperature(brewStep);

            return brewStep;
        }

        private void ApplyStepTemperature(BrewLogStep brewStep)
        {
            var targetTemp = GetTargetTemp();
            targetTemp.Target1 = brewStep.GetTargetMashTemp;
            targetTemp.Target2 = brewStep.GetTargetSpargeTemp;
        }

        public void GoToNextStep(int brewId)
        {
            var brewResult = GetBrew(brewId);
            Task.WhenAll(brewResult);
            var brew = brewResult.Result;
            var nextStep = Steps.GetAllSteps(brew.BrewLog.StrikeTemp, brew.BrewLog.SpargeTemp, brew.BrewLog.MashTemp, brew.BrewLog.MashOutTemp).OrderBy(x => x.Order).Where(x => x.Order > brew.Order).FirstOrDefault();
            if (nextStep != null)
            {
                AddStep(brew.BrewLog, nextStep);
            }
        }
        public void GoBackOneStep(int brewId)
        {
            var logs = _db.BrewLogSteps.Where(x => x.BrewId == brewId).OrderByDescending(x => x.Order);
            if (logs.Count() > 1) //we dont want to delete the last row
            {
                _db.Remove(logs.First());
            }
            ApplyStepTemperature(logs.ToList()[1]);
        }

    }

}