using BrewMatic3000.Extensions;
using Microsoft.SPOT;

namespace BrewMatic3000.States
{
    public class StateOutputLog : State
    {

        public StateOutputLog(BrewData brewData)
            : base(brewData)
        {
        }

        public override void Start()
        {
            Debug.Print("===Current Brew Log=====================================================================");
            Debug.Print("===Warmup Started                  :0");
            Debug.Print("===Warmup Complete / Adding Grain  :" + BrewData.BrewAddGrainStart.Subtract(BrewData.BrewWarmupStart) + "    ");
            Debug.Print("===Starting Mash                   :" + BrewData.BrewMashStart.Subtract(BrewData.BrewWarmupStart) + "  (Took: " + BrewData.BrewMashStart.Subtract(BrewData.BrewAddGrainStart) + ") ");
            Debug.Print("===Mash Complete / Begin Mash Out  :" + BrewData.BrewMashOutStart.Subtract(BrewData.BrewWarmupStart) + "   (Took: " + BrewData.BrewMashOutStart.Subtract(BrewData.BrewMashStart) + ") ");
            Debug.Print("===Mash Out Complete / Begin sparge:" + BrewData.BrewSpargeStart.Subtract(BrewData.BrewWarmupStart) + "    (Took: " + BrewData.BrewSpargeStart.Subtract(BrewData.BrewMashOutStart) + ") ");
            Debug.Print("===Sparge Complete / Begin boil    :" + BrewData.BrewSpargeEnd.Subtract(BrewData.BrewWarmupStart) + "  (Took: " + BrewData.BrewSpargeEnd.Subtract(BrewData.BrewSpargeStart) + ") ");

            //Output all mash logValues to standard output
            var logValues = BrewData.MashPID.GetLogValues();
            if (logValues != null)
            {
                Debug.Print("===Log Values for Mash PID=====================================================================");
                Debug.Print("PID;Time;CurrentTemp;PreferredTemp;Effect");
                foreach (var logValue in logValues)
                {
                    Debug.Print("Mash;" + logValue.TimeStamp + ";" + logValue.CurrentTemperature + ";" + logValue.PreferredTemperature + ";" + logValue.Effect);
                }
                Debug.Print("===End: Log Values for Mash PID=====================================================================");
            }

            //Output all sparge logValues to standard output
            logValues = BrewData.SpargePID.GetLogValues();
            if (logValues != null)
            {
                Debug.Print("===Log Values for Sparge PID=====================================================================");
                Debug.Print("PID;Time;CurrentTemp;PreferredTemp;Effect");
                foreach (var logValue in logValues)
                {
                    Debug.Print("Sparge;" + logValue.TimeStamp + ";" + logValue.CurrentTemperature + ";" + logValue.PreferredTemperature + ";" + logValue.Effect);
                }
                Debug.Print("===End: Log Values for Sparge PID=====================================================================");
            }
            Debug.Print("===End: Current Brew Log=====================================================================");
            RiseStateChangedEvent(new State7Boil(BrewData));
        }

        private void WriteDefaultText()
        {
            WriteToLcd("Output log");
        }

        public override void OnKeyPressShort()
        {
            RiseStateChangedEvent(new StateDashboard(BrewData));
        }

        public override void OnKeyPressLongWarning()
        {
            WriteToLcd("..");
        }

        public override void OnKeyPressLongCancelled()
        {
            WriteDefaultText();
        }

        public override void OnKeyPressLong()
        {
            RiseStateChangedEvent(new State7Boil(BrewData));
        }

        public override string[] GetNewStateIndication(int secondsLeft)
        {
            return null;
        }
    }
}
