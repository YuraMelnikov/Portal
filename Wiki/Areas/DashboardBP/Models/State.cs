using System;
using System.Linq;

namespace Wiki.Areas.DashboardBP.Models
{
    public class State
    {
        public void UpdateBP()
        {
            DeactiveState();
            CreateNewState();
        }

        bool DeactiveState()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                try
                {
                    DashboardBP_State state = db.DashboardBP_State.First(d => d.active == true);
                    db.Entry(state).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            return true;
        }

        bool CreateNewState()
        {
            DashboardBP_State state = new DashboardBP_State
            {
                active = true,
                datetime = DateTime.Now,
                numberWeek = new Wiki.Models.WeekNumber().GetNowWeekNumber()
            };
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.DashboardBP_State.Add(state);
                db.SaveChanges();
            }
            return true;
        }
    }
}