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
                    .Find(order.id);
                mail.From = new MailAddress(login);
                if (stepNumber == 1) //Create - 1
                {
                    GetMailListCreate();
                }
                else if (stepNumber == 2) //ToWork - 2
                {
                    GetMailList();
                    GetMailClient();
                }
                else if (stepNumber == 3) //ToUpdate - 3
                {
                    GetMailList();
                    GetMailClient();
                    GetMailPM();
                }
                else if (stepNumber == 4) //ToCustomer - 4
                {
                    GetMailListCreate();
                    GetMailClient();
                }
                else if (stepNumber == 5) //ToGetDateComplited
                {
                    GetMailPM();
                }
                else if (stepNumber == 6) //ToComplited
                {
                    GetMailPM();
                }
                else
                {

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
                body += "Размещаем заказ сэндвич-панелей: " + order.id + "<br/>" + "<br/>";
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
                body += "Получена плановая дата изготовления сэндвич-панелей: " + order..ToString().Substring(0, 10) + "<br/>" + "<br/>";
            }
            else if (stepNumber == 6)
            {
                body = "Добрый день!" + "<br/>";
                body += "Изменена плановая дата изготовления: " + order.manufDate.ToString().Substring(0, 10) + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" + "Гришель Дмитрий Петрович" + "<br/>" + "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                        "Тел:  +375 17 366 90 67(вн. 329)" + "<br/>" + "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" + "Skype: sitek_dima" + "<br/>" +
                        "gdp@katek.by";
            }
            return true;
        }
    }
}