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
            CorrectCloseReclamation();
            GetCorrectFieldReclamation();
        }

        public CreateReclamation(Wiki.Reclamation reclamation, string login, bool? reload, int? reloadDevision)
        {
            this.reclamation = reclamation;
            AspNetUsers aspNetUsers = db.AspNetUsers.First(d => d.Email == login);
            GetCorrectFieldReclamation();
            CorrectCloseReclamation();
            ReloadReclamation(reload, reloadDevision);

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

        bool ReloadReclamation(bool? reload, int? reloadDevision)
        {
            if (reload == true)
            {
                reclamation.id_DevisionReclamation = reloadDevision.Value;
                reclamation.id_AspNetUsersError = null;
                reclamation.closeDevision = false;
                reclamation.id_Reclamation_CountErrorFinal = 1;
                reclamation.id_Reclamation_CountErrorFirst = 1;
            }
            return true;
        }

        bool CorrectCloseReclamation()
        {
            if (reclamation.id_DevisionCreate != 6)
            {
                reclamation.closeDevision = false;
                reclamation.close = true;
            }
            if (reclamation.id_AspNetUsersError != null)
            {
                reclamation.id_DevisionReclamation = db.AspNetUsers.Find(reclamation.id_AspNetUsersError).Devision.Value;
                reclamation.close = true;
                reclamation.closeDevision = true;
            }
            if (reclamation.id_AspNetUsersError != null)
            {
                reclamation.id_DevisionReclamation = db.AspNetUsers.Find(reclamation.id_AspNetUsersError).Devision.Value;
                reclamation.closeDevision = true;
            }
            else if (reclamation.close == true)
                reclamation.closeDevision = true;
            return true;
        }
    }
}