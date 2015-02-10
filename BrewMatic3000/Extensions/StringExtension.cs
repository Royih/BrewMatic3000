
namespace BrewMatic3000.Extensions
{
    public static class StringtExtension
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
    }
}
