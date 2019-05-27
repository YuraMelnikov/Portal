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
        string userCreate;
        string userReclamation;
        string leavelReclamation;
        string lastLeavelReclamation;

        public string EditLinkJS { get => editLinkJS; set => editLinkJS = value; }
        public string ViewLinkJS { get => viewLinkJS; set => viewLinkJS = value; }
        public string PlanZakaz { get => planZakaz; set => planZakaz = value; }
        public int Id_Reclamation { get => id_Reclamation; set => id_Reclamation = value; }
        public DateTime DateCreate { get => dateCreate; set => dateCreate = value; }
        public string Devision { get => devision; set => devision = value; }
        public string Type { get => type; set => type = value; }
        public string Text { get => text; set => text = value; }
        public string Description { get => description; set => description = value; }
        public string Answers { get => answers; set => answers = value; }
        public float TimeToEliminate { get => timeToEliminate; set => timeToEliminate = value; }
        public float TimeToSearch { get => timeToSearch; set => timeToSearch = value; }
        public string Close { get => close; set => close = value; }
        public string UserCreate { get => userCreate; set => userCreate = value; }
        public string UserReclamation { get => userReclamation; set => userReclamation = value; }
        public string LeavelReclamation { get => leavelReclamation; set => leavelReclamation = value; }
        public string LastLeavelReclamation { get => lastLeavelReclamation; set => lastLeavelReclamation = value; }

        public ReclamationViwers(Wiki.Reclamation reclamation) 
        {
            viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
            editLinkJS = "";
            this.id_Reclamation = reclamation.id;
            planZakaz = "";
            foreach (var data in reclamation.Reclamation_PZ.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz))
            {
                planZakaz += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            this.type = reclamation.Reclamation_Type.name;
            if(reclamation.close == true)
                this.close = "активная";
            else
                this.close = "закрытая";
            this.text = reclamation.text;
            this.description = reclamation.description;
            this.timeToSearch = (float)reclamation.timeToSearch;
            this.timeToEliminate = (float)reclamation.timeToEliminate;
            answers = "";
            foreach (var data in reclamation.Reclamation_Answer.OrderByDescending(d => d.dateTimeCreate))
            {
                answers += data.AspNetUsers.CiliricalName + " : " + data.answer + "<br>";
            }
            this.devision = reclamation.Devision.name;
            this.dateCreate = reclamation.dateTimeCreate;
            this.userCreate = reclamation.AspNetUsers.CiliricalName;
            try
            {
                this.userReclamation = reclamation.AspNetUsers1.CiliricalName;
            }
            catch
            {
                this.userReclamation = "";
            }
        }

        public ReclamationViwers(Wiki.Reclamation reclamation, int id_Devision) 
        {
            viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
            if (id_Devision == 6 && reclamation.Reclamation_PZ.Max(d => d.PZ_PlanZakaz.dataOtgruzkiBP) < DateTime.Now.AddDays(-15))
                editLinkJS = "";
            else
                editLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamation('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            this.id_Reclamation = reclamation.id;
            planZakaz = "";
            foreach (var data in reclamation.Reclamation_PZ.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz))
            {
                planZakaz += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            this.type = reclamation.Reclamation_Type.name;
            if (reclamation.close == true)
                this.close = "активная";
            else
                this.close = "закрытая";
            this.text = reclamation.text;
            this.description = reclamation.description;
            this.timeToSearch = (float)reclamation.timeToSearch;
            this.timeToEliminate = (float)reclamation.timeToEliminate;
            answers = "";
            foreach (var data in reclamation.Reclamation_Answer.OrderByDescending(d => d.dateTimeCreate))
            {
                answers += data.AspNetUsers.CiliricalName + " : " + data.answer + "<br>";
            }
            this.devision = reclamation.Devision.name;
            this.dateCreate = reclamation.dateTimeCreate;
            this.userCreate = reclamation.AspNetUsers.CiliricalName;
            try
            {
                this.userReclamation = reclamation.AspNetUsers1.CiliricalName;
            }
            catch
            {
                this.userReclamation = "";
            }
        }
    }
}