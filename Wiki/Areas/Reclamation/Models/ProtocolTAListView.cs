using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Areas.Reclamation.Models
{
    public class ProtocolTAListView
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        List<ProtocolTAView> protocolTAViews;

        public List<ProtocolTAView> ProtocolTAViews { get => protocolTAViews; set => protocolTAViews = value; }

        public ProtocolTAListView()
        {
            protocolTAViews = new List<ProtocolTAView>();
            foreach(var data in db.Reclamation_TechnicalAdviceProtocol.ToList())
            {

            }
        }
    }
}