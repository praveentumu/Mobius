using System;
using System.Collections.Generic;
using System.Text;

namespace FirstGenesis.Mobius.Common
{
    public class Helper
    {
        public static string ConvertToUTCFormat(DateTime dateTime)
        {
            try
            {
                if (dateTime == null)
                    return "";

                DateTime dt = Convert.ToDateTime(dateTime.ToString());

                return dt.ToFileTimeUtc().ToString();
            }
            catch (Exception)
            {
            }

            return "";
        }
        public static string ConvertToUTCFormat(string dateTime)
        {
            try
            {
                DateTime date = Convert.ToDateTime(dateTime);
                return date.ToFileTimeUtc().ToString();
            }
            catch (Exception)
            {
            }

            return "";
        }

        public static DateTime ConvertFromUTCFormat(string dateTime)
        {
//            if (!String.IsNullOrEmpty(dateTime))
                return DateTime.FromFileTimeUtc(Convert.ToInt64(dateTime));
//            else
//                return "";
        }
    }
}
