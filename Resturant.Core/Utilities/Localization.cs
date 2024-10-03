using System.Globalization;
using System.Threading;

namespace Resturant.Core.Utilities
{
    public class Localization
    {
        public static void SetCulture(string language)
        {
            if (language != null && language.Trim() != "")
            {
                var ci = new CultureInfo(language, false)
                {
                    DateTimeFormat = { ShortDatePattern = "dd/MM/yyyy" },
                    NumberFormat = { CurrencySymbol = "�.�.�" }
                };
                int[] mySizes = { 0 };
                ci.NumberFormat.NumberGroupSizes = mySizes;
                ci.NumberFormat.CurrencyGroupSizes = mySizes;

                Thread.CurrentThread.CurrentCulture = ci;
                var ciui = new CultureInfo(language, false)
                {
                    DateTimeFormat = {ShortDatePattern = "dd/MM/yyyy"},
                    NumberFormat = {NumberGroupSizes = mySizes, CurrencyGroupSizes = mySizes}
                };
                Thread.CurrentThread.CurrentUICulture = ciui;
            }
            else
            {
                var ci = new CultureInfo(Constants.DefaultLanguage, false)
                {
                    DateTimeFormat = {ShortDatePattern = "dd/MM/yyyy"}, NumberFormat = {CurrencySymbol = "�.�.�"}
                };
                //"$";
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 12; //Make negative currencies have -ve sign
        }
    }
}
