using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Model;
using WebApp.Model.BrewGuide;
using WebApp.Model.BrewLogs;

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
                    BoilTimeInMinutes = t.BrewLog.BoilTimeInMinutes,
                    BatchSize = t.BrewLog.BatchSize,
                    MashWaterAmount = t.BrewLog.MashWaterAmount,
                    SpargeWaterAmount = t.BrewLog.SpargeWaterAmount
                },
                CurrentStep = new StepDto
                {
                    Id = t.Id,
                    Order = t.Order,
                    Name = t.Name,
                    StartTime = t.StartTime,
                    CompleteTime = t.CompleteTime,
                    TargetMashTemp = t.TargetMashTemp,
                    TargetSpargeTemp = t.TargetSpargeTemp,
                    CompleteButtonText = t.CompleteButtonText,
                    Instructions = t.Instructions,
                    ShowTimer = t.ShowTimer
                }
            };
        }

        public async Task<IEnumerable<BrewLogHistoryDto>> GetBrewHistory(int brewId)
        {
            var t = await _db.BrewLogSteps.Where(x => x.BrewId == brewId).OrderBy(x => x.Order).ToArrayAsync();
            var list = new List<BrewLogHistoryDto>();
            for (var i = 0; i < t.Length; i++)
            {
                var thisLog = t[i];
                BrewLogStep next = null;
                if (i + 1 < t.Length)
                    next = t[i + 1];

                list.Add(
                    new BrewLogHistoryDto
                    {
                        Name = thisLog.Name,
                        Started = thisLog.StartTime,
                        Completed = next?.StartTime
                    });
            }
            return list.AsEnumerable();
        }

        public async Task<IEnumerable<DataCaptureValueDto>> GetDefinedDataCaptureValues(int brewId)
        {
            var v1 = _db.DataCaptureFloatValues.Where(x => x.BrewLogStep.BrewId == brewId && x.Value.HasValue).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = x.BrewLogStepId,
                Label = x.Label,
                ValueAsString = x.Value.ToString(),
                ValueType = "float",
                Units = x.Units,
                Optional = x.Optional
            });
            var v2 = _db.DataCaptureIntValues.Where(x => x.BrewLogStep.BrewId == brewId && x.Value.HasValue).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = x.BrewLogStepId,
                Label = x.Label,
                ValueAsString = x.Value.ToString(),
                ValueType = "int",
                Units = x.Units,
                Optional = x.Optional
            });
            var v3 = _db.DataCaptureStringValues.Where(x => x.BrewLogStep.BrewId == brewId && x.Value != null && x.Value != "").Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = x.BrewLogStepId,
                Label = x.Label,
                ValueAsString = x.Value,
                ValueType = "string",
                Units = x.Units,
                Optional = x.Optional
            });
            var r = await v1.Union(v2).Union(v3).ToArrayAsync();
            return r.AsEnumerable();
        }



        public async Task<BrewLogStep> GetBrew(int brewId)
        {
            return await _db.BrewLogSteps.Include(x => x.BrewLog).OrderByDescending(x => x.Order).FirstOrDefaultAsync(x => x.BrewId == brewId);
        }

        private StepDto GetFirstStep(BrewLog brewLog)
        {
            return GetStepDto(_db.BrewStepTemplates.OrderBy(x => x.Id).First(), brewLog);
        }
        private StepDto GetNextStep(BrewLogStep brewLogStep)
        {
            return GetStepDto(_db.BrewStepTemplates.Where(x => x.Id > brewLogStep.Order).First(), brewLogStep.BrewLog);
        }

        private StepDto GetStepDto(BrewStepTemplate template, BrewLog brewLog)
        {
            return new StepDto
            {
                Order = template.Id,
                Name = template.Name,
                StartTime = DateTime.Now,
                CompleteButtonText = template.CompleteButtonText,
                Instructions = template.Instructions,
                CompleteTime = ResolveCompleteTime(brewLog, template.CompleteTimeAdd),
                TargetMashTemp = ResolveTemp(brewLog, template.Target1TempFrom),
                TargetSpargeTemp = ResolveTemp(brewLog, template.Target2TempFrom),
                ShowTimer = template.ShowTimer
            };

        }

        private DateTime? ResolveCompleteTime(BrewLog brewLog, string placeHolder)
        {
            if (placeHolder == "mashTimeInMinutes")
            {
                return DateTime.Now.AddMinutes(brewLog.MashTimeInMinutes);
            }
            if (placeHolder == "boilTimeInMinutes")
            {
                return DateTime.Now.AddMinutes(brewLog.BoilTimeInMinutes);
            }
            return null;
        }
        private float ResolveTemp(BrewLog brewLog, string placeHolder)
        {
            if (placeHolder == "strikeTemp")
            {
                return brewLog.StrikeTemp;
            }
            if (placeHolder == "spargeTemp")
            {
                return brewLog.SpargeTemp;
            }
            if (placeHolder == "mashTemp")
            {
                return brewLog.MashTemp;
            }
            if (placeHolder == "mashOutTemp")
            {
                return brewLog.MashOutTemp;
            }
            return 0;
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
                BoilTimeInMinutes = value.BoilTimeInMinutes,
                BatchSize = value.BatchSize,
                MashWaterAmount = value.MashWaterAmount,
                SpargeWaterAmount = value.SpargeWaterAmount
            };
            _db.Add(l);

            var firstStep = GetFirstStep(l);

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
                CompleteTime = step.CompleteTime,
                TargetMashTemp = step.TargetMashTemp,
                TargetSpargeTemp = step.TargetSpargeTemp,
                CompleteButtonText = step.CompleteButtonText,
                Instructions = step.Instructions,
                ShowTimer = step.ShowTimer
            };
            _db.Add(brewStep);

            ApplyStepTemperature(brewStep);

            //Add DataCapture values
            var dataCaptureValues = _db.DataCaptureDefinitions.Where(x => x.BrewStepTemplateId == step.Order);
            foreach (var v in dataCaptureValues)
            {
                if (v.ValueType == "float")
                {
                    _db.Add(new DataCaptureFloatValue
                    {
                        BrewLogStep = brewStep,
                        Label = v.Label,
                        Optional = v.Optional,
                        Units = v.Units
                    });
                }
                else if (v.ValueType == "int")
                {
                    _db.Add(new DataCaptureIntValue
                    {
                        BrewLogStep = brewStep,
                        Label = v.Label,
                        Optional = v.Optional,
                        Units = v.Units
                    });
                }
                else if (v.ValueType == "string")
                {
                    _db.Add(new DataCaptureStringValue
                    {
                        BrewLogStep = brewStep,
                        Label = v.Label,
                        Optional = v.Optional,
                        Units = v.Units
                    });
                }
            }

            return brewStep;
        }


        private void ApplyStepTemperature(BrewLogStep brewStep)
        {
            var targetTemp = GetTargetTemp();
            targetTemp.Target1 = brewStep.TargetMashTemp;
            targetTemp.Target2 = brewStep.TargetSpargeTemp;
        }

        public void GoToNextStep(int brewId)
        {
            var brewResult = GetBrew(brewId);
            Task.WhenAll(brewResult);
            var brewLogStep = brewResult.Result;

            var nextStep = GetNextStep(brewLogStep);

            if (nextStep != null)
            {
                AddStep(brewLogStep.BrewLog, nextStep);
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

        public async Task<IEnumerable<DataCaptureValueDto>> GetDataCaptureValues(int brewStepId)
        {
            var floatValues = _db.DataCaptureFloatValues.Where(x => x.BrewLogStepId == brewStepId).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = brewStepId,
                Label = x.Label,
                ValueAsString = x.Value.ToString(),
                ValueType = "float",
                Units = x.Units,
                Optional = x.Optional
            });
            var intValues = _db.DataCaptureIntValues.Where(x => x.BrewLogStepId == brewStepId).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = brewStepId,
                Label = x.Label,
                ValueAsString = x.Value.ToString(),
                ValueType = "int",
                Units = x.Units,
                Optional = x.Optional
            });
            var stringValues = _db.DataCaptureStringValues.Where(x => x.BrewLogStepId == brewStepId).Select(x => new DataCaptureValueDto
            {
                Id = x.Id,
                BrewStepId = brewStepId,
                Label = x.Label,
                ValueAsString = x.Value,
                ValueType = "string",
                Units = x.Units,
                Optional = x.Optional
            });
            return await floatValues.Union(intValues).Union(stringValues).OrderBy(x => x.Label).ToListAsync();
        }

        public void SaveDataCaptureValues(DataCaptureValueDto[] values)
        {
            foreach (var value in values)
            {
                if (value.ValueType == "float")
                {
                    var thisValue = _db.DataCaptureFloatValues.Single(x => x.Id == value.Id);
                    if (!string.IsNullOrEmpty(value.ValueAsString))
                    {
                        float floatValue;
                        if (float.TryParse(value.ValueAsString, out floatValue))
                        {
                            thisValue.Value = floatValue;
                        }
                    }
                    else
                    {
                        thisValue.Value = null;
                    }
                }
                if (value.ValueType == "int")
                {
                    var thisValue = _db.DataCaptureIntValues.Single(x => x.Id == value.Id);
                    if (!string.IsNullOrEmpty(value.ValueAsString))
                    {
                        int intValue;
                        if (int.TryParse(value.ValueAsString, out intValue))
                        {
                            thisValue.Value = intValue;
                        }
                    }
                    else
                    {
                        thisValue.Value = null;
                    }
                }
                if (value.ValueType == "string")
                {
                    var thisValue = _db.DataCaptureStringValues.Single(x => x.Id == value.Id);
                    if (!string.IsNullOrEmpty(value.ValueAsString))
                    {
                        thisValue.Value = value.ValueAsString;
                    }
                    else
                    {
                        thisValue.Value = null;
                    }
                }
            }
        }
        public IEnumerable<BrewLogsDto> ListBrewLogs()
        {
            var lastSteps = _db.BrewLogSteps.GroupBy(x => x.BrewId).Select(x => x.OrderByDescending(y => y.Id).First());
            return lastSteps.Select(x => new BrewLogsDto
            {
                Id = x.BrewId, 
                Name = x.BrewLog.Name, 
                BatchSize = x.BrewLog.BatchSize, 
                Initiated = x.BrewLog.TimeStamp, 
                BrewDate = x.BrewLog.TimeStamp, /*Resolve actual brewdate somehow*/
                CurrentStep = x.Name
            });

        }
    }
}