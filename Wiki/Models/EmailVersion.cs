using System.Net.Mail;

namespace Wiki.Models
{
    public class EmailVersion : EmailRKD
    {
        public EmailVersion(RKD_Order rkdOrder, string recipient) 
            : base(rkdOrder, recipient)
        {

        }

        public virtual void CreateAndSendMail(string description)
        {
            GetSubject();
            GetBody(description);
            SetMailAddress();
            SetGipMailAddress();
            SendEmail();
        }

        protected virtual void GetSubject()
        {
            subject += GetNumberPlanZakaz() + " - ";
            subject += db.TypeRKD_Mail_Version.Find(1).name.ToString();
        }

        protected virtual void GetBody(string description)
        {
            body += subject + "<br/>";
            if (description != "" || description != null)
                body += @"<a href=" + description + ">" + description + @"</a>" + "<br/>";
        }

        protected virtual void SetMailAddress()
        {
            mail.To.Add(new MailAddress("vi@katek.by"));
            mail.To.Add(new MailAddress("goa@katek.by"));
            mail.To.Add(new MailAddress("yaa@katek.by"));
            mail.To.Add(new MailAddress("maj@katek.by"));
            mail.To.Add(new MailAddress("nrf@katek.by"));
            mail.To.Add(new MailAddress("Kuchynski@katek.by"));
            mail.To.Add(new MailAddress("fvs@katek.by"));
            mail.To.Add(new MailAddress("myi@katek.by"));
            mail.To.Add(new MailAddress("bav@katek.by"));
            mail.To.Add(new MailAddress("gea@katek.by"));
        }
    }
}