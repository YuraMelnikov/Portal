using System.Collections.Generic;
using System.Linq;

namespace Wiki.Models
{
    public class ElementsWorkDeskOS
    {
        public ElementsWorkDeskOS()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();

            this.ActiveOrder = db.CMO_Order.Where(d => d.firstTenderStart == false).ToList();
            this.ActiveFirstUpload = db.CMO_Order.Where(d => d.firstTenderStart == true && d.companyWin == null).ToList();
            this.ActiveOrderClose = db.CMO_Report.Where(d => d.FactFinishDate == null & d.PlanFinishWork1 != null).ToList();
        }

        public List<CMO_Order> ActiveOrder { get; set; }
        public List<CMO_Order> ActiveFirstUpload { get; set; }
        public List<CMO_Report> ActiveOrderClose { get; set; }
    }
}