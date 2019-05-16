using System;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class CreateReclamation
    {
        Wiki.Reclamation reclamation;
        PortalKATEKEntities db = new PortalKATEKEntities();

        public Wiki.Reclamation Reclamation { get => reclamation; set => reclamation = value; }

        public CreateReclamation(Wiki.Reclamation reclamation, string login)
        {
            this.reclamation = reclamation;
            AspNetUsers aspNetUsers = db.AspNetUsers.First(d => d.Email == login);
            this.reclamation.id_AspNetUsersCreate = aspNetUsers.Id;
            this.reclamation.id_DevisionCreate = aspNetUsers.Devision.Value;
            if (reclamation.id_DevisionCreate != 6)
            {
                this.reclamation.closeDevision = false;
                this.reclamation.close = true;
            }
            else if (reclamation.close == true)
                this.reclamation.closeDevision = true;
            GetCorrectFieldReclamation();
            if (reclamation.id_AspNetUsersError != null)
            {
                this.reclamation.id_DevisionReclamation = db.AspNetUsers.Find(reclamation.id_AspNetUsersError).Devision.Value;
                this.reclamation.close = true;
                this.reclamation.closeDevision = true;
            }
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
            return true;
        }
    }
}