namespace Wiki.Areas.DashboardBP.Models
{
    public struct HSSToDay
    {
        ulong xday;
        int hss;

        public HSSToDay(ulong xday, int hss)
        {
            this.xday = xday;
            this.hss = hss;
        }
        public ulong XDay { get => xday; set => xday = value; }

        public int Hss { get => hss; set => hss = value; }
    }
}