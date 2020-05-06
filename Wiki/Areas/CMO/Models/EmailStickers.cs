using NLog;
using System.Collections.Generic;
using Wiki.Models;
using System.Linq;
using System.Net.Mail;
using System;
using System.IO;

namespace Wiki.Areas.CMO.Models
{
    public class EmailStickers : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        int stepNumber;
        string subject;
        string body;
        string login;
        List<string> mailToList;
        StickersPreOrder order;

        public EmailStickers(StickersPreOrder order, string login, int stepNumber)
        {
            mailToList = new List<string>();
            try
            {
                this.login = login;
                this.stepNumber = stepNumber;
                this.order = db.StickersPreOrder.First(d => d.id == order.id);
                mail.From = new MailAddress(this.login);
                if (stepNumber == 1) //CreateOrder - 1
                {
                    GetMailPurchaseDepartment();
                    GetMailKO();
                    GetMailMYI();

                }
                else if (stepNumber == 2) //CreateReOrder - 2
                {
                    GetMailPurchaseDepartment();
                    GetMailKO();
                    GetMailMYI();
                }
                else if (stepNumber == 3) //CreateSimpleOrder - 3
                {
                    GetMailPurchaseDepartment();
                    GetMailKO();
                    GetMailMYI();
                }
                else if (stepNumber == 4) //Push - 4
                {
                    GetMailCustomer();
                    GetMailPurchaseDepartment();
                    GetMailKO();
                    GetMailMYI();
                }
                else
                {
                    GetMailMYI();
                }
                GetSubject();
                GetBody();
                SendEmail();
                logger.Debug("EmailStickers: " + order.id);
            }
            catch (Exception ex)
            {
                logger.Error("EmailStickers: " + order.id + " | " + ex);
            }
        }

        private bool GetSubject()
        {
            if (stepNumber == 1)
            {
                subject = "Размещен новый заказ наклеек: " + GetPlanZakazs();
            }
            else if (stepNumber == 2)
            {
                subject = "Размещен новый дозаказ наклеек: " + GetPlanZakazs();
            }
            else if (stepNumber == 3)
            {
                subject = "Размещен новый заказ наклеек (общий)";
            }
            else
            {
                subject = "Заказ наклеек: " + GetPlanZakazs();
            }
            subject = subject.Replace(Environment.NewLine, "");
            return true;
        }

        private string GetPlanZakazs()
        {
            string pzs = "";
            if(order.id_PZ_PlanZakaz == null)
            {
                pzs += " (заявка № " + order.id.ToString() + ")";
            }
            else
            {
                pzs += "заказ № " + db.PZ_PlanZakaz.Find(order.id_PZ_PlanZakaz).PlanZakaz.ToString();
                pzs += " (заявка № " + order.id.ToString() + ")";
            }
            return pzs;
        }

        private bool GetBody()
        {
            if (stepNumber == 1)
            {
                body = "Размещен новый заказ наклеек: " + GetPlanZakazs() + "<br/>";
                body += "Требуемая дата изготовления: " + order.deadline.ToShortDateString() + "<br/>";
                if(order.description != "")
                    body += "Прим.: " + order.description + "<br/>";
            }
            else if (stepNumber == 2)
            {
                body = "Размещен новый дозаказ наклеек: " + GetPlanZakazs() + "<br/>";
                body += "Требуемая дата изготовления: " + order.deadline.ToShortDateString() + "<br/>";
                if (order.description != "")
                    body += "Прим.: " + order.description + "<br/>";
            }
            else if (stepNumber == 3)
            {
                body = "Размещен новый общий заказ наклеек: заявка №" + order.id + "<br/>";
                body += "Требуемая дата изготовления: " + order.deadline.ToShortDateString() + "<br/>";
                if (order.description != "")
                    body += "Прим.: " + order.description + "<br/>";
            }
            else if (stepNumber == 4)
            {
                body = "Добрый день!" + "<br/>";
                body += "Просим выслать предложение/счет на поставку наклеек. Внутренний номер " + order.id + ", просим прописывать в счете." + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" +
                    "Гришель Дмитрий Петрович" + "<br/>" +
                    "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                    "Тел: +375 17 366 90 67(вн. 329)" + "<br/>" +
                    "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" +
                    "Skype: sitek_dima" + "<br/>";
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
            string directory = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Stickers\" + order.id.ToString() + @"\";
            var fileList = Directory.GetFiles(directory).ToList();
            return fileList;
        }

        bool GetMailMYI()
        {
            mailToList.Add("myi@katek.by");
            return true;
        }

        bool GetMailKO()
        {
            //mailToList.Add("nrf@katek.by");
            //mailToList.Add("vi@katek.by");
            //mailToList.Add("fvs@katek.by");
            return true;
        }

        bool GetMailCustomer()
        {
            return true;
        }

        bool GetMailPurchaseDepartment()
        {
            //mailToList.Add("xan@katek.by");
            //mailToList.Add("gdp@katek.by");
            return true;
        }
    }
}