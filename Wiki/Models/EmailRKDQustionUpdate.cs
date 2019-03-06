using System.Linq;

namespace Wiki.Models
{
    public class EmailRKDQustionUpdate : EmailRKDQuestion
    {
        public EmailRKDQustionUpdate(RKD_Order rkdOrder, string recipient, int id_RKD_Question)
            : base(rkdOrder, recipient)
        {
            this.id_RKD_Question = id_RKD_Question;
        }

        private readonly int id_RKD_Question;

        protected override void GetBody(string description)
        {
            body += "Вопрос по РКД: " + rkdOrder.PZ_PlanZakaz.PlanZakaz + "<br/>";
            body += "Текст вопроса: " + db.RKD_Question.Find(id_RKD_Question).textQuestion + "<br/>";
            body += "История переписки" + "<br/>" + GetAllTextCorrespondence();
        }

        private string GetAllTextCorrespondence()
        {
            string historyMail = "";
            try
            {
                foreach (var data in db.RKD_QuestionData.Where(d => d.id_RKD_Question == id_RKD_Question)
                    .OrderByDescending(d => d.dateUpload))
                {
                    historyMail += data.dateUpload.ToString() + " | " + data.textData + " (" +
                                   db.AspNetUsers.Find(data.userUpload).CiliricalName + ")" + "<br/>";
                }
                return historyMail;
            }
            catch
            {
                return historyMail;
            }
        }
    }
}