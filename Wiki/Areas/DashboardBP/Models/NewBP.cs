namespace Wiki.Areas.DashboardBP.Models
{
    public class NewBP
    {
        public NewBP()
        {
            new State().UpdateBP();
            //updatedbPZ
            new ProjectsList().CreatePZList();
            new RatePlan().CreateNewRatePlan();
            //new remaining hss
            //new hss plan


            //tableManufacturingPO, short & long
        }
    }
}