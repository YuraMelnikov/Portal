using System.Collections.Generic;
using System.Linq;

namespace Wiki.Models.CMO
{
    public class CMO_ReportList
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        List<CMO_Report> cMO_Report = new List<CMO_Report>();
        List<CMO_Order> cMO_DefaultOrders = new List<CMO_Order>();

        public CMO_ReportList()
        {
            cMO_Report = new List<CMO_Report>();
            CMO_Report = db.CMO_Report.OrderByDescending(d => d.Id_Order).Take(150).ToList();

            cMO_DefaultOrders = new List<CMO_Order>();
            CMO_DefaultOrders = db.CMO_Order.ToList()
                    .Where(d => d.datetimeFirstTenderFinish == null)
                    .Where(d => d.dateCloseOrder != null)
                    .Where(d => d.companyWin > 0)
                    .ToList();
        }

        public List<CMO_Report> CMO_Report { get => cMO_Report; set => cMO_Report = value; }
        public List<CMO_Order> CMO_DefaultOrders { get => cMO_DefaultOrders; set => cMO_DefaultOrders = value; }
    }
}