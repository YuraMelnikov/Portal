using System;

namespace Wiki.Areas.DashboardKO.Models
{
    public class TimesheetElamaent
    {
        public string user;
        public string date;
        public int stepUser;
        public int stepDate;
        public double data;

        public TimesheetElamaent()
        {

        }

        public TimesheetElamaent(string user, string date, int stepUser, int stepDate, double data)
        {
            this.user = user;
            this.date = date;
            this.stepUser = stepUser;
            this.stepDate = stepDate;
            this.data = Math.Round(data, 1);
        }
    }
}