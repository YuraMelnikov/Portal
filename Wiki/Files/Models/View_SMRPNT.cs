using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class View_SMRPNT
    {
        public List<SMRPNR_Order> NoInvitation { get; set; }
        public List<SMRPNR_Order> Preparation { get; set; }
        public List<SMRPNR_Order> Work { get; set; }
        public List<SMRPNR_Order> TeckAct { get; set; }
        public List<SMRPNR_Order> FinAct { get; set; }
        public List<SMRPNR_Order> GetMoney { get; set; }
        public List<SMRPNR_Order> CostIsNull { get; set; }
    }
}