namespace Wiki.Areas.DashboardBP.Models
{
    public struct Task
    {
        public string name;
        public string id;
        public string parent;
        public string start;
        public string end;
        public int completed;
        public string owner;
    }
}