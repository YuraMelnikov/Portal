namespace Wiki.Areas.DashboardBP.Models
{
    public struct HSSToDay
    {
        int hss;
        ulong day;

        public HSSToDay(int hss, ulong day)
        {
            this.hss = hss;
            this.day = day;
        }

        public int Hss { get => hss; set => hss = value; }
        public ulong Day { get => day; set => day = value; }
    }
}