namespace Wiki.Areas.DashboardBP.Models
{
    public class NewBP
    {
        public NewBP()
        {
            //new State().UpdateBP();
            ////update PZ dateShPlan & ProjectUID
            //new ProjectsList().CreatePZList();
            //new RatePlan().CreateNewRatePlan();
            new SQLHSSPO().CreateNew();
            new RemainingHSS().CreateNew();
            new HssPlan().CreateNew();
        }
    }
}