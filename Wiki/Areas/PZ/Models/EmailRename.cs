using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.PZ.Models
{
    public class EmailRename : EmailClient
    {
        string planZakaz;
        string before;
        string next;
        string recipient;
        string subject;
        string body;
        bool renameTU;

        public EmailRename(string planZakaz, string before, string next, string recipient, bool renameTU)
        {
            this.planZakaz = planZakaz;
            this.recipient = recipient;
            this.renameTU = renameTU;
            this.before = before;
            this.next = next;
        }

        string GetSubject()
        {
            if (renameTU == true)
                return "Изменено наименование по ТУ заказа № " + planZakaz;
            else
                return "Изменено контрактное (договорное) наименование заказа № " + planZakaz;
        }

        string GetBody()
        {
            string textBody = subject + "<br/>" + "<br/>" + ":";
            if (renameTU == true)
            {
                textBody += "Наименование по ТУ до изменения: " + before + "<br/>" + "<br/>" + ";";
                textBody += "Наименование по ТУ после изменения: " + next + "<br/>";

            }
            else
            {
                textBody += "Контрактное (договорное) наименование до изменения: " + before + "<br/>" + "<br/>" + ";";
                textBody += "Контрактное (договорное) наименование после изменения: " + next + "<br/>";
            }
            return textBody;
        }

        void SetMailAddress()
        {
            mail.To.Add(new MailAddress("myi@katek.by"));
            mail.To.Add(new MailAddress("bav@katek.by"));
            foreach (var data in db.AspNetUsers.Where(d => d.Devision == 3).Where(d => d.LockoutEnabled == true))
            {
                mail.To.Add(new MailAddress(data.Email));
            }
            foreach (var data in db.AspNetUsers.Where(d => d.Devision == 15).Where(d => d.LockoutEnabled == true))
            {
                mail.To.Add(new MailAddress(data.Email));
            }
            foreach (var data in db.AspNetUsers.Where(d => d.Devision == 16).Where(d => d.LockoutEnabled == true))
            {
                mail.To.Add(new MailAddress(data.Email));
            }
        }

        public void SendEmail()
        {
            subject = GetSubject();
            body = GetBody() ;
            if (recipient != null)
            {
                mail.From = new MailAddress(recipient);
            }
            SetMailAddress();
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
            mail.Dispose();
        }
    }
}