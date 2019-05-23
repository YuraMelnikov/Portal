using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class ReclamationsList
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        List<Wiki.Reclamation> reclamations;

        DateTime dateDeactiveOTK = DateTime.Now.AddDays(-15);

        public ReclamationsList()
        {
            List<Wiki.Reclamation> reclamations = new List<Wiki.Reclamation>();
        }

        public List<Wiki.Reclamation> Reclamations { get => reclamations; set => reclamations = value; }

        public void GetReclamation(int id_Devision)
        {
            Initialization();
            if (id_Devision == 6)
                Reclamations = db.Reclamation.Where(d => d.id_DevisionCreate == id_Devision).ToList();
            else
                Reclamations = db.Reclamation.Where(d => d.id_DevisionReclamation == id_Devision).ToList();
        }

        public void GetReclamation(int id_Devision, bool active)
        {
            Initialization();
            if (id_Devision == 6)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.close == active)
                    .Where(d => d.id_DevisionCreate == 6)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .ToList();
            }
            else if (id_Devision == 3 || id_Devision == 16)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.closeDevision == active)
                    .Where(d => d.id_DevisionReclamation == 3 || d.id_DevisionReclamation == 16)
                    .ToList();
            }
            else if (id_Devision == 0)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.close == active)
                    .ToList();
            }
            else
            {
                Reclamations = db.Reclamation
                    .Where(d => d.closeDevision == active)
                    .Where(d => d.id_DevisionReclamation == id_Devision)
                    .ToList();
            }
        }

        public void GetReclamation(int id_Devision, bool active, string login)
        {
            Initialization();
            if (login == "nrf@katek.by")
            {
                Reclamations = db.Reclamation
                    .Where(d => d.closeMKO == active)
                    .Where(d => d.id_DevisionReclamation == id_Devision)
                    .ToList();
            }
            else
            {
                Reclamations = db.Reclamation
                    .Where(d => d.closeMKO == active)
                    .Where(d => d.id_DevisionReclamation == 3 || d.id_DevisionReclamation == 16)
                    .ToList();
            }
        }

        public void GetReclamationPlanZakaz(int id_PZ_PlanZakaz)
        {
            Initialization();
            Reclamations = db.Reclamation
                .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                .ToList();
        }

        public void GetReclamationPlanZakaz(int id_Devision, int id_PZ_PlanZakaz)
        {
            Initialization();
            if (id_Devision == 6)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.id_DevisionCreate == 6)
                    .ToList();
            }
            else if (id_Devision == 3 || id_Devision == 16)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.id_DevisionReclamation == 3 || d.id_DevisionReclamation == 16)
                    .ToList();
            }
            else if (id_Devision == 0)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .ToList();
            }
            else
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.id_DevisionReclamation == id_Devision)
                    .ToList();
            }
        }

        public void GetReclamationPlanZakaz(int id_Devision, bool active, int id_PZ_PlanZakaz)
        {
            Initialization();
            if (id_Devision == 6)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.close == active)
                    .Where(d => d.id_DevisionCreate == 6)
                    .ToList();
            }
            else if (id_Devision == 3 || id_Devision == 16)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.close == active)
                    .Where(d => d.id_DevisionReclamation == 3 || d.id_DevisionReclamation == 16)
                    .ToList();
            }
            else if (id_Devision == 0)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.close == active)
                    .ToList();
            }
            else
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.close == active)
                    .Where(d => d.id_DevisionReclamation == id_Devision)
                    .ToList();
            }
        }

        void Initialization()
        {
            Reclamations = new List<Wiki.Reclamation>();
        }
    }
}