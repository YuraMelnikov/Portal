using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class ReclamationsList
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        List<Wiki.Reclamation> reclamations;

        public ReclamationsList()
        {
            List<Wiki.Reclamation> reclamations = new List<Wiki.Reclamation>();
        }

        public List<Wiki.Reclamation> Reclamations { get => reclamations; set => reclamations = value; }

        public void GetActiveReclamationOTK()
        {
            Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ
                    .Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > DateTime.Now.AddDays(-10))
                    .Where(d => d.close == false)
                    .Where(d => d.id_DevisionCreate == 6)
                    .ToList();
        }
    }
}