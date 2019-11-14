using System;
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
        string close;
        string text;
        string description;
        string answers;
        string devision;
        string userCreate;
        string userReclamation;
        string leavelReclamation;
        string lastLeavelReclamation;
        string onePZName;
        string pfName;

        public string EditLinkJS { get => editLinkJS; set => editLinkJS = value; }
        public string ViewLinkJS { get => viewLinkJS; set => viewLinkJS = value; }
        public string PlanZakaz { get => planZakaz; set => planZakaz = value; }
        public int Id_Reclamation { get => id_Reclamation; set => id_Reclamation = value; }
        public string Devision { get => devision; set => devision = value; }
        public string Text { get => text; set => text = value; }
        public string Description { get => description; set => description = value; }
        public string Answers { get => answers; set => answers = value; }
        public string Close { get => close; set => close = value; }
        public string UserCreate { get => userCreate; set => userCreate = value; }
        public string UserReclamation { get => userReclamation; set => userReclamation = value; }
        public string LeavelReclamation { get => leavelReclamation; set => leavelReclamation = value; }
        public string LastLeavelReclamation { get => lastLeavelReclamation; set => lastLeavelReclamation = value; }
        public string OnePZName { get => onePZName; set => onePZName = value; }
        public string PfName { get => pfName; set => pfName = value; }

        public ReclamationViwers(Wiki.Reclamation reclamation)
        {
            this.onePZName = "";
            editLinkJS = "";
            id_Reclamation = reclamation.id;
            planZakaz = "";
            var pzList = reclamation.Reclamation_PZ.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();
            for (int i = 0; i < pzList.Count; i++)
            {
                planZakaz += pzList[i].PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            if (reclamation.close == true)
            {
                close = "активная";
                viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
            }
            else
            {
                viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt colorWhite" + '\u0022' + "></span></a></td>";
                close = "закрытая";
            }
            text = reclamation.text;
            description = reclamation.description;
            answers = "";
            var answerList = reclamation.Reclamation_Answer.OrderByDescending(d => d.dateTimeCreate).ToList();
            for (int i = 0; i < answerList.Count; i++)
            {
                answers += answerList[i].AspNetUsers.CiliricalName + " : " + answerList[i].answer + "<br>";
            }
            devision = reclamation.Devision.name;
            userCreate = reclamation.AspNetUsers.CiliricalName;
            try
            {
                userReclamation = reclamation.AspNetUsers1.CiliricalName;
            }
            catch
            {
                userReclamation = "";
            }
            leavelReclamation = reclamation.Reclamation_CountError.name;
            lastLeavelReclamation = reclamation.Reclamation_CountError1.name;
            pfName = reclamation.PF.name;
        }

        public ReclamationViwers(Wiki.Reclamation reclamation, int id_Devision)
        {
            this.onePZName = "";
            id_Reclamation = reclamation.id;
            planZakaz = "";
            var pzList = reclamation.Reclamation_PZ.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();
            for (int i = 0; i < pzList.Count; i++)
            {
                planZakaz += pzList[i].PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            if (reclamation.close == true)
            {
                viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
                editLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamation('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
                close = "активная";
            }
            else
            {
                viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt  colorWhite" + '\u0022' + "></span></a></td>";
                editLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamation('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil  colorWhite" + '\u0022' + "></span></a></td>";
                close = "закрытая";
            }
            if (id_Devision == 6 && reclamation.Reclamation_PZ.Max(d => d.PZ_PlanZakaz.dataOtgruzkiBP) < DateTime.Now.AddDays(-15))
                editLinkJS = "";
            text = reclamation.text;
            description = reclamation.description;
            answers = "";
            foreach (var data in reclamation.Reclamation_Answer.OrderByDescending(d => d.dateTimeCreate))
            {
                answers += data.AspNetUsers.CiliricalName + " : " + data.answer + "<br>";
            }
            devision = reclamation.Devision.name;
            userCreate = reclamation.AspNetUsers.CiliricalName;
            try
            {
                userReclamation = reclamation.AspNetUsers1.CiliricalName;
            }
            catch
            {
                userReclamation = "";
            }
            leavelReclamation = reclamation.Reclamation_CountError.name;
            lastLeavelReclamation = reclamation.Reclamation_CountError1.name;
            pfName = reclamation.PF.name;
        }

        public ReclamationViwers(Wiki.Reclamation reclamation, string onePZName) 
        {
            this.onePZName = onePZName;
            editLinkJS = "";
            id_Reclamation = reclamation.id;
            planZakaz = "";
            var pzList = reclamation.Reclamation_PZ.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();
            for(int i = 0; i < pzList.Count; i++)
            {
                planZakaz += pzList[i].PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            if(reclamation.close == true)
            {
                close = "активная";
                viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
            }
            else
            {
                viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt colorWhite" + '\u0022' + "></span></a></td>";
                close = "закрытая";
            }
            text = reclamation.text;
            description = reclamation.description;
            answers = "";
            var answerList = reclamation.Reclamation_Answer.OrderByDescending(d => d.dateTimeCreate).ToList();
            for (int i = 0; i < answerList.Count; i++)
            {
                answers += answerList[i].AspNetUsers.CiliricalName + " : " + answerList[i].answer + "<br>";
            }
            devision = reclamation.Devision.name;
            userCreate = reclamation.AspNetUsers.CiliricalName;
            try
            {
                userReclamation = reclamation.AspNetUsers1.CiliricalName;
            }
            catch
            {
                userReclamation = "";
            }
            leavelReclamation = reclamation.Reclamation_CountError.name;
            lastLeavelReclamation = reclamation.Reclamation_CountError1.name;
            pfName = reclamation.PF.name;
        }

        public ReclamationViwers(Wiki.Reclamation reclamation, int id_Devision, string onePZName) 
        {
            this.onePZName = onePZName;
            id_Reclamation = reclamation.id;
            planZakaz = "";
            var pzList = reclamation.Reclamation_PZ.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList();
            for (int i = 0; i < pzList.Count; i++)
            {
                planZakaz += pzList[i].PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            if (reclamation.close == true)
            {
                viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
                editLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamation('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
                close = "активная";
            }
            else
            {
                viewLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationView('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt  colorWhite" + '\u0022' + "></span></a></td>";
                editLinkJS = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamation('" + reclamation.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil  colorWhite" + '\u0022' + "></span></a></td>";
                close = "закрытая";
            }
            if (id_Devision == 6 && reclamation.Reclamation_PZ.Max(d => d.PZ_PlanZakaz.dataOtgruzkiBP) < DateTime.Now.AddDays(-15))
                editLinkJS = "";
            text = reclamation.text;
            description = reclamation.description;
            answers = "";
            foreach (var data in reclamation.Reclamation_Answer.OrderByDescending(d => d.dateTimeCreate))
            {
                answers += data.AspNetUsers.CiliricalName + " : " + data.answer + "<br>";
            }
            devision = reclamation.Devision.name;
            userCreate = reclamation.AspNetUsers.CiliricalName;
            try
            {
                userReclamation = reclamation.AspNetUsers1.CiliricalName;
            }
            catch
            {
                userReclamation = "";
            }
            leavelReclamation = reclamation.Reclamation_CountError.name;
            lastLeavelReclamation = reclamation.Reclamation_CountError1.name;
            pfName = reclamation.PF.name;
        }
    }
}