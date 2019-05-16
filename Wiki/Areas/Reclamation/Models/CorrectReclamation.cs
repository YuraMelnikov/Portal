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
            AspNetUsers aspNetUsers = db.AspNetUsers.First(d => d.Email == login);
            this.reclamation.id_AspNetUsersCreate = aspNetUsers.Id;
            this.reclamation.id_DevisionCreate = aspNetUsers.Devision.Value;
            if (reclamation.id_AspNetUsersError != null)
                this.reclamation.id_DevisionReclamation = db.AspNetUsers.Find(reclamation.id_AspNetUsersError).Devision.Value;
            GetCorrectFieldReclamation();
        }

        bool GetCorrectFieldReclamation()
        {
            if (reclamation.editManufacturingIdDevision == 0)
                reclamation.editManufacturingIdDevision = null;
            if (reclamation.dateTimeCreate == null)
                reclamation.dateTimeCreate = DateTime.Now;
            if (reclamation.text == null)
                reclamation.text = "";
            if (reclamation.description == null)
                reclamation.description = "";
            if (reclamation.PCAM == null)
                reclamation.PCAM = "";
            if (reclamation.id_Reclamation_CountErrorFirst == 0)
                reclamation.id_Reclamation_CountErrorFirst = 1;
            if (reclamation.id_Reclamation_CountErrorFinal == 0)
                reclamation.id_Reclamation_CountErrorFinal = 1;
            if (reclamation.close == true)
                reclamation.closeDevision = true;
            return true;
        }
    }
}