using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.CMO.Models
{
    public class EmailSandwichPanel : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        int stepNumber;
        string subject;
        string body;
        string login;
        List<string> mailToList;
        SandwichPanel order;

        public EmailSandwichPanel(SandwichPanel order, string login, int stepNumber)
        {
            mailToList = new List<string>();
            try
            {
                this.login = login;
                this.stepNumber = stepNumber;
                this.order = db.SandwichPanel
                    .Include(d => d.SandwichPanel_PZ.Select(s => s.PZ_PlanZakaz))
                    .First(d => d.id == order.id);
                mail.From = new MailAddress(this.login);
                if (stepNumber == 1) //Create - 1
                {
                    GetMailPM();
                    GetMailManufacturing();
                    GetMailMYI();
                    GetMailKO();
                }
                else if (stepNumber == 2) //ToWork - 2
                {
                    GetMailPurchaseDepartment();
                    GetMailPM();
                    GetMailMYI();
                }
                else if (stepNumber == 3) //ToUpdate - 3
                {
                    GetMailKB();
                    GetMailPM();
                    GetMailMYI();
                }
                else if (stepNumber == 4) //ToCustomer - 4
                {
                    GetMailCustomer();
                    GetMailPM();
                    GetMailMYI();
                }
                else if (stepNumber == 5) //ToGetDateComplited
                {
                    GetMailPM();
                    GetMailManufacturing();
                    GetMailMYI();
                }
                else if (stepNumber == 6) //ToComplited
                {
                    GetMailPM();
                    GetMailManufacturing();
                    GetMailMYI();
                }
                else
                {
                    GetMailMYI();
                }
                GetSubject();
                GetBody();
                SendEmail();
                logger.Debug("EmailSandwichPanel: " + order.id);
            }
            catch (Exception ex)
            {
                logger.Error("EmailSandwichPanel: " + order.id + " | " + ex);
            }
        }

        private bool GetSubject()
        {
            if (stepNumber == 1)
            {
                subject = "Размещен новый заказ сэндвич-панелей: " + GetPlanZakazs();
            }
            else
            {
                subject = "Заказ сэндвич-панелей: " + GetPlanZakazs();
            }
            subject = subject.Replace(Environment.NewLine, "");
            return true;
        }

        private string GetPlanZakazs()
        {
            string pzs = "";
            foreach (var data in order.SandwichPanel_PZ.ToList())
            {
                pzs += data.PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            pzs += " (заявка № " + order.id.ToString() + ")";
            return pzs;
        }

        private bool GetBody()
        {
            if (stepNumber == 1)
            {
                body = "Добрый день!" + "<br/>" + "Размещаем заказ сэндвич-панелей №: " + order.id + "<br/>";
                body += GetPlanZakazs() + "<br/>";
                body += "Просьба произвести сверку данных.";
            }
            else if (stepNumber == 2)
            {
                body = "Добрый день!" + "<br/>" + "Заказ сэндвич-панелей №: " + order.id + "<br/>";
                body += GetPlanZakazs() + "<br/>";
                body += "Сверка данных прошла успешно, можно запускать в работу." + "<br/>" + "<br/>";
            }
            else if (stepNumber == 3)
            {
                body = "Добрый день!" + "<br/>" + "Заказ сэндвич-панелей №: " + order.id + "<br/>";
                body += GetPlanZakazs() + "<br/>";
                body += "Необходимо внести изменения в заказ." + "<br/>" + "<br/>";
            }
            else if (stepNumber == 4)
            {
                body = "Добрый день!" + "<br/>";
                body += "Просим выслать предложение/счет на поставку сэндвич-панелей. Внутренний номер " + GetPlanZakazs() + ", просим прописывать в счете." + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" +
                    "Хардин Александр Николаевич" + "<br/>" +
                    "Тел: 8(017) 366-91-38   внутр - 349" + "<br/>" +
                    "Тел. (Моб): + 375 29 384 66 75" + "<br/>" +
                    "Тел  (Моб раб) +375 44 544 01 33" + "<br/>" +
                    "Skype: aleksandr.h.86" + "<br/>";
            }
            else if (stepNumber == 5)
            {
                body = "Добрый день!" + "<br/>" + "Заказ сэндвич-панелей №: " + order.id + "<br/>";
                body += GetPlanZakazs() + "<br/>";
                body += "Получена плановая дата изготовления сэндвич-панелей: " + order.datetimePlanComplited.ToString().Substring(0, 10) + "<br/>" + "<br/>";
            }
            else if (stepNumber == 6)
            {
                body = "Добрый день!" + "<br/>" + "Заказ сэндвич-панелей №: " + order.id + "<br/>";
                body += GetPlanZakazs() + "<br/>";
                body += "Сэндвич-панели поступили на хранение: " + order.numberOrder + "<br/>" + "<br/>";
            }
            return true;
        }

        void SendEmail()
        {
            foreach (var data in GetFileArray())
            {
                mail.Attachments.Add(new Attachment(data));
            }
            foreach (var dataUser in mailToList)
            {
                mail.To.Add(new MailAddress(dataUser));
            }
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
            mail.Dispose();
        }

        private List<string> GetFileArray()
        {
            var fileList = Directory.GetFiles(order.folder).ToList();
            return fileList;
        }

        bool GetMailMYI()
        {
            mailToList.Add("myi@katek.by");
            mailToList.Add("xan@katek.by");
            return true;
        }

        bool GetMailKO()
        {
            mailToList.Add("nrf@katek.by");
            mailToList.Add("vi@katek.by");
            mailToList.Add("goa@katek.by");
            return true;
        }

        bool GetMailPM()
        {
            mailToList.Add("bav@katek.by");
            return true;
        }

        bool GetMailCustomer()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var query = db.SandwichPanelCustomer.Where(d => d.id == order.id_SandwichPanelCustomer && d.active == true).ToList();
                foreach (var data in query)
                {
                    mailToList.Add(data.email);
                }
                return true;
            }
        }

        bool GetMailKB()
        {
            mailToList.Add("nrf@katek.by");
            mailToList.Add("vi@katek.by");
            mailToList.Add("goa@katek.by");
            return true;
        }

        bool GetMailManufacturing()
        {
            mailToList.Add("ovp@katek.by");
            return true;
        }

        bool GetMailPurchaseDepartment()
        {
            mailToList.Add("xan@katek.by");
            mailToList.Add("gdp@katek.by");
            return true;
        }
    }
}