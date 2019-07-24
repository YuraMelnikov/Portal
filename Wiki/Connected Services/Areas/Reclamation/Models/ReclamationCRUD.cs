using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class ReclamationCRUD
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        Wiki.Reclamation reclamation;
        string history;
        string answer;
        int userError;
        bool trash;
        int[] pZ_PlanZakaz;

        public ReclamationCRUD()
        {
            Reclamation = new Wiki.Reclamation();
            History = "";
        }

        public ReclamationCRUD(int id_Reclamation)
        {
            Reclamation = db.Reclamation.Find(id_Reclamation);
            GetHistory(id_Reclamation);
        }

        public Wiki.Reclamation Reclamation { get => reclamation; set => reclamation = value; }
        public string History { get => history; set => history = value; }
        public string Answer { get => answer; set => answer = value; }
        public int UserError { get => userError; set => userError = value; }
        public bool Trash { get => trash; set => trash = value; }
        public int[] PZ_PlanZakaz { get => pZ_PlanZakaz; set => pZ_PlanZakaz = value; }

        bool GetHistory(int id_Reclamation)
        {
            History = "";
            var listAnswer = db.Reclamation_Answer.Where(d => d.id_Reclamation == id_Reclamation).OrderByDescending(d => d.dateTimeCreate).ToList();
            foreach (var data in listAnswer)
            {
                History += data.AspNetUsers.CiliricalName + " | " + data.answer + "</br>";
            }
            return true;
        }
    }
}