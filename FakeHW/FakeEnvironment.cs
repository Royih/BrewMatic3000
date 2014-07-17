using System;
using BrewMatic3000.Interfaces;

namespace BrewMatic3000.FakeHW
{
    /// <summary>
    /// Will simulate reality by altering the temparature based on the heating applid over time
    /// </summary>
    public class FakeEnvironment
    {
        private readonly ITempReader _tempReader;

        private const float WaterDencity = 0.994f;

        private const float WaterInTank = 30;

        private const int RoomTemparature = 20;

        private DateTime _dtMeasureStart;

        private const float CoolingEffect = 750;



        public FakeEnvironment(ITempReader tempReader)
        {
            _tempReader = tempReader;
            _dtMeasureStart = DateTime.Now;
        }

        public void UpdateValues(float watts)
        {
            var timeSpan = DateTime.Now.Subtract(_dtMeasureStart);
            float milliseconds = (timeSpan.Seconds * 1000) + timeSpan.Milliseconds;

            var tempDiff = GetDiffValueByEffectAndTime(watts, milliseconds);

            var coolingDiff = GetDiffValueByEffectAndTime(CoolingEffect, milliseconds);

            var currentValue = _tempReader.GetValue();

            if (currentValue <= RoomTemparature)
                coolingDiff = 0;

            _tempReader.SetValue(currentValue + tempDiff - coolingDiff);

            _dtMeasureStart = DateTime.Now;
        }

        private float GetDiffValueByEffectAndTime(float watt, float milliseconds)
        {
            var kiloJoules = (((watt * milliseconds) / 1000)) / 1000;

            const float massOfWater = WaterInTank * WaterDencity;

            var tempDiff = (kiloJoules / (GetHeatCapasity(_tempReader.GetValue()) * massOfWater));

            return tempDiff;
        }

        private float GetHeatCapasity(float temparature)
        {
            if (temparature < 15)
                return 4.210f;
            if (temparature < 25)
                return 4.184f;
            if (temparature < 35)
                return 4.178f;
            if (temparature < 45)
                return 4.181f;
            if (temparature < 55)
                return 4.183f;
            if (temparature < 65)
                return 4.188f;
            if (temparature < 75)
                return 4.194f;
            if (temparature < 85)
                return 4.283f;
            return 4.219f;
        }

    }
}
