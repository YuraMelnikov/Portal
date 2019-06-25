namespace Wiki.Areas.DashboardBP.Models
{
    public struct HSSToMonth
    {
        string hss;
        string month;

        public HSSToMonth(string hss, string month)
        {
            this.hss = hss;
            this.month = month;
        }

        public string Hss { get => hss; set => hss = value; }
        public string Month { get => month; set => month = value; }
    }
}