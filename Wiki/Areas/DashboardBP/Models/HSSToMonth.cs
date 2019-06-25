namespace Wiki.Areas.DashboardBP.Models
{
    public struct HSSToMonth
    {
        int hss;
        string month;

        public HSSToMonth(int hss, string month)
        {
            this.hss = hss;
            this.month = month;
        }

        public int Hss { get => hss; set => hss = value; }
        public string Month { get => month; set => month = value; }
    }
}