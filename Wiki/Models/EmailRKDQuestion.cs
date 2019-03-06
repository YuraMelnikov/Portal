using System.Linq;

namespace Wiki.Models
{
    public class EmailRKDQuestion : EmailVersion
    {
        public EmailRKDQuestion(RKD_Order rkdOrder, string recipient)
            : base(rkdOrder, recipient)
        {

        }

        protected override void GetSubject()
        {
            subject += "Вопрос по РКД: " + GetNumberPlanZakaz();
        }

        protected override void GetBody(string description)
        {
            body += "Вопрос по РКД: " + rkdOrder.PZ_PlanZakaz.PlanZakaz + "<br/>";
            body += "Текст вопроса: " + description + "<br/>";
            body += "Сотрудник задавший вопрос: " + db.AspNetUsers.First(d => d.Email == Recipient).CiliricalName + "<br/>";
        }
    }
}