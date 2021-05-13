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
            this.reclamation.text = reclamation.text.Replace("!", "");
            if (login == "korhov@katek.by")
                this.reclamation.text = this.reclamation.text.ToLower();
            AspNetUsers aspNetUsers = db.AspNetUsers.First(d => d.Email == login);
            this.reclamation.id_AspNetUsersCreate = aspNetUsers.Id;
            this.reclamation.id_DevisionCreate = aspNetUsers.Devision.Value;
            CorrectAddCloseReclamation();
            GetCorrectFieldReclamation();
            int count = db.Reclamation_Answer.Count(a => a.id_Reclamation == reclamation.id);
            GetCloseMKO(login, false, reclamation.id_DevisionReclamation, count);
        }

        public CreateReclamation(Wiki.Reclamation reclamation, string login, bool? reload, int? reloadDevision)
        {
            this.reclamation = reclamation;
            if(this.reclamation.id_AspNetUsersError == null)
                this.reclamation.id_AspNetUsersError = db.Reclamation.Find(reclamation.id).id_AspNetUsersError;
            if (this.reclamation.closeMKO == false)
                this.reclamation.closeMKO = db.Reclamation.Find(reclamation.id).closeMKO;
            if (this.reclamation.closeKO == false)
                this.reclamation.closeKO = db.Reclamation.Find(reclamation.id).closeKO;
            AspNetUsers aspNetUsers = db.AspNetUsers.First(d => d.Email == login);
            GetCorrectFieldReclamation();
            CorrectCloseReclamation();
            ReloadReclamation(reload, reloadDevision);
            this.reclamation.dateTimeCreate = GetDatetimeCreate(reclamation.id);
            this.reclamation.id_AspNetUsersCreate = GetUserCreate(reclamation.id);
            int count = db.Reclamation_Answer.Count(a => a.id_Reclamation == reclamation.id);
            GetCloseMKO(login, reload.Value, reclamation.id_DevisionReclamation, count);
        }

        bool GetCorrectFieldReclamation()
        {
            if (reclamation.id_ReclamationTypeKB == 0)
                reclamation.id_ReclamationTypeKB = 20;
            if (reclamation.editManufacturing == false)
            {
                reclamation.editManufacturingIdDevision = null;
            }
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
            if (reclamation.id_Reclamation_CountErrorFinal == 0)
                reclamation.id_Reclamation_CountErrorFinal = 1;
            return true;
        }

        bool ReloadReclamation(bool? reload, int? reloadDevision)
        {
            if (reloadDevision == 26)
                reloadDevision = 13;
            if (reload == true)
            {
                reclamation.id_DevisionReclamation = reloadDevision.Value;
                reclamation.id_AspNetUsersError = null;
                reclamation.closeDevision = false;
                reclamation.id_Reclamation_CountErrorFirst = 1;
                reclamation.closeMKO = false;
                reclamation.closeKO = false;
            }
            return true;
        }

        bool CorrectAddCloseReclamation()
        {
            if (reclamation.id_ReclamationTypeKB == 0)
                reclamation.id_ReclamationTypeKB = 26;
            if (reclamation.id_DevisionCreate != 6)
            {
                reclamation.closeDevision = false;
                reclamation.close = true;
            }
            if (reclamation.id_DevisionReclamation == 7)
            {
                reclamation.closeDevision = true;
            }
            if (reclamation.id_DevisionReclamation == 26)
            {
                reclamation.id_DevisionReclamation = 13;
            }

            if (reclamation.id_AspNetUsersError != null)
            {
                reclamation.id_DevisionReclamation = db.AspNetUsers.Find(reclamation.id_AspNetUsersError).Devision.Value;
                reclamation.closeDevision = true;
            }
            return true;
        }

        bool CorrectCloseReclamation()
        {
            if (reclamation.id_AspNetUsersError != null)
            {
                reclamation.id_DevisionReclamation = db.AspNetUsers.Find(reclamation.id_AspNetUsersError).Devision.Value;
                reclamation.closeDevision = true;
            }
            if (reclamation.id_DevisionReclamation == 7)
            {
                reclamation.closeDevision = true;
            }
            return true;
        }

        DateTime GetDatetimeCreate(int id_reclamation)
        {
            return db.Reclamation.Find(id_reclamation).dateTimeCreate;
        }

        string GetUserCreate(int id_reclamation)
        {
            return db.Reclamation.Find(id_reclamation).id_AspNetUsersCreate;
        }

        bool GetCloseMKO(string login, bool reload, int devision, int queAnswers)
        {
            int devisionUser= db.AspNetUsers.First(a => a.Email == login).Devision.Value;
            if (login == "fvs@katek.by" || login == "nrf@katek.by" || login == "vi@katek.by")
            {
                if (devisionUser != devision)
                {
                    
                }
                else
                {
                    if (queAnswers > 0)
                    {
                        reclamation.closeMKO = true;
                        reclamation.closeDevision = true;
                    }
                }
            }
            if (login == "nrf@katek.by")
            {
                reclamation.closeKO = true;
            }
            if (reload == true)
            {
                reclamation.closeKO = false;
                reclamation.closeMKO = false;
                reclamation.closeDevision = false;
            }
            return true;
        }
    }
}