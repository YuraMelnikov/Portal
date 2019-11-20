using System;
using System.Linq;

namespace Wiki.Areas.DashboardBP.Models
{
    public class RatePlan
    {
        readonly double plan = 16500000.0;

        public void CreateNewRatePlan()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                DashboardRatePlan dashboardRatePlan = new DashboardRatePlan
                {
                    plan = plan,
                    fact = new TEOData().GetRateUSDToYear(DateTime.Now.Year)
                };
                db.DashboardRatePlan.Add(dashboardRatePlan);
                db.SaveChanges();
            }
        }
    }
}