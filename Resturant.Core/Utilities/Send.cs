using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Resturant.Core
{
    public class Send
    {

        public static string CheckMobile(string mobileNo)
        {
            if (!string.IsNullOrEmpty(mobileNo))
            {
                if (mobileNo.StartsWith("0"))
                {
                    mobileNo = mobileNo.Substring(1);
                }
                if (mobileNo.StartsWith("5"))
                {
                    mobileNo = "966" + mobileNo;
                }
                if (mobileNo.Length == 12 && mobileNo.StartsWith("9665"))
                {
                    return mobileNo;
                }
            }
            return null;
        }

        public static bool SendSMS(string smsApi, string MessageTo, string txtMessage)
        {
            try
            {
                WebClient client = new WebClient();
                string baseurl = string.Format(smsApi + "mobile={0}&msg={1}", MessageTo, txtMessage);
                Stream data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                data.Close();
                reader.Close();
                return true;
            }
            catch(Exception ec)
            {
                return false;
            }
        }
    }
}
