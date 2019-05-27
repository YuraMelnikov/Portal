﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public struct ReclamationViwers
    {
        PortalKATEKEntities db;
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

        public ReclamationViwers(Wiki.Reclamation reclamation) : this()
        {
            db = new PortalKATEKEntities();
            viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
            editLinkJS = "";
            GetReclamationData(reclamation);
            GetLeavelReclamation(reclamation);
        }

        public ReclamationViwers(Wiki.Reclamation reclamation, int id_Devision) : this()
        {
            db = new PortalKATEKEntities();
            viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
            editLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamation('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            if (id_Devision == 6 && reclamation.Reclamation_PZ.Max(d => d.PZ_PlanZakaz.dataOtgruzkiBP) < DateTime.Now.AddDays(-15))
                editLinkJS = "";
            GetReclamationData(reclamation);
            GetLeavelReclamation(reclamation);
            GetLastLeavelReclamation(reclamation);
        }

        string GetLeavelReclamation(Wiki.Reclamation reclamation)
        {
            string level = "";
            if(reclamation.id_DevisionReclamation == 3 || reclamation.id_DevisionReclamation == 15 || reclamation.id_DevisionReclamation == 16)
            {
                level = reclamation.Reclamation_CountError.name;
            }
            return level;
        }

        string GetLastLeavelReclamation(Wiki.Reclamation reclamation)
        {
            string level = "";
            if (reclamation.id_DevisionReclamation == 3 || reclamation.id_DevisionReclamation == 15 || reclamation.id_DevisionReclamation == 16)
            {
                level = reclamation.Reclamation_CountError1.name;
            }
            return level;
        }

        bool GetReclamationData(Wiki.Reclamation reclamation)
        {
            this.id_Reclamation = reclamation.id;
            this.planZakaz = GetPlanZakazName(reclamation.Reclamation_PZ.ToList());
            this.type = reclamation.Reclamation_Type.name;
            this.close = GetClose(reclamation.close);
            this.text = reclamation.text;
            this.description = reclamation.description;
            this.timeToSearch = (float)reclamation.timeToSearch;
            this.timeToEliminate = (float)reclamation.timeToEliminate;
            this.answers = GetAnswer(GetAnswerList(reclamation.id));
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
            return true;
        }

        string GetPlanZakazName(List<Reclamation_PZ> reclamation_PZs)
        {
            string pzNames = "";
            int count = reclamation_PZs.Count;
            if (count > 0)
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
            int count = reclamations.Count;
            if (count > 0)
            {
                foreach (var data in reclamations.OrderByDescending(d => d.dateTimeCreate))
                {
                    answers += data.AspNetUsers.CiliricalName + " : " + data.answer + "<br>";
                }
            }
            return answers;
        }

        List<Reclamation_Answer> GetAnswerList(int id_Reclamation)
        {
            return db.Reclamation_Answer
                            .Where(d => d.id_Reclamation == id_Reclamation)
                            .ToList();
        }
    }
}