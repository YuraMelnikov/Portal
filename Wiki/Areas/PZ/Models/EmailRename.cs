﻿using System.Linq;
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
        double massBefare;
        double massNext;

        public EmailRename(string planZakaz, string before, string next, string recipient, bool renameTU)
        {
            this.planZakaz = planZakaz;
            this.recipient = recipient;
            this.renameTU = renameTU;
            this.before = before;
            this.next = next;
        }

        public EmailRename(string planZakaz, double before, double next, string recipient, bool renameTU)
        {
            this.planZakaz = planZakaz;
            this.recipient = recipient;
            this.renameTU = renameTU;
            massBefare = before;
            massNext = next;
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
            string textBody = subject + "<br/>" + "<br/>";
            if (renameTU == true)
            {
                textBody += "Наименование по ТУ до изменения: " + before + "<br/>";
                textBody += "Наименование по ТУ после изменения: " + next + "<br/>";

            }
            else
            {
                textBody += "Контрактное (договорное) наименование до изменения: " + before + "<br/>";
                textBody += "Контрактное (договорное) наименование после изменения: " + next + "<br/>";
            }
            return textBody;
        }

        void SetMailAddress()
        {
            if(renameTU != true)
                mail.To.Add(new MailAddress("maa@katek.by"));
            mail.To.Add(new MailAddress("myi@katek.by"));
            mail.To.Add(new MailAddress("gea@katek.by"));
            mail.To.Add(new MailAddress("bav@katek.by"));
            mail.To.Add(new MailAddress("pev@katek.by"));
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

        public void SendEmailMassa()
        {
            subject = GetSubjectMass();
            body = GetBodyMass();
            if (recipient != null)
            {
                mail.From = new MailAddress(recipient);
            }
            SetMailAddressMass();
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
            mail.Dispose();
        }

        string GetSubjectMass()
        {
            return "Изменена расчетная масса изделия: " + planZakaz;
        }

        string GetBodyMass()
        {
            string textBody = subject + "<br/>" + "<br/>";
            textBody += "Изменена расчетная масса изделия c: " + massBefare.ToString() + "<br/>";
            textBody += "Изменена расчетная масса изделия на: " + massNext.ToString() + "<br/>";
            return textBody;
        }

        void SetMailAddressMass()
        {
            mail.To.Add(new MailAddress("myi@katek.by"));
            mail.To.Add(new MailAddress("gea@katek.by"));
            mail.To.Add(new MailAddress("bav@katek.by"));
            mail.To.Add(new MailAddress("pev@katek.by"));
            mail.To.Add(new MailAddress("Medvedev@katek.by"));
            mail.To.Add(new MailAddress("lgs@katek.by"));
            foreach (var data in db.AspNetUsers.Where(d => d.Devision == 15).Where(d => d.LockoutEnabled == true))
            {
                mail.To.Add(new MailAddress(data.Email));
            }
        }

    }
}