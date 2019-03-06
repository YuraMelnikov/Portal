using System.Linq;
using System.Net.Mail;

namespace Wiki.Models
{
    public class EmailRKD : EmailClient
    {
        public EmailRKD(RKD_Order rkdOrder, string recipient)
        {
            this.rkdOrder = rkdOrder;
            Recipient = recipient;
        }

        protected string subject;
        protected string body;
        protected RKD_Order rkdOrder;

        protected string Recipient { get; set; }
        
        protected string GetNumberPlanZakaz()
        {
            return "Заказ №: " + db.PZ_PlanZakaz.Find(rkdOrder.id_PZ_PlanZakaz).PlanZakaz.ToString();
        }

        protected void SendEmail()
        {
            if (Recipient != null)
            {
                mail.From = new MailAddress(Recipient);
            }
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
            mail.Dispose();
        }

        protected void SetGipMailAddress()
        {
            foreach (var VARIABLE in db.RKD_GIP.Where(d => d.id_RKD_Order == rkdOrder.id).ToList())
            {
                if(VARIABLE.AspNetUsers.Email != "katekproject@gmail.com" || VARIABLE.AspNetUsers.Email != "melnikauyi@gmail.com")
                    mail.To.Add(new MailAddress(VARIABLE.AspNetUsers.Email));
            }
        }
    }
}