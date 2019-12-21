using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using SmartCattle.Web.ExtensionMethods;
using System.Threading;

namespace SmartCattle.Web.Helper
{
    public class DateHelper
    {
         
        #region holidays
        public static List<DateTime> holidays = new List<DateTime>()
        {
            new DateTime(1395, 1, 1, new PersianCalendar()),
            new DateTime(1395, 2, 1, new PersianCalendar()),
            new DateTime(1395, 3, 1, new PersianCalendar()),
            new DateTime(1395, 4, 1, new PersianCalendar()),
            new DateTime(1395, 12, 12, new PersianCalendar()),
            new DateTime(1395, 2, 2, new PersianCalendar()),
            new DateTime(1395, 2, 16, new PersianCalendar()),
            new DateTime(1395, 3, 2, new PersianCalendar()),
            new DateTime(1395, 3, 15, new PersianCalendar()),

            new DateTime(1395, 4, 7, new PersianCalendar()),
            new DateTime(1395, 4, 16, new PersianCalendar()),
            new DateTime(1395, 4, 17, new PersianCalendar()),
            new DateTime(1395, 5, 9, new PersianCalendar()),
            new DateTime(1395, 6, 22, new PersianCalendar()),
            new DateTime(1395, 6, 30, new PersianCalendar()),

            new DateTime(1395, 7, 20, new PersianCalendar()),
            new DateTime(1395, 7, 21, new PersianCalendar()),
            new DateTime(1395, 8, 30, new PersianCalendar()),
            new DateTime(1395, 9, 8, new PersianCalendar()),
            new DateTime(1395, 9, 10, new PersianCalendar()),
            new DateTime(1395, 9, 27, new PersianCalendar()),

            new DateTime(1395, 12, 12, new PersianCalendar()),
            new DateTime(1395, 12, 29, new PersianCalendar()),
        };
        #endregion

        //public static DateTime toGeorjian(DateTime? persianDate)
        public static DateTime toGeorjian(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            PersianCalendar persianCal = new PersianCalendar();
            DateTime persianDate = new DateTime(year, month, day, hour, minute, second, persianCal);
            return persianDate;
        }

        public static string GetMonthName(int month)
        {
            string lang = HttpContext.Current.Request.RequestContext.RouteData.Values["language"] as string ?? "fa";
            switch (lang)
            {
                case "nl":
                case "en":
                    switch (month)
                    {
                        case 1: return "January ";
                        case 2: return "February  ";
                        case 3: return "March ";
                        case 4: return "April";
                        case 5: return "May ";
                        case 6: return "June ";
                        case 7: return "July ";
                        case 8: return "Augest ";
                        case 9: return "September ";
                        case 10: return "October ";
                        case 11: return "November ";
                        case 12: return "December ";
                        default: return "Invalid Month";
                    } 
                case "fa":
                    switch (month)
                    {
                        case 1: return "فروردین ";
                        case 2: return "اردیبهشت ";
                        case 3: return "خرداد ";
                        case 4: return "تیر";
                        case 5: return "مرداد ";
                        case 6: return "شهریور ";
                        case 7: return "مهر ";
                        case 8: return "آبان ";
                        case 9: return "آذر ";
                        case 10: return "دی ";
                        case 11: return "بهمن ";
                        case 12: return "اسفند ";
                        default: return "ماه نامعتبر";
                    }               
            }
            return "";

        }

        public static DateTime toGeorjian(string persianDate)
        {
            persianDate = persianDate.ToEnglishNumber(); 
            DateTime fdate = Convert.ToDateTime(persianDate);
            GregorianCalendar gcalendar = new GregorianCalendar();
            DateTime eDate = gcalendar.ToDateTime(
                   gcalendar.GetYear(fdate),
                   gcalendar.GetMonth(fdate),
                   gcalendar.GetDayOfMonth(fdate),
                   gcalendar.GetHour(fdate),
                   gcalendar.GetMinute(fdate),
                   gcalendar.GetSecond(fdate), 0);
            return eDate;
        }

        public static string toPersian(DateTime? GregorianDate)
        {
            if (GregorianDate != null)
            {
                DateTime d = (DateTime)GregorianDate;
                PersianCalendar pc = new PersianCalendar();
                return string.Format("{0}/{1}/{2} {3}:{4}:{5}", pc.GetYear(d), pc.GetMonth(d), pc.GetDayOfMonth(d), pc.GetHour(d), pc.GetMinute(d), pc.GetSecond(d));
            }
            return "";
        }

        public static string getPersianDayOfWeek(DateTime? date)
        {
            if (date != null)
            {
                DateTime notNulldate = (DateTime)date;
                switch (notNulldate.DayOfWeek)
                {
                    case DayOfWeek.Friday: return "جمعه ";
                    case DayOfWeek.Sunday: return "یکشنبه ";
                    case DayOfWeek.Monday: return "دوشنبه ";
                    case DayOfWeek.Tuesday: return "سه شنبه ";
                    case DayOfWeek.Wednesday: return "چهارشنبه ";
                    case DayOfWeek.Thursday: return "پنجشنبه ";
                    case DayOfWeek.Saturday: return "شنبه ";
                    default: return "روز هفته نامعتبر";
                }
            }
            return "روز هفته نامعتبر";
        }

        public static int getPersianDayOfMonth(DateTime? date)
        {
            if (date != null)
            {
                PersianCalendar PC = new PersianCalendar();
                return PC.GetDayOfMonth((DateTime)date);
            }
            return -1;
        }

        public static DateTime ConvertToGeorginDate(string DateTimeStr)
        {
            DateTime retDateTime = DateTime.Now;
            if (DateTimeStr != null)
            {
                DateTimeStr = DateTimeStr.Replace("۰", "0");
                DateTimeStr = DateTimeStr.Replace("۱", "1");
                DateTimeStr = DateTimeStr.Replace("۲", "2");
                DateTimeStr = DateTimeStr.Replace("۳", "3");
                DateTimeStr = DateTimeStr.Replace("۴", "4");
                DateTimeStr = DateTimeStr.Replace("۵", "5");
                DateTimeStr = DateTimeStr.Replace("۶", "6");
                DateTimeStr = DateTimeStr.Replace("۷", "7");
                DateTimeStr = DateTimeStr.Replace("۸", "8");
                DateTimeStr = DateTimeStr.Replace("۹", "9");
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("fa-IR"))
                {
                    String[] SplitedEventDate = DateTimeStr.Split(' ')[0].Split('/');
                    
                    if (SplitedEventDate.Length >= 4)
                    {
                        String[] SplitedEventDateHour = DateTimeStr.Split(' ')[1].Split(':');
                        retDateTime = DateHelper.toGeorjian(
                            Convert.ToInt16(SplitedEventDate[0]),
                            Convert.ToInt16(SplitedEventDate[1]),
                            Convert.ToInt16(SplitedEventDate[2]),
                            Convert.ToInt16(SplitedEventDateHour[0]),
                            Convert.ToInt16(SplitedEventDateHour[1]),
                            0);
                    }
                    else
                    {
                        retDateTime = DateHelper.toGeorjian(
                            Convert.ToInt16(SplitedEventDate[0]),
                            Convert.ToInt16(SplitedEventDate[1]),
                            Convert.ToInt16(SplitedEventDate[2]),
                            0, 0, 0);
                    }
                }
            }
            return retDateTime;
        }

        public static int getPersianYear(DateTime? date)
        {
            if (date != null)
            {
                PersianCalendar PC = new PersianCalendar();
                return PC.GetYear((DateTime)date);
            }
            return -1;
        }

        public static string getPersianMonth(DateTime? date)
        {
            PersianCalendar PC = new PersianCalendar();
            if (date != null)
            {
                switch (PC.GetMonth((DateTime)date))
                {
                    case 1: return "فروردین ";
                    case 2: return "اردیبهشت ";
                    case 3: return "خرداد ";
                    case 4: return "تیر";
                    case 5: return "مرداد ";
                    case 6: return "شهریور ";
                    case 7: return "مهر ";
                    case 8: return "آبان ";
                    case 9: return "آذر ";
                    case 10: return "دی ";
                    case 11: return "بهمن ";
                    case 12: return "اسفند ";
                    default: return "ماه نامعتبر";
                }
            }
            return "ماه نامعتبر";
        }

        public static bool isHoliday(DateTime? date)
        {
            if (date != null)
            {
                DateTime validDate = (DateTime)date;
                PersianCalendar Pc = new PersianCalendar();
                if (Pc.GetDayOfWeek(validDate) == DayOfWeek.Friday)
                    return true;
                DateTime holiday = holidays.Where(d => d.Date == validDate.Date).FirstOrDefault();
                if (holiday.Year != 0001)
                    return true;
                return false;
            }
            return false;
        }

        public static List<DateTime> getDaysofMonth(DateTime? date)
        {
            List<DateTime> monthDays = new List<DateTime>();
            if (date != null)
            {
                PersianCalendar PC = new PersianCalendar();
                int dayInMonth = PC.GetDaysInMonth(PC.GetYear((DateTime)date), PC.GetMonth((DateTime)date));
                DateTime startOfMonth = new DateTime((PC.GetYear((DateTime)date)), PC.GetMonth((DateTime)date), 1, PC);

                for (int i = 0; i < dayInMonth; i++)
                {
                    monthDays.Add(startOfMonth.AddDays(i));
                }
            }
            return monthDays;
        }

        public static int calculateAge(DateTime? date)
        {
            if (date != null)
                return (DateTime.Now.Year - ((DateTime)date).Year);
            return 0;
        }

        public static int Diff(DateTime? date)
        {
            if(date!=null)
            {
                return (DateTime.Now - date.Value).Days;
            }
            return 0;
        }

        public static DateTime TimeStampToDateTime (string tiemstamp)
        {
            double parsed;
            if (double.TryParse(tiemstamp.Substring(0,10), out parsed))
            {
                try
                {
                    DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    return dtDateTime.AddSeconds(parsed).ToLocalTime();
                }
                catch(Exception ex)
                {

                }
            }
            return new DateTime();
        }

        public static long ToTimestamp(DateTime target)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            var unixTimestamp = System.Convert.ToInt64((target - date).TotalSeconds);

            return unixTimestamp;
        }
    }
}