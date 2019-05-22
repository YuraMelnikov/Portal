using System.Collections.Generic;
using System.Linq;

namespace Wiki.Areas.Reclamation.Models
{
    public class TARemarkView
    {
        int id_Reclamation;
        string linkToEdit;
        string linkToView;
        string orders;
        string textReclamation;
        string descriptionReclamation;
        string answers;
        string decision;
        string userToTA;
        string userCreate;
        string devisionReclamation;
        string leavelReclamation;
        string lastLeavelReclamation;
        string firstPartLinkToEdit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getTAEdit('";
        string secondPartLinkToEdit = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
        string firstPartLinkToView = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getTAView('";
        string secondPartLinkToView = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>";
        string firstPartLinkToEditCounter = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetReclamationExpert('";

        public int Id_Reclamation { get => id_Reclamation; set => id_Reclamation = value; }
        public string LinkToEdit { get => linkToEdit; set => linkToEdit = value; }
        public string LinkToView { get => linkToView; set => linkToView = value; }
        public string Orders { get => orders; set => orders = value; }
        public string TextReclamation { get => textReclamation; set => textReclamation = value; }
        public string DescriptionReclamation { get => descriptionReclamation; set => descriptionReclamation = value; }
        public string Answers { get => answers; set => answers = value; }
        public string Decision { get => decision; set => decision = value; }
        public string UserToTA { get => userToTA; set => userToTA = value; }
        public string UserCreate { get => userCreate; set => userCreate = value; }
        public string DevisionReclamation { get => devisionReclamation; set => devisionReclamation = value; }
        public string LeavelReclamation { get => leavelReclamation; set => leavelReclamation = value; }
        public string LastLeavelReclamation { get => lastLeavelReclamation; set => lastLeavelReclamation = value; }

        public TARemarkView(Reclamation_TechnicalAdvice reclamation)
        {
            id_Reclamation = reclamation.Reclamation.id;
            linkToEdit = firstPartLinkToEdit + reclamation.id + secondPartLinkToEdit;
            linkToView = firstPartLinkToView + reclamation.id + secondPartLinkToView;
            orders = GetOrders(reclamation.Reclamation.Reclamation_PZ.ToList());
            textReclamation = reclamation.Reclamation.text;
            descriptionReclamation = reclamation.Reclamation.description;
            answers = GetAnswers(reclamation.Reclamation.Reclamation_Answer.OrderByDescending(d => d.dateTimeCreate).ToList());
            decision = reclamation.text;
            userToTA = reclamation.AspNetUsers1.CiliricalName;
            userCreate = reclamation.AspNetUsers.CiliricalName;
            devisionReclamation = reclamation.Reclamation.Devision.name;
            leavelReclamation = reclamation.Reclamation.Reclamation_CountError.name;
            lastLeavelReclamation = reclamation.Reclamation.Reclamation_CountError1.name;
        }

        public TARemarkView(Wiki.Reclamation reclamation)
        {
            id_Reclamation = reclamation.id;
            linkToEdit = firstPartLinkToEditCounter + reclamation.id + secondPartLinkToEdit;
            linkToView = "";
            orders = GetOrders(reclamation.Reclamation_PZ.ToList());
            textReclamation = reclamation.text;
            descriptionReclamation = reclamation.description;
            answers = GetAnswers(reclamation.Reclamation_Answer.OrderByDescending(d => d.dateTimeCreate).ToList());
            decision = reclamation.text;
            userToTA = "";
            userCreate = reclamation.AspNetUsers.CiliricalName;
            devisionReclamation = reclamation.Devision.name;
            leavelReclamation = reclamation.Reclamation_CountError.name;
            lastLeavelReclamation = reclamation.Reclamation_CountError1.name;
        }

        string GetOrders(List<Reclamation_PZ> reclamation)
        {
            string ordersName = "";
            foreach (var data in reclamation)
            {
                ordersName += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            return ordersName;
        }

        string GetAnswers(List<Reclamation_Answer> answer)
        {
            string answers = "";
            foreach (var data in answer)
            {
                answers += data.dateTimeCreate.ToString().Substring(0, 10) + " | " + data.answer + " | " + data.AspNetUsers.CiliricalName;
            }
            return answers;
        }
    }
}