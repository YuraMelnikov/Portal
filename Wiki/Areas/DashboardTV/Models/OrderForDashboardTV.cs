using System;

namespace Wiki.Areas.DashboardTV.Models
{
    public class OrderForDashboardTV
    {
        string orderNumber;
        int current;
        DateTime dataOtgruzkiBP;
        DealsForDashboardTV[] deals;
        string color;

        public string OrderNumber { get => orderNumber; set => orderNumber = value; }
        public int Current { get => current; set => current = value; }
        public DealsForDashboardTV[] Deals { get => deals; set => deals = value; }
        public DateTime DataOtgruzkiBP { get => dataOtgruzkiBP; set => dataOtgruzkiBP = value; }
        public string Color { get => color; set => color = value; }
    }
}