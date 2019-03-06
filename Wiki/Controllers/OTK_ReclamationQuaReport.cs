using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Controllers
{
    public class OTK_ReclamationQuaReport
    {
        public DateTime DateShopPlanZakaz { get; set; }
        public int PlanZakaz { get; set; }
        public int CountReclamation4 { get; set; }
        public int CountReclamation8 { get; set; }
        public int CountReclamation16 { get; set; }
        public int CountReclamationBig { get; set; }
        public int CountReclamationNo { get; set; }
        public int AllCount { get; set; }
        public float PercentResult { get; set; }
    }
}