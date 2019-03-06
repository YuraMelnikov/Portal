using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class ViewReclamatoinOrder
    {
        public List<Reclamation_Order> ActiveOrder { get; set; }
        public List<Reclamation_Order> CloseOrder { get; set; }
    }
}