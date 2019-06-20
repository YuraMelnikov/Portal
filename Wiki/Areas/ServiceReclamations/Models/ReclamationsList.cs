using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Wiki.Areas.ServiceReclamations.Models
{
    public class ReclamationsList
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        public List<ServiceRemarks> GetActive()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            return db.ServiceRemarks
                .AsNoTracking()
                .Include(d => d.ServiceRemarksPlanZakazs)
                .Include(d => d.ServiceRemarksTypes.Select(s => s.Reclamation_Type))
                .Include(d => d.ServiceRemarksCauses.Select(s => s.ServiceRemarksCause))
                .Where(d => d.dateClose == null)
                .ToList();
        }

        public List<ServiceRemarks> GetClose()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            return db.ServiceRemarks
                .AsNoTracking()
                .Include(d => d.ServiceRemarksPlanZakazs)
                .Include(d => d.ServiceRemarksTypes.Select(s => s.Reclamation_Type))
                .Include(d => d.ServiceRemarksCauses.Select(s => s.ServiceRemarksCause))
                .Where(d => d.dateClose != null)
                .ToList();
        }

        public List<ServiceRemarks> GetAll()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            return db.ServiceRemarks
                .AsNoTracking()
                .Include(d => d.ServiceRemarksPlanZakazs)
                .Include(d => d.ServiceRemarksTypes.Select(s => s.Reclamation_Type))
                .Include(d => d.ServiceRemarksCauses.Select(s => s.ServiceRemarksCause))
                .ToList();
        }
    }
}