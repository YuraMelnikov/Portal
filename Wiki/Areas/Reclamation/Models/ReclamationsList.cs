using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void GetReclamation()
        {
            Initialization();
            Reclamations = db.Reclamation
                .Where(d => d.editManufacturing == true)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
        }

        public void GetReclamation(string login)
        {
            string id_User = db.AspNetUsers.First(d => d.Email == login).Id;
            Initialization();
            Reclamations = db.Reclamation.Where(d => d.id_AspNetUsersCreate == id_User)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
        }

        public void GetReclamation(int id_Devision)
        {
            Initialization();
            if (id_Devision == 6)
                Reclamations = db.Reclamation.Where(d => d.id_DevisionCreate == id_Devision)
                    .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            else
                Reclamations = db.Reclamation.Where(d => d.id_DevisionReclamation == id_Devision)
                    .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
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
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else if (id_Devision == 3 || id_Devision == 16)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.closeDevision == active)
                    .Where(d => d.id_DevisionReclamation == 3 || d.id_DevisionReclamation == 16)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else if (id_Devision == 0)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.close == active)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else
            {
                Reclamations = db.Reclamation
                    .Where(d => d.closeDevision == active)
                    .Where(d => d.id_DevisionReclamation == id_Devision)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
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
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else
            {
                Reclamations = db.Reclamation
                    .Where(d => d.closeMKO == active)
                    .Where(d => d.id_DevisionReclamation == 3 || d.id_DevisionReclamation == 16)
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
        }

        public void GetReclamationPlanZakaz(int id_PZ_PlanZakaz)
        {
            Initialization();
            Reclamations = db.Reclamation
                .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
        }

        public void GetReclamationPlanZakaz(int id_Devision, int id_PZ_PlanZakaz)
        {
            Initialization();
            if (id_Devision == 6)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.id_DevisionCreate == 6)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else if (id_Devision == 3 || id_Devision == 16)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.id_DevisionReclamation == 3 || d.id_DevisionReclamation == 16)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else if (id_Devision == 0)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.id_DevisionReclamation == id_Devision)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
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
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else if (id_Devision == 3 || id_Devision == 16)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.close == active)
                    .Where(d => d.id_DevisionReclamation == 3 || d.id_DevisionReclamation == 16)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else if (id_Devision == 0)
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.close == active)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
            else
            {
                Reclamations = db.Reclamation
                    .Where(d => d.Reclamation_PZ.Where(c => c.id_PZ_PlanZakaz == id_PZ_PlanZakaz).Count() > 0)
                    .Where(d => d.Reclamation_PZ.Max(c => c.PZ_PlanZakaz.dataOtgruzkiBP) > dateDeactiveOTK)
                    .Where(d => d.close == active)
                    .Where(d => d.id_DevisionReclamation == id_Devision)
                .Include(d => d.Reclamation_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.Reclamation_Answer.Select(s => s.AspNetUsers))
                .Include(d => d.Devision)
                .Include(d => d.AspNetUsers)
                .Include(d => d.AspNetUsers1)
                .Include(d => d.Reclamation_CountError)
                .Include(d => d.Reclamation_CountError1)
                .ToList();
            }
        }

        void Initialization()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            Reclamations = new List<Wiki.Reclamation>();
        }
    }
}