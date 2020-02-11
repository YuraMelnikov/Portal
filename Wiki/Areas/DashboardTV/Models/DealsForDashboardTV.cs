using System;

namespace Wiki.Areas.DashboardTV.Models
{
    public class DealsForDashboardTV
    {
        int tcpm;
        DateTime from;
        DateTime to;
        bool milestone;
        string color;
        string user;

        public int TCPM { get => tcpm; set => tcpm = value; }
        public DateTime From { get => from; set => from = value; }
        public DateTime To { get => to; set => to = value; }
        public bool Milestone { get => milestone; set => milestone = value; }
        public string Color { get => color; set => color = value; }
        public string User { get => user; set => user = value; }
    }
}