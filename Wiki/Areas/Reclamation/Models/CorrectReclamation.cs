using System;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class CorrectReclamation
    {
        Wiki.Reclamation reclamation;
        PortalKATEKEntities db = new PortalKATEKEntities();

        public Wiki.Reclamation Reclamation { get => reclamation; set => reclamation = value; }

        public CorrectReclamation(Wiki.Reclamation reclamation)
        {
            this.reclamation = reclamation;
            GetCorrectFieldReclamation();
        }

        public CorrectReclamation(Wiki.Reclamation reclamation, string login)
        {
            this.reclamation = reclamation;
            this.reclamation.AspNetUsers = db.AspNetUsers.First(d => d.Email == login);
            GetCorrectFieldReclamation();
        }

        bool GetCorrectFieldReclamation()
        {
            if (reclamation.dateTimeCreate == null)
                reclamation.dateTimeCreate = DateTime.Now;
            if (reclamation.text == null)
                reclamation.text = "";
            if (reclamation.description == null)
                reclamation.description = "";
            if (reclamation.PCAM == null)
                reclamation.PCAM = "";
            return true;
        }
    }
}