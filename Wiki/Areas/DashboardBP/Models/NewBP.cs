namespace Wiki.Areas.DashboardBP.Models
{
    public class NewBP
    {
        public NewBP()
        {
            new State().UpdateBP();
            new ProjectsList().CreatePZList();

            //new rate plan
            //new remaining hss
            //new hss plan
        }
    }
}