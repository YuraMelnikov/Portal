using System;
using System.Linq;

namespace Wiki.Areas.DashboardBP.Models
{
    public class HssPlan
    {
        private readonly double plan = 10000000.0;

        public void CreateNew()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                DashboardHSSPlan hssPlan = new DashboardHSSPlan
                {
                    plan = plan,
                    id_DashboardBP_State = db.DashboardBP_State.First(d => d.active == true).id,
                    hss = new TEOData().GetHSSToYear(DateTime.Now.Year)
                };
                db.DashboardHSSPlan.Add(hssPlan);
                db.SaveChanges();
            }
        }
    }
}