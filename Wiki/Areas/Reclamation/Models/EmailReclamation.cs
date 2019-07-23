using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.Reclamation.Models
{
    public class EmailReclamation : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //Stepnumber:
        //Create - 1
        //Reload - 2
        //Answer - 3
        //Remove - 4

        int stepNumber;
        string subject;
        string body;
        string login;
        List<string> mailToList;
        Wiki.Reclamation reclamation;
        private new readonly PortalKATEKEntities db = new PortalKATEKEntities();
        public EmailReclamation(Wiki.Reclamation reclamation, string login, int stepNumber)
        {
            try
            {
                this.login = login;
                this.stepNumber = stepNumber;
                this.reclamation = db.Reclamation.Find(reclamation.id);
                mail.From = new MailAddress(login);
                GetMailList();
                GetSubject();
                GetBody();
                //SendEmail();
            }
            catch (Exception ex)
            {
                logger.Error("EmailReclamation: ReclamationId: " + reclamation.id + " | Exeption text: " + ex);
            }
        }

        void SendEmail()
        {
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
            mail.Dispose();
        }

        private bool GetSubject()
        {
            subject = "Замечание ";
            subject += reclamation.Devision1.name + " по зак.: " + GetPlanZakazs() + "; " + GetProcessName();
            return true;
        }

        private string GetPlanZakazs()
        {
            string pzs = "";
            foreach (var data in reclamation.Reclamation_PZ.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList())
            {
                pzs += data.PZ_PlanZakaz.PlanZakaz.ToString() + ", ";
            }
            pzs = pzs.Substring(0, pzs.Length - 2);
            return pzs;
        }

        private string GetProcessName()
        {
            string name = "";
            if (stepNumber == 1)
                name = "создано";
            else if (stepNumber == 2)
                name = "перенаправлено";
            else if (stepNumber == 3)
                name = "направлен ответ";
            else if (stepNumber == 4)
                name = "УДАЛЕНА";
            return name;
        }

        private bool GetBody()
        {
            body = @"<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 style='mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'><tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'><td colspan=2 style='background:#124280;padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif;color:white'>";
            body += subject;
            body += @"</span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr>";
            if(stepNumber == 4)
                body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> ЗАМЕЧАНИЕ УДАЛЕНО </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>";
            body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Автор замечания: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + reclamation.AspNetUsers.CiliricalName;
            body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Ответственное СП: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + reclamation.Devision.name;
            body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> № замечания:</span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + reclamation.id;
            body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Текст замечания:</span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + reclamation.text;
            body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Прим.: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + reclamation.description;
            body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Полуфабрикат: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + reclamation.PF.name;
            body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> PCAM: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + reclamation.PCAM;
            if(stepNumber != 1)
            {
                ReclamationViwers reclamationViwers = new ReclamationViwers(reclamation);
                body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Сотрудник: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + db.AspNetUsers.First(d => d.Email == login).CiliricalName;
                body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> История переписки: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + reclamationViwers.Answers;
            }
            body += @"</table>";
            return true;
        }

        bool GetMailList()
        {
            mailToList = new List<string>();
            mailToList.Add("myi@katek.by");
            if(reclamation.id_DevisionReclamation == 3 || reclamation.id_DevisionReclamation == 16)
            {
                foreach (var data in db.AspNetUsers.Where(d => d.Devision == 3 || d.Devision == 16).Where(d => d.LockoutEnabled == true))
                {
                    mail.To.Add(new MailAddress(data.Email));
                }
            }
            else
            {
                foreach (var data in db.AspNetUsers.Where(d => d.Devision == reclamation.id_DevisionReclamation).Where(d => d.LockoutEnabled == true))
                {
                    mail.To.Add(new MailAddress(data.Email));
                }
            }
            if(reclamation.editManufacturing == true)
            {
                foreach (var data in db.AspNetUsers.Where(d => d.Devision == reclamation.editManufacturingIdDevision).Where(d => d.LockoutEnabled == true))
                {
                    mail.To.Add(new MailAddress(data.Email));
                }
            }
            foreach(var dataList in mailToList)
            {
                mail.To.Add(new MailAddress(dataList));
            }
            mailToList.Add(db.AspNetUsers.Find(reclamation.id_AspNetUsersCreate).CiliricalName);
            return true;
        }
    }
}