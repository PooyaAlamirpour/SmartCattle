using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Runtime.Remoting.Contexts;

namespace SmartCattle.Core
{
   public class Utility
    {
        //public static double? EvaluateExpression(string expression, List<KeyValuePair<string, double>> variables)
        //{
        //    NCalc.Expression exp = new NCalc.Expression(expression);
        //    foreach (KeyValuePair<string, double> var in variables)
        //    {
        //        exp.Parameters[var.Key] = var.Value;
        //    }
        //    return exp.Evaluate() as double?;
        //}

        public static DbGeography CreatePoint(double lat, double lon, int srid = 4326)
        {
            string wkt = String.Format("POINT({0} {1})", lon, lat);
            wkt = wkt.Replace("/", ".");
            return DbGeography.PointFromText(wkt, srid);
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        public static string toPersianNumber(string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(j.ToString(), persian[j]);

            return input;
        }

        public static string toEnglishNumber(string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string ConvertFromPersian(String PersianDate)
        {
            String[] spliteddate = PersianDate.Split(' ');
            String[] spliteYear = spliteddate[0].Split('/');
            String[] splitedTime = spliteddate[1].Split(':');
            DateTime dt = Convert.ToDateTime(PersianDate);// DateTime.ParseExact(PersianDate, "yyyy/MM/dd HH:mm", null);
            int year = Convert.ToInt16(spliteYear[0]);
            int month = Convert.ToInt16(spliteYear[1]);
            int day = Convert.ToInt16(spliteYear[2]);
            DateTime georgianDateTime = new DateTime(year, month, day, new System.Globalization.PersianCalendar());
            return georgianDateTime.Year + "-" + int2Str(georgianDateTime.Month) + "-" + int2Str(georgianDateTime.Day) + " " + int2Str(dt.Hour) + ":" + int2Str(dt.Minute);
        }

        public static string ConvertToPersian(DateTime _date)
        {
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(pc.GetYear(_date).ToString("0000"));
            sb.Append("/");
            sb.Append(pc.GetMonth(_date).ToString("00"));
            sb.Append("/");
            sb.Append(pc.GetDayOfMonth(_date).ToString("00"));
            return sb.ToString();
        }

        public static string ConvertToPersianWithTime(DateTime _date)
        {
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(pc.GetYear(_date).ToString("0000"));
            sb.Append("/");
            sb.Append(pc.GetMonth(_date).ToString("00"));
            sb.Append("/");
            sb.Append(pc.GetDayOfMonth(_date).ToString("00"));
            sb.Append(" ");
            sb.Append(pc.GetHour(_date).ToString("00"));
            sb.Append(":");
            sb.Append(pc.GetMinute(_date).ToString("00"));
            return sb.ToString();
        }

        public static string ConvertToPersian(String input)
        {
            DateTime _date = DateTime.ParseExact(input, "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(pc.GetYear(_date).ToString("0000"));
            sb.Append("/");
            sb.Append(pc.GetMonth(_date).ToString("00"));
            sb.Append("/");
            sb.Append(pc.GetDayOfMonth(_date).ToString("00"));
            sb.Append(" ");
            sb.Append(pc.GetHour(_date).ToString("00"));
            sb.Append(":");
            sb.Append(pc.GetMinute(_date).ToString("00"));
            return sb.ToString();
        }

        public static String int2Str(int input)
        {
            String retValue = "";
            if(input <= 9)
            {
                retValue = "0" + input.ToString();
            }
            else
            {
                retValue = input.ToString();
            }
            return retValue;
        }

        
    }
}
