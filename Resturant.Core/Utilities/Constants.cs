using System.Data;

namespace Resturant.Core.Utilities
{
    public static class Constants
    {
        public const string DefaultLanguage = "en-US";

        #region Lists
        public static string KEY = "Key";
        public static string VALUE = "Value";
        static public string[] Language = { "en", "ar", "fr" };
        static public string DbDateFormate = "yyyy-MM-dd";
        static public string DisplayDateFormat = "dd/MM/yyyy";
        static public string DisplayTimeFormat = "HH:mm";
        static public string DisplayDateTimeFormat = "dd/MM/yyyy hh:mm:ss tt";
        static public string AutoCompleteDateFormat = "dd/MM/yyyy";
        static public string Session_ApprovalExpression = "strExpression";
        static public string Session_BoolLookupManagerIsUpdate = "isUpdate";
        #endregion

        public static DataTable YesNoValuesEN()
        {
            var table = new DataTable();
            table.Columns.Add("Key", typeof(int));
            table.Columns.Add("Value", typeof(string));
            table.Rows.Add(0, "No");
            table.Rows.Add(1, "Yes");
            return table;
        }
        public enum LogLevel
        {
            DontLog = 1,
            NormalLog = 2
        }

        public static DataTable YesNoValuesAR()
        {
            var table = new DataTable();
            table.Columns.Add("Key", typeof(int));
            table.Columns.Add("Value", typeof(string));
            table.Rows.Add(0, "لا");
            table.Rows.Add(1, "نعم");
            return table;
        }
        public enum LangInx
        {
            en = 0,
            ar = 1,
            fr = 2
        }
        public enum AlertIcons
        {
            Information, Warning, Erorr
        }

    }
}
