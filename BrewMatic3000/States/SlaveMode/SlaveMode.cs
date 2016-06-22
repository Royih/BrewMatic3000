using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using BrewMatic3000.Extensions;
using Json.NETMF;

namespace BrewMatic3000.States.SlaveMode
{
    public class SlaveMode : State
    {
        private int _ok;
        private int _error;

        private const string _url = "http://brewmaticwebapp.azurewebsites.net/api/communicate";

        private string[] _screen = new[] { "", "", "", "" };


        public SlaveMode(BrewData brewData, string[] initialMessage = null, int initialScreen = 0)
            : base(brewData, initialMessage, initialScreen)
        {

        }

        public enum Screens
        {
            Default,
            Return
        }

        public override int GetNumberOfScreens()
        {
            return (int)Screens.Return;
        }


        public override Screen GetScreen(int screenNumber)
        {
            switch (screenNumber)
            {
                case (int)Screens.Default:
                    {
                        return new Screen(screenNumber, _screen);
                    }
                case (int)Screens.Return:
                    {
                        var strLine1 = "=  Slave mode  =";
                        var strLine2 = "";
                        var strLine3 = "Return";
                        var strLine4 = "";

                        return new Screen(screenNumber, new[] { strLine1, strLine2, strLine3, strLine4 });
                    }
                default:
                    {
                        return GetScreenError(screenNumber);
                    }
            }
        }

        protected override void DoWorkExtra()
        {
            try
            {
                var json = "{\"Temp1\":\"" + BrewData.TempReader1.GetValue() + "\"," +
                            "\"Temp2\":\"" + BrewData.TempReader2.GetValue() + "\"," +
                            "\"Heater1Percentage\":\"" + BrewData.Heater1.GetCurrentValue() + "\"," +
                            "\"Heater2Percentage\":\"" + BrewData.Heater2.GetCurrentValue() + "\"}";

                var request = (HttpWebRequest)WebRequest.Create(_url);
                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.Method = "POST";
                request.ContentLength = byteArray.Length;
                request.ContentType = "application/json";
                request.Timeout = 60000;
                request.ReadWriteTimeout = 60000;
                //request.KeepAlive = false;

                Stream postStream = request.GetRequestStream();

                postStream.Write(byteArray, 0, byteArray.Length);
                postStream.Close();

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseValue = string.Empty;

                    // Error
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new ApplicationException("Received code: " + response.StatusCode);
                    }

                    // Grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                                var hashTable = JsonSerializer.DeserializeString(responseValue) as Hashtable;
                                if (hashTable != null)
                                {
                                    var screenContent = hashTable["screenContent"] as ArrayList;
                                    _screen = ParseScreenContent(screenContent);
                                    var targetTemp1 = (double)hashTable["targetTemp1"];
                                    var targetTemp2 = (double)hashTable["targetTemp2"];
                                    ConsiderNewTemp(BrewData.MashPID, targetTemp1);
                                    ConsiderNewTemp(BrewData.SpargePID, targetTemp2);
                                }
                            }
                        }
                    }


                }

                _ok++;

            }
            catch (Exception ex)
            {
                _error++;
            }
        }

        private void ConsiderNewTemp(PID.PID pid, double newTargetTemp)
        {
            var diff = Math.Abs(BrewData.MashPID.GetPreferredTemperature - newTargetTemp);
            if (diff > 0.1)
            {
                if (pid.Started())
                {
                    pid.Stop();
                }
                pid.Start((float)newTargetTemp);
            }
        }


        public override void KeyPressNextLong()
        {
            if (GetCurrentScreenNumber == (int)Screens.Default)
            {
                _ok = 0;
                _error = 0;
            }
            if (GetCurrentScreenNumber == (int)Screens.Return)
            {
                RiseStateChangedEvent(new StateDashboard(BrewData));
            }
        }

        private string[] ParseScreenContent(ArrayList screenContent)
        {
            if (screenContent != null && screenContent.Count >= 4)
            {
                return new[] { (screenContent[0] as string) ?? "", (screenContent[1] as string) ?? "", (screenContent[2] as string) ?? "", (screenContent[3] as string) ?? "" };
            }
            return new[] { "Error", "Has", "Occurred", "" };
        }


        protected override void StartExtra()
        {
            BrewData.Heater1.SetValue(0);
            BrewData.Heater2.SetValue(0);
        }

    }
}
