using System;

namespace BrewMatic3000.Extensions
{
    public static class DateTimeExtension
    {
        public static string Display(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy MMM dd HH:mm:ss");
        }

        public static string DisplayShort(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM HH:mm");
        }

           
    }
}
