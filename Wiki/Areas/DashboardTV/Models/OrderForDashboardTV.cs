using System;

namespace Wiki.Areas.DashboardTV.Models
{
    public class OrderForDashboardTV
    {
        string orderNumber;
        int current;
        DateTime dataOtgruzkiBP;
        DateTime contractDateComplited;
        int failure;
        DealsForDashboardTV[] deals;
        string color;
        bool milestone;
        double duration;
        double remainingDuration;
        int percentComplited;
        string taskName;
        string userName;

        public string OrderNumber { get => orderNumber; set => orderNumber = value; }
        public int Current { get => current; set => current = value; }
        public DealsForDashboardTV[] Deals { get => deals; set => deals = value; }
        public DateTime DataOtgruzkiBP { get => dataOtgruzkiBP; set => dataOtgruzkiBP = value; }
        public string Color { get => color; set => color = value; }
        public bool Milestone { get => milestone; set => milestone = value; }
        public DateTime ContractDateComplited { get => contractDateComplited; set => contractDateComplited = value; }
        public int Failure { get => failure; set => failure = value; }
        public double Duration { get => Math.Round(duration, 1); set => duration = value; }
        public double RemainingDuration { get => Math.Round(remainingDuration, 1); set => remainingDuration = value; }
        public int PercentComplited { get => percentComplited; set => percentComplited = value; }
        public string TaskName { get => taskName; set => taskName = value; }
        public string UserName { get => userName; set => userName = value; }
    }
}