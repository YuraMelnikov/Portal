using System;

namespace Wiki.Areas.DashboardD.Models
{
    public class GeneralID
    {
        string month;
        int year;
        int rate;
        int ssm;
        int ssw;
        int ik;
        int pk;
        int pi;
        int profit;
        int monthNum;
        string quart;

        public string Month { get => month; set => month = value; }
        public int Year { get => year; set => year = value; }
        public int Rate { get => rate; set => rate = value; }
        public int SSM { get => ssm; set => ssm = value; }
        public int SSW { get => ssw; set => ssw = value; }
        public int IK { get => ik; set => ik = value; }
        public int PK { get => pk; set => pk = value; }
        public int PI { get => pi; set => pi = value; }
        public int Profit { get => profit; set => profit = value; }
        public int MonthNum { get => monthNum; set => monthNum = value; }
        public string Quart { get => quart; set => quart = value; }
    }
}