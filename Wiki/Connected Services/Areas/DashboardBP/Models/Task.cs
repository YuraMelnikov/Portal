namespace Wiki.Areas.DashboardBP.Models
{
    public struct Task
    {
        public string name;
        public string id;
        public string parent;
        public ulong start;
        public ulong end;
        public string completed;
        public string owner;
    }
}