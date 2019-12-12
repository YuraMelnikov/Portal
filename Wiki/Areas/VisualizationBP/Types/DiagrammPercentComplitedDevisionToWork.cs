namespace Wiki.Areas.VisualizationBP.Types
{
    public class DiagrammPercentComplitedDevisionToWork
    {
        string devisionName;
        int percentComplited;
        int percentRemainingWork;

        public string DevisionName { get => devisionName; set => devisionName = value; }
        public int PercentComplited { get => percentComplited; set => percentComplited = value; }
        public int PercentRemainingWork { get => percentRemainingWork; set => percentRemainingWork = value; }

        public DiagrammPercentComplitedDevisionToWork(string devisionName)
        {
            this.devisionName = devisionName;
            this.percentComplited = 100;
            this.percentRemainingWork = 0;
        }
    }
}