using System;
using System.Globalization;

namespace Resturant.Core.Utilities
{
    public class DateConvert
    {
        public static DateTime GetCurrentHjriDate()
        {
            return ConvertToHijriData(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.Month),
                Convert.ToInt16(DateTime.Now.Day), Convert.ToInt16(DateTime.Now.Hour), DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }
        public static string GetCurrentHjriDateStr()
        {
            return ConvertToHijriDataString(Convert.ToInt16(DateTime.Now.Year), Convert.ToInt16(DateTime.Now.Month),
                Convert.ToInt16(DateTime.Now.Day), Convert.ToInt16(DateTime.Now.Hour), DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }

        public static DateTime ConvertToHijriData(int yr, int mn, int dy, int hour = 0, int Minute = 0, int second = 0, int millisecond = 0)
        {
            var calendar = new UmAlQuraCalendar();
            var gregDateTime = new DateTime(yr, mn, dy, hour, Minute, second);
            var MyString = calendar.GetYear(gregDateTime) + "-" + calendar.GetMonth(gregDateTime) + "-" + calendar.GetDayOfMonth(gregDateTime) + " " + hour + ":" + Minute + ":" + second;
            var MyDateTime = DateTime.ParseExact(MyString, "yyyy-M-d H:m:s",
                                    null);
            return MyDateTime;
        }

        public static string ConvertToHijriDataString(int yr, int mn, int dy, int hour = 0, int Minute = 0, int second = 0, int millisecond = 0)
        {
            var calendar = new UmAlQuraCalendar();
            var gregDateTime = new DateTime(yr, mn, dy, hour, Minute, second);
            var MyDateTime = calendar.GetYear(gregDateTime) + "-" + calendar.GetMonth(gregDateTime) + "-" + calendar.GetDayOfMonth(gregDateTime) + " " + hour + ":" + Minute + ":" + second;
            return MyDateTime;
        }

        public static string ConvertToHijriDateTimetoString(DateTime hijri)
        {
            var MyDateTime = hijri.Year + "-" + hijri.Month + "-" + hijri.Day + " " + hijri.Hour + ":" + hijri.Minute + ":" + hijri.Second;
            return MyDateTime;
        }

        public static string ConvertToHijriDatetoString(DateTime hijri)
        {
            var MyDateTime = hijri.Year + "-" + hijri.Month + "-" + hijri.Day;
            return MyDateTime;
        }
    }
}
