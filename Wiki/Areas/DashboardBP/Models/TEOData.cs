using System.Data.Entity;
using System.Linq;

namespace Wiki.Areas.DashboardBP.Models
{
    public class TEOData
    {
        public double GetRateUSDToYear(int year)
        {
            double rate = 0.0;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                rate = db.PZ_TEO.Include(d => d.PZ_PlanZakaz).Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP.Year == year).Sum(d => d.Rate);
            }
            return rate;
        }
    }
}