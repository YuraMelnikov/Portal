using System;
using System.Linq;

namespace Wiki.Areas.DashboardBP.Models
{
    public class RemainingHSS
    {
        private readonly double plan = 3000000.0;

        public void CreateNew()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                DashboardRemaining remainingHss = new DashboardRemaining
                {
                    fact = new TEOData().GetRemainingHSS()
                };
                db.DashboardRemaining.Add(remainingHss);
                db.SaveChanges();
            }
        }
    }
}