namespace Wiki.Areas.DashboardBP.Models
{
    public class NewBP
    {
        public NewBP()
        {
            new ProjectsList().CreatePZList();
            new RatePlan().CreateNewRatePlan();
            new RemainingHSS().CreateNew();
            new HssPlan().CreateNew();
        }
    }
}