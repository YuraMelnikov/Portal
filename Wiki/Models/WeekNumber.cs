using System;
using System.Globalization;

namespace Wiki.Models
{
    public class WeekNumber
    {
        public short GetNowWeekNumber()
        {
            CultureInfo myCI = new CultureInfo("en-US");
            System.Globalization.Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            return (short)myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
        }
    }
}