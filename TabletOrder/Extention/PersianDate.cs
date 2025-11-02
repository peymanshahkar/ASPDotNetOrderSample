using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace  Extention
{
    public static class PersianDate
    {

        public static string Persian2Miladi(string _date)
        {
            int year = int.Parse(_date.Substring(0, 4));
            int month = int.Parse(_date.Substring(5, 2));
            int day = int.Parse(_date.Substring(8, 2));
            PersianCalendar p = new PersianCalendar();
            DateTime date = p.ToDateTime(year, month, day, 0, 0, 0, 0);
            return date.ToShortDateString();
           
        }
        public static DateTime Persian2MiladiDate(this string _date)
        {
            int year = int.Parse(_date.Substring(0, 4));
            int month = int.Parse(_date.Substring(5, 2));
            int day = int.Parse(_date.Substring(8, 2));
            PersianCalendar p = new PersianCalendar();
            DateTime date = p.ToDateTime(year, month, day, 0, 0, 0, 0);
            return date;
        }
        private static PersianCalendar Persiancalender = new PersianCalendar();

        

        public static int year()
        {
            return int.Parse(Persiancalender.GetYear(DateTime.Now).ToString().Substring(2, 2));
        }

        public static string GetPersianDateNow()
        {
       
            return Persiancalender.GetYear(DateTime.Now).ToString().PadLeft(4,'0') + "/" + Persiancalender.GetMonth(DateTime.Now).ToString().PadLeft(2,'0') + "/" + Persiancalender.GetDayOfMonth(DateTime.Now).ToString().PadLeft(2,'0');
        }
        private static string ConvertMiladi2Persian(this DateTime dateT)
        {
            string Pdate= Persiancalender.GetYear(DateTime.Now).ToString() + "/" + Persiancalender.GetMonth(DateTime.Now).ToString() + "/" + Persiancalender.GetDayOfMonth(DateTime.Now).ToString();
            return   GetPersianDateInTrueFormat(Pdate);
        }
    
      
        private static string GetPersianDateInTrueFormat(string date)
        {
            
            string[] dt = date.Split('/');
         
            return dt[0] + "/" + dt[1].PadLeft(2,'0') + "/" + dt[2].PadLeft(2,'0');
        }


        public static string  PersianDayName(DateTime date)
        {
            if (date.DayOfWeek.ToString().ToLower() == "Saturday".ToLower())
            {
                return "شنبه";
            }
            else if (date.DayOfWeek.ToString().ToLower() == "Sunday".ToLower())
            {
                return "یکشنبه";
            }
            else if (date.DayOfWeek.ToString().ToLower() == "Monday".ToLower())
            {
                return "دوشنبه";
            }
            else if (date.DayOfWeek.ToString().ToLower() == "Tuesday".ToLower())
            {
                return "سه شنبه";
            }
            else if (date.DayOfWeek.ToString().ToLower() == "Wednesday".ToLower())
            {
                return "چهارشنبه";
            }
            else if (date.DayOfWeek.ToString().ToLower() == "Thursday".ToLower())
            {
                return "پنجشنبه";
            }
            else if (date.DayOfWeek.ToString().ToLower() == "Friday".ToLower())
            {
                return "جمعه";
            }
            return "";
        }
        public static string GetPersianDateTimeNowInTrueFormat()
        {
       
            return GetPersianDateNow() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        }
        public static string GetPersianDateTimeInTrueFormat(this DateTime dateT)
        {
            if (dateT != null)
                return ConvertMiladi2Persian(dateT) + " " +  dateT.Hour.ToString().PadLeft(2,'0') + ":" + dateT.Minute.ToString().PadLeft(2,'0') + ":" + dateT.Second.ToString().PadLeft(2,'0');
            else
                return GetPersianDateTimeNowInTrueFormat();
        }

        public static string ToPersianDate(this DateTime dateT)
        {

            if (dateT != null)
                return ConvertMiladi2Persian(dateT);
            else
                return GetPersianDateTimeNowInTrueFormat();
        }

    }
}
