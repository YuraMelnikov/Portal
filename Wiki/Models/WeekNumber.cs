using System;
using System.Globalization;

namespace Wiki.Models
{
    public class WeekNumber
    {
        public short GetNowWeekNumber()
        {

            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = new Calendar();
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            return myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
        }
    }
}