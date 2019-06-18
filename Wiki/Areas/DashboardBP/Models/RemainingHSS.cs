using System;
using System.Linq;

namespace Wiki.Areas.DashboardBP.Models
{
    public class RemainingHSS
    {
        readonly double plan = 3000000.0;

        //public void CreateNew()
        //{
        //    using (PortalKATEKEntities db = new PortalKATEKEntities())
        //    {
        //        DashboardRemaining data = new DashboardRemaining
        //        {
        //            plan = plan,
        //            id_DashboardBP_State = db.DashboardBP_State.First(d => d.active == true).id,
        //            rate = new TEOData().GetRateUSDToYear(DateTime.Now.Year)
        //        };
        //        db.DashboardRatePlan.Add(dashboardRatePlan);
        //        db.SaveChanges();
        //    }
        //}
    }
}