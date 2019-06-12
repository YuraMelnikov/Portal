namespace Wiki.Areas.DashboardBP.Models
{
    public struct Project
    {
        string name;
        Task[] tasks;

        public string Name { get => name; set => name = value; }
        public Task[] Tasks { get => tasks; set => tasks = value; }
    }
}