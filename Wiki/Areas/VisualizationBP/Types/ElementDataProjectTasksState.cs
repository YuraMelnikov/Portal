using System;

namespace Wiki.Areas.VisualizationBP.Types
{
    public class ElementDataProjectTasksState
    {
        double work;
        double remainingWork;
        DateTime startDate;
        DateTime finishDate;
        string name;
        string users;

        public double Work { get => work; set => work = value; }
        public double RemainingWork { get => remainingWork; set => remainingWork = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime FinishDate { get => finishDate; set => finishDate = value; }
        public string Name { get => name; set => name = value; }
        public string Users { get => users; set => users = value; }
    }
}