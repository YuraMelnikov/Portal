using System.Collections.Generic;
using System.Linq;

namespace Wiki.Models
{
    public class WorkDeskKO
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        List<CMO_PreOrder> cMO_PreOrders = new List<CMO_PreOrder>();
        List<CMO_Order> cMO_Orders = new List<CMO_Order>();
        List<CMO_Order> cMO_DefaultOrders = new List<CMO_Order>();

        public WorkDeskKO()
        {
            cMO_PreOrders = new List<CMO_PreOrder>();
            cMO_Orders = new List<CMO_Order>();
            cMO_DefaultOrders = new List<CMO_Order>();
            CMO_DefaultOrders = db.CMO_Order.ToList()
                    .Where(d => d.datetimeFirstTenderFinish == null)
                    .Where(d => d.dateCloseOrder != null)
                    .Where(d => d.companyWin > 0)
                    .ToList();
            CMO_PreOrders = db.CMO_PreOrder.Where(d => d.firstTenderStart == false).ToList();
            CMO_Orders = db.CMO_Order.OrderByDescending(d => d.id).Take(120).ToList();
        }

        public List<CMO_PreOrder> CMO_PreOrders { get => cMO_PreOrders; set => cMO_PreOrders = value; }
        public List<CMO_Order> CMO_Orders { get => cMO_Orders; set => cMO_Orders = value; }
        public List<CMO_Order> CMO_DefaultOrders { get => cMO_DefaultOrders; set => cMO_DefaultOrders = value; }
    } 
}