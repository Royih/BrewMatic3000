

namespace WebApp
{
    public static class Extensions
    {
        public static string PadLeft(this string input, int totalLength)
        {
            var returnValue = input;
            while (returnValue.Length < totalLength)
            {
                returnValue = " " + returnValue;
            }
            return returnValue;
        }
        public static string PadRight(this string input, int totalLength)
        {
            var returnValue = input;
            while (returnValue.Length < totalLength)
            {
                returnValue = returnValue + " ";
            }
            return returnValue;
        }
        public static string DisplayHeat(this float input)
        {
            return ((int)input).ToString().PadLeft(3) + "%";
        }

        public static string DisplayTemperature(this float input)
        {
            return input.ToString("f1").PadLeft(5) + (char)223;
        }
    }
}
