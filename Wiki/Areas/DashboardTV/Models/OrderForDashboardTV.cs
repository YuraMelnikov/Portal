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
        int duration;
        int remainingDuration;
        int percentComplited;

        public string OrderNumber { get => orderNumber; set => orderNumber = value; }
        public int Current { get => current; set => current = value; }
        public DealsForDashboardTV[] Deals { get => deals; set => deals = value; }
        public DateTime DataOtgruzkiBP { get => dataOtgruzkiBP; set => dataOtgruzkiBP = value; }
        public string Color { get => color; set => color = value; }
        public bool Milestone { get => milestone; set => milestone = value; }
        public DateTime ContractDateComplited { get => contractDateComplited; set => contractDateComplited = value; }
        public int Failure { get => failure; set => failure = value; }
        public int Duration { get => duration; set => duration = value; }
        public int RemainingDuration { get => remainingDuration; set => remainingDuration = value; }
        public int PercentComplited { get => percentComplited; set => percentComplited = value; }
    }
}