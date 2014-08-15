
namespace BrewMatic3000.Extensions
{
    public static class StringtExtension
    {
        public static string PadLeft(this string input, int length)
        {
            var returnValue = input;
            while (returnValue.Length < length)
            {
                returnValue = " " + returnValue;
            }
            return returnValue;
        }
        public static string PadRight(this string input, int length)
        {
            var returnValue = input;
            while (returnValue.Length < length)
            {
                returnValue = returnValue + " ";
            }
            return returnValue;
        }
    }
}
