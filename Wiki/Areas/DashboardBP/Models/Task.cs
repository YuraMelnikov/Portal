namespace Wiki.Areas.DashboardBP.Models
{
    public struct Task
    {
        string name;
        string id;
        string parent;
        string start;
        string end;
        int completed;
        string owner;

        public string Name { get => name; set => name = value; }
        public string Id { get => id; set => id = value; }
        public string Parent { get => parent; set => parent = value; }
        public string Start { get => start; set => start = value; }
        public string End { get => end; set => end = value; }
        public int Completed { get => completed; set => completed = value; }
        public string Owner { get => owner; set => owner = value; }
    }
}