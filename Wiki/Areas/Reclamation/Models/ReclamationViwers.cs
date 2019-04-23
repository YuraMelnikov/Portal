using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class ReclamationViwers
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        int id_Reclamation;
        string planZakaz;
        string type;
        string close;
        string text;
        string description;
        float timeToSearch;
        float timeToEliminate;
        string answers;
        string devision;
        DateTime dateCreate;

        public DateTime DateCreate { get => dateCreate; set => dateCreate = value; }
        public string Devision { get => devision; set => devision = value; }
        public string Answers { get => answers; set => answers = value; }
        public float TimeToEliminate { get => timeToEliminate; set => timeToEliminate = value; }
        public float TimeToSearch { get => timeToSearch; set => timeToSearch = value; }
        public string Description { get => description; set => description = value; }
        public string Text { get => text; set => text = value; }
        public string Close { get => close; set => close = value; }
        public string Type { get => type; set => type = value; }
        public string PlanZakaz { get => planZakaz; set => planZakaz = value; }
        public int Id_Reclamation { get => id_Reclamation; set => id_Reclamation = value; }

        public ReclamationViwers(Wiki.Reclamation reclamation)
        {
            id_Reclamation = reclamation.id;
            planZakaz = GetPlanZakazName(reclamation.Reclamation_PZ.ToList());
            type = reclamation.Reclamation_Type.name;
            close = GetClose(reclamation.close);
            text = reclamation.text;
            description = reclamation.description;
            timeToSearch = (float)reclamation.timeToSearch;
            timeToEliminate = (float)reclamation.timeToEliminate;
            answers = GetAnswer(GetAnswerList(reclamation.id));
            devision = reclamation.Devision.name;
            dateCreate = reclamation.dateTimeCreate;
        }
        
        string GetPlanZakazName(List<Reclamation_PZ> reclamation_PZs)
        {
            string pzNames = "";
            if(reclamation_PZs.Count > 0)
            {
                foreach (var planZakaz in reclamation_PZs)
                {
                    pzNames += planZakaz.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
                }
            }
            return pzNames;
        }

        string GetClose(bool close)
        {
            string closeName = "активная";
            if (close == true)
                closeName = "закрыта";
            return closeName;
        }

        string GetAnswer(List<Reclamation_Answer> reclamations)
        {
            string answers = "";
            if(reclamations.Count > 0)
            {
                foreach (var data in reclamations)
                {
                    answers += data.AspNetUsers.CiliricalName + " : " + data.answer;
                }
            }
            return answers;
        }

        List<Reclamation_Answer> GetAnswerList(int id_Reclamation)
        {
            return db.Reclamation_Answer
                            .Where(d => d.id_Reclamation == id_Reclamation)
                            .OrderByDescending(d => d.dateTimeCreate)
                            .ToList();
        }
    }
}