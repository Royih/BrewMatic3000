using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
{
    public class BrewMaticStatus
    {
        public float Temp1 { get; set; }
        public float Temp2 { get; set; }
        public float Heater1Percentage { get; set; }
        public float Heater2Percentage { get; set; }
    }

    public class BrewMaticNewSettings
    {
        public string[] ScreenContent { get; set; }
        public float TargetTemp1 { get; set; }
        public float TargetTemp2 { get; set; }
    }

    [Route("api/[controller]")]
    public class CommunicateController : Controller
    {

        private readonly ILogger<CommunicateController> _logger;

        public CommunicateController(ILogger<CommunicateController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public BrewMaticNewSettings Post([FromBody]BrewMaticStatus value)
        {
            _logger.LogDebug($"{DateTime.Now}: Temp1: {value.Temp1}. Temp2: {value.Temp2}. Heater1: {value.Heater1Percentage}%. Heater2: {value.Heater2Percentage}%");

            return new BrewMaticNewSettings
            {
                TargetTemp1 = 0,
                TargetTemp2 = 0,
                ScreenContent = new[]
                {
                    "Hello! " + DateTime.UtcNow.AddHours(2).ToString("HH:mm:ss"),
                    "",
                    GetLineString("1", value.Temp1, 0, value.Heater1Percentage),
                    GetLineString("2", value.Temp2, 0, value.Heater2Percentage)
                }
            };
        }

        private string GetLineString(string prefix, float currentTemp, float desiredTemp, float effectPercentage)
        {
            var currentTempString = currentTemp.ToString("f1").PadLeft(4);
            var desiredTempString = desiredTemp.ToString("f1").PadLeft(4);
            return prefix + ":" + currentTempString.PadLeft(5) + "/" + desiredTempString + " H:" + effectPercentage.DisplayHeat(); //Ms: 58.9/68.0 W:100%"
        }


    }
}
