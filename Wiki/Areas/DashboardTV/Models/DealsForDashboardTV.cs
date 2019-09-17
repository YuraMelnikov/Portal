using System;

namespace Wiki.Areas.DashboardTV.Models
{
    public class DealsForDashboardTV
    {
        int tcpm;
        DateTime from;
        DateTime to;

        public int TCPM { get => tcpm; set => tcpm = value; }
        public DateTime From { get => from; set => from = value; }
        public DateTime To { get => to; set => to = value; }
    }
}