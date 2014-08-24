
namespace BrewMatic3000.Extensions
{
    public static class FloatExtension
    {
        public static string PadLeft(this float input, int length)
        {
            var returnValue = input.ToString();
            while (returnValue.Length < length)
            {
                returnValue = " " + returnValue;
            }
            return returnValue;
        }
        public static string DisplayHeat(this float input)
        {
            return input.PadLeft(3) + "%";
        }

        public static string DisplayTemperature(this float input)
        {
            return input.ToString("f1").PadLeft(5) + (char)223;
        }
    }
}
