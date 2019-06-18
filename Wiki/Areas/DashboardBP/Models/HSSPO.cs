using System.Linq;

namespace Wiki.Areas.DashboardBP.Models
{
    public class HSSPO
    {
        public void CreateNew()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                int state = db.DashboardBP_State.First(d => d.active == true).id;
                var listHSS = db.HSSPO.ToList();
                foreach (var data in listHSS)
                {
                    DashboardBP_HSSPO dashboardBP_HSSPO = new DashboardBP_HSSPO
                    {
                        id_DashboardBP_State = state,
                        id_PZ_PlanZakaz = db.PZ_PlanZakaz.First(d => d.PlanZakaz.ToString() == data.OrderPZ).Id,
                        taskUID = data.TaskUID.Value,
                        taskName = data.TaskName,
                        timeByDay = data.TimeByDay.Value,
                        assignmentWork = (double)data.AssignmentWork,
                        rateUSD = (double)data.RatePlan,
                        rateHoure = (double)data.RateHoure,
                        xReate = (double)data.xRate,
                        ssmPlan = (double)data.SSMPlan,
                        ssmPlanHoure = (double)data.ssmHoure,
                        xSsm = (double)data.xSSM,
                        ssmFact = (double)data.SSMFact,
                        ssmFactHoure = (double)data.ssmFactHoure,
                        xSsmFact = (double)data.xSSMFact,
                        ssrPlan = (double)data.SSRPlan,
                        ssrHoure = (double)data.ssrHoure,
                        xSsr = (double)data.xSSR,
                        ik = (double)data.ik,
                        ikHoure = (double)data.ikHoure,
                        xIk = (double)data.xIK,
                        ppk = (double)data.ppk,
                        ppkHoure = (double)data.ppkHoure,
                        xPpk = (double)data.xPPK,
                        pi = (double)data.pi,
                        piHoure = (double)data.piHoure,
                        xPi = (double)data.xPI,
                        nop = (double)data.nop,
                        nopHoure = (double)data.nopHoure,
                        xNop = (double)data.nopHoure
                    };
                    db.DashboardBP_HSSPO.Add(dashboardBP_HSSPO);
                    db.SaveChanges();
                }
            }
        }
    }
}