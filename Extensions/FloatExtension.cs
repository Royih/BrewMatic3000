
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
    }
}
