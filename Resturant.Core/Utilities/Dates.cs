using System;
using System.Globalization;


namespace Resturant.Core.Utilities
{
    public class Dates
    {
        private const int startGreg = 1900;
        private const int endGreg = 2100;
        private readonly string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
            "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy"};
        private readonly CultureInfo arCul;
        private readonly CultureInfo enCul;
        private readonly UmAlQuraCalendar h;
        private readonly GregorianCalendar g;

        public Dates()
        {
            arCul = new CultureInfo("ar-SA");
            enCul = new CultureInfo("en-US");

            h = new UmAlQuraCalendar();
            g = new GregorianCalendar(GregorianCalendarTypes.USEnglish);

            arCul.DateTimeFormat.Calendar = h;
        }

        /// <summary>
        /// Check if string is hijri date and then return true 
        /// </summary>
        /// <PARAM name="hijri"></PARAM>
        /// <returns></returns>
        public bool IsHijri(string hijri)
        {
            if (hijri.Length <= 0)
            {
                //cur.Trace.Warn("IsHijri Error: Date String is Empty");
                return false;
            }
            try
            {
                var tempDate = DateTime.ParseExact(hijri, allFormats,
                     arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch
            {
                //cur.Trace.Warn("IsHijri Error :" + hijri.ToString() + "\n" +ex.Message);
                return false;
            }

        }
        /// <summary>
        /// Check if string is Gregorian date and then return true 
        /// </summary>
        /// <PARAM name="greg"></PARAM>
        /// <returns></returns>
        public bool IsGreg(string greg)
        {
            if (greg.Length <= 0)
            {

                //cur.Trace.Warn("IsGreg :Date String is Empty");
                return false;
            }
            try
            {
                var tempDate = DateTime.ParseExact(greg, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch
            {
                //cur.Trace.Warn("IsGreg Error :" + greg.ToString() + "\n" + ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Return Formatted Hijri date string 
        /// </summary>
        /// <PARAM name="date"></PARAM>
        /// <PARAM name="format"></PARAM>
        /// <returns></returns>
        public string FormatHijri(string date, string format)
        {
            if (date.Length <= 0)
            {
                //cur.Trace.Warn("Format :Date String is Empty");
                return "";
            }
            try
            {
                var tempDate = DateTime.ParseExact(date,
                   allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(format, arCul.DateTimeFormat);

            }
            catch
            {
                //cur.Trace.Warn("Date :\n" + ex.Message);
                return "";
            }

        }
        /// <summary>
        /// Returned Formatted Gregorian date string
        /// </summary>
        /// <PARAM name="date"></PARAM>
        /// <PARAM name="format"></PARAM>
        /// <returns></returns>
        public string FormatGreg(string date, string format)
        {
            if (date.Length <= 0)
            {

                //cur.Trace.Warn("Format :Date String is Empty");
                return "";
            }
            try
            {
                var tempDate = DateTime.ParseExact(date, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(format, enCul.DateTimeFormat);

            }
            catch
            {
                //cur.Trace.Warn("Date :\n" + ex.Message);
                return "";
            }

        }
        /// <summary>
        /// Return Today Gregorian date and return it in yyyy/MM/dd format
        /// </summary>
        /// <returns></returns>
        public string GDateNow()
        {
            try
            {
                return DateTime.Now.ToString("yyyy/MM/dd", enCul.DateTimeFormat);
            }
            catch
            {
                //cur.Trace.Warn("GDateNow :\n" + ex.Message);
                return "";
            }
        }
        /// <summary>
        /// Return formatted today Gregorian date based on your format
        /// </summary>
        /// <PARAM name="format"></PARAM>
        /// <returns></returns>
        public string GDateNow(string format)
        {
            try
            {
                return DateTime.Now.ToString(format, enCul.DateTimeFormat);
            }
            catch
            {
                //cur.Trace.Warn("GDateNow :\n" + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Return Today Hijri date and return it in yyyy/MM/dd format
        /// </summary>
        /// <returns></returns>
        public string HDateNow()
        {
            try
            {
                return DateTime.Now.ToString("yyyy/MM/dd", arCul.DateTimeFormat);
            }
            catch
            {
                //cur.Trace.Warn("HDateNow :\n" + ex.Message);
                return "";
            }
        }
        /// <summary>
        /// Return formatted today hijri date based on your format
        /// </summary>
        /// <PARAM name="format"></PARAM>
        /// <returns></returns>

        public string HDateNow(string format)
        {
            try
            {
                return DateTime.Now.ToString(format, arCul.DateTimeFormat);
            }
            catch
            {
                //cur.Trace.Warn("HDateNow :\n" + ex.Message);
                return "";
            }

        }

        /// <summary>
        /// Convert Hijri Date to it's equivalent Gregorian Date
        /// </summary>
        /// <PARAM name="hijri"></PARAM>
        /// <returns></returns>
        public string HijriToGreg(string hijri)
        {

            if (hijri.Length <= 0)
            {

                //cur.Trace.Warn("HijriToGreg :Date String is Empty");
                return "";
            }
            try
            {
                var tempDate = DateTime.ParseExact(hijri, allFormats,
                   arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString("yyyy/MM/dd", enCul.DateTimeFormat);
            }
            catch
            {
                //cur.Trace.Warn("HijriToGreg :" + hijri.ToString() + "\n" + ex.Message);
                return "";
            }
        }

        public DateTime HijriToGregDate(string hijri)
        {

            if (hijri.Length <= 0)
            {

                //cur.Trace.Warn("HijriToGreg :Date String is Empty");
                return DateTime.MinValue;
            }
            try
            {
                var tempDate = DateTime.ParseExact(hijri, allFormats,
                   arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                var g = tempDate.ToString("yyyy/MM/dd", enCul.DateTimeFormat);
                var v = Convert.ToDateTime(DateTime.Now.ToString());
                return Convert.ToDateTime(tempDate.ToString("yyyy/MM/dd", enCul.DateTimeFormat));
            }
            catch
            {
                //cur.Trace.Warn("HijriToGreg :" + hijri.ToString() + "\n" + ex.Message);
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// Convert Hijri Date to it's equivalent Gregorian Date
        /// and return it in specified format
        /// </summary>
        /// <PARAM name="hijri"></PARAM>
        /// <PARAM name="format"></PARAM>
        /// <returns></returns>
        public string HijriToGreg(string hijri, string format)
        {

            if (hijri.Length <= 0)
            {

                //cur.Trace.Warn("HijriToGreg :Date String is Empty");
                return "";
            }
            try
            {
                var tempDate = DateTime.ParseExact(hijri,
                   allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(format, enCul.DateTimeFormat);

            }
            catch
            {
                //cur.Trace.Warn("HijriToGreg :" + hijri.ToString() + "\n" + ex.Message);
                return "";
            }
        }
        /// <summary>
        /// Convert Gregoian Date to it's equivalent Hijir Date
        /// </summary>
        /// <PARAM name="greg"></PARAM>
        /// <returns></returns>
        public string GregToHijri(string greg)
        {

            if (greg.Length <= 0)
            {

                //cur.Trace.Warn("GregToHijri :Date String is Empty");
                return "";
            }
            try
            {
                var tempDate = DateTime.ParseExact(greg, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString("yyyy/MM/dd", arCul.DateTimeFormat);

            }
            catch
            {
                //cur.Trace.Warn("GregToHijri :" + greg.ToString() + "\n" + ex.Message);
                return "";
            }
        }
        /// <summary>
        /// Convert Hijri Date to it's equivalent Gregorian Date and
        /// return it in specified format
        /// </summary>
        /// <PARAM name="greg"></PARAM>
        /// <PARAM name="format"></PARAM>
        /// <returns></returns>
        public string GregToHijri(string greg, string format)
        {

            if (greg.Length <= 0)
            {

                //cur.Trace.Warn("GregToHijri :Date String is Empty");
                return "";
            }
            try
            {

                var tempDate = DateTime.ParseExact(greg, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(format, arCul.DateTimeFormat);

            }
            catch
            {
                //cur.Trace.Warn("GregToHijri :" + greg.ToString() + "\n" + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Return Gregrian Date Time as digit stamp
        /// </summary>
        /// <returns></returns>
        public string GTimeStamp()
        {
            return GDateNow("yyyyMMddHHmmss");
        }
        /// <summary>
        /// Return Hijri Date Time as digit stamp
        /// </summary>
        /// <returns></returns>
        public string HTimeStamp()
        {
            return HDateNow("yyyyMMddHHmmss");
        }


        /// <summary>
        /// Compare two instaces of string date 
        /// and return indication of thier values 
        /// </summary>
        /// <PARAM name="d1"></PARAM>
        /// <PARAM name="d2"></PARAM>
        /// <returns>positive d1 is greater than d2,
        /// negative d1 is smaller than d2, 0 both are equal</returns>
        public int Compare(string d1, string d2)
        {
            try
            {
                var date1 = DateTime.ParseExact(d1, allFormats,
                    arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                var date2 = DateTime.ParseExact(d2, allFormats,
                    arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return DateTime.Compare(date1, date2);
            }
            catch
            {
                //cur.Trace.Warn("Compare :" + "\n" + ex.Message);
                return -1;
            }

        }

        public static bool IsFromBiggerThanTo(DateTime from, DateTime to)
        {
            if (from.Year > to.Year)
                return true;
            else if (from.Year < to.Year)
                return false;
            else // == 
            {
                if (from.Month > to.Month)
                    return true;
                else if (from.Month < to.Month)
                    return false;
                else // == 
                {
                    if (from.Day > to.Day)
                        return true;
                    else if (from.Day < to.Day)
                        return false;
                    else // ==  // they are both the same date 
                        return true;
                }
            }
        }


    }

}