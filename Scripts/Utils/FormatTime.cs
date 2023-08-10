using System;
using System.Globalization;

namespace Utils
{
    public static class FormatTime
    {
        public static string HoursStringFormat(int totalHours)
        {
            int hours = totalHours / 3600;
            int minutes = totalHours % 3600 / 60;
            int seсonds = totalHours % 60;
            return $"{hours:00}:{minutes:00}:{seсonds:00}";
        }
        
        public static string MinutesStringFormat(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            return $"{minutes:00}m:{seconds:00}s";
        }
        
        public static int HoursIntFormat(int totalHours)
        {
            return totalHours * 3600;
        }
        
        public static int MinutesIntFormat(int totalMinutes)
        {
            return totalMinutes * 60;
        }

        public static DateTime StrToDateTime(string strTime)
        {
            DateTime aaa = Convert.ToDateTime(strTime);
            return aaa;
        }
        
        public static string DateTimeToStr(DateTime time)
        {
            return time.ToString(CultureInfo.CurrentCulture);
        }
    }
}