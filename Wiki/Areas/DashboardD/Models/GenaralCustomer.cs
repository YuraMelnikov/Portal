using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Areas.DashboardD.Models
{
    public class GenaralCustomer
    {
        string customer;
        int ssm;
        int percentSSM;
        int profit;
        int percentProfit;
        int rate;
        int percentRate;
        int fssm;
        int percentFSSM;

        public string Customer { get => customer; set => customer = value; }
        public int Ssm { get => ssm; set => ssm = value; }
        public int PercentSSM { get => percentSSM; set => percentSSM = value; }
        public int Profit { get => profit; set => profit = value; }
        public int PercentProfit { get => percentProfit; set => percentProfit = value; }
        public int Rate { get => rate; set => rate = value; }
        public int PercentRate { get => percentRate; set => percentRate = value; }
        public int Fssm { get => fssm; set => fssm = value; }
        public int PercentFSSM { get => percentFSSM; set => percentFSSM = value; }
    }
}