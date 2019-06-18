using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.ServiceReclamations.Models
{
    public class ReclamationsList
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        public List<ServiceRemarks> GetActive()
        {
            return db.ServiceRemarks.Where(d => d.dateClose == null).ToList();
        }

        public List<ServiceRemarks> GetClose()
        {
            return db.ServiceRemarks.Where(d => d.dateClose != null).ToList();
        }

        public List<ServiceRemarks> GetAll()
        {
            return db.ServiceRemarks.ToList();
        }
    }
}