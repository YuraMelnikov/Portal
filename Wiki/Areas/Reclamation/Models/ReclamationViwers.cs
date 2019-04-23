using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class ReclamationViwers
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        string editLinkJS;
        string viewLinkJS;
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
        public string EditLinkJS { get => editLinkJS; set => editLinkJS = value; }
        public string ViewLinkJS { get => viewLinkJS; set => viewLinkJS = value; }

        public ReclamationViwers(Wiki.Reclamation reclamation)
        {
            viewLinkJS = "";
            editLinkJS = "";
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

        public ReclamationViwers(Wiki.Reclamation reclamation, int id_Devision)
        {
            viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getID('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            editLinkJS = GetEditLink(id_Devision, reclamation.id);
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

        string GetEditLink(int id_Devision, int id_Reclamation)
        {
            string link = "";
            if (id_Devision == 6)
                link += "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getOTKID('";
            if (id_Devision == 3 || id_Devision == 15 || id_Devision == 16)
                link += "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getKOID('";
            if (id_Devision > 0)
                link += "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getStandartID('";
            if (id_Devision != 0)
                link += id_Reclamation + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            return link;
        }
    }
}