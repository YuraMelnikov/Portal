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
                else if (stepNumber == 5) 
                {
                    GetMailPurchaseDepartment();
                    GetMailKO();
                    GetMailMYI();
                }
                else if (stepNumber == 6) 
                {
                    GetMailPurchaseDepartment();
                    GetMailKO();
                    GetMailMYI();
                }
                else if (stepNumber == 7) 
                {
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
                subject = "Размещен новый заказ наклеек: " + order.orderNumString;
            }
            else if (stepNumber == 2)
            {
                subject = "Размещен новый дозаказ наклеек: " + order.orderNumString;
            }
            else if (stepNumber == 3)
            {
                subject = "Размещен новый заказ наклеек (общий): " + order.orderNumString;
            }
            else if (stepNumber == 4)
            {
                subject = "ЗАО КАТЭК. Заказ наклеек. Заказ №:" + order.orderNumString;
            }
            else if(stepNumber == 5)
            {
                subject = "Изменен требуемый срок поставки наклеек: " + order.orderNumString;
            }
            else if (stepNumber == 6)
            {
                subject = "Наклейки поступили на склад: " + order.orderNumString;
            }
            else if (stepNumber == 7)
            {
                subject = "Удален (отменен) заказ наклеек: " + order.orderNumString;
            }
            else
            {
                subject = "Заказ наклеек: " + order.orderNumString;
            }
            subject = subject.Replace(Environment.NewLine, "");
            return true;
        }

        private bool GetBody()
        {
            if (stepNumber == 1)
            {
                body = "Размещен новый заказ наклеек: " + order.orderNumString + "<br/>";
                body += "Требуемая дата изготовления: " + order.deadline.ToShortDateString() + "<br/>";
                if(order.description != "")
                    body += "Прим.: " + order.description + "<br/>";
            }
            else if (stepNumber == 2)
            {
                body = "Размещен новый дозаказ наклеек: " + order.orderNumString + "<br/>";
                body += "Требуемая дата изготовления: " + order.deadline.ToShortDateString() + "<br/>";
                if (order.description != "")
                    body += "Прим.: " + order.description + "<br/>";
            }
            else if (stepNumber == 3)
            {
                body = "Размещен новый общий заказ наклеек: " + order.orderNumString + "<br/>";
                body += "Требуемая дата изготовления: " + order.deadline.ToShortDateString() + "<br/>";
                if (order.description != "")
                    body += "Прим.: " + order.description + "<br/>";
            }
            else if (stepNumber == 4)
            {
                body = "Добрый день!" + "<br/>";
                body += "Просим изготовить наклейки в соответствии с вложениями." + "<br/>";
                body += "Заказ № " + order.orderNumString + "<br/>";
                body += "Крайний срок изготовления: " + order.deadline.ToShortDateString() + "<br/>";
                if (order.description != "")
                    body += "Прим.: " + order.description + "<br/>" + "<br/>";
                else
                    body += "<br/>";
                body += "С уважением," + "<br/>" +
                    "Филончик Валентина Сергеевна" + "<br/>" +
                    "начальник конструкторского бюро электриков" + "<br/>" +
                    "Моб.:   + 375 44 546 24 20" + "<br/>" +
                    "Раб.:   +375 17 366 94 15 ( вн. 337)" + "<br/>" +
                    "fvs@katek.by" + "<br/>";
            }
            else if(stepNumber == 5)
            {
                body = "Изменен ожидаемый срок поставки наклеек: " + order.orderNumString + "<br/>";
                body += "Требуемая дата изготовления: " + order.deadline.ToShortDateString() + "<br/>";
                body += "Дата готовности поставщика: " + order.datePlanUpload.ToShortDateString() + "<br/>";
                if (order.description != "")
                    body += "Прим.: " + order.description + "<br/>";
            }
            else if (stepNumber == 6)
            {
                body = "Наклейки поступили на склад: " + order.orderNumString + "<br/>";
            }
            else if (stepNumber == 7)
            {
                body = "Удален (отменен) заказ наклеек: " + order.orderNumString;
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
            string directory = "";
            if(order.id_PZ_PlanZakaz == null)
                directory = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Stickers\" + order.orderNumString + @"\";
            else
                directory = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Stickers\" + order.orderNumString + order.id + @"\";
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
            mailToList.Add("nrf@katek.by");
            mailToList.Add("vi@katek.by");
            mailToList.Add("fvs@katek.by");
            try
            {
                mailToList.Add(GetUserPost());
            }
            catch
            {

            }
            return true;
        }

        bool GetMailCustomer()
        {
            mailToList.Add("stickers@katek.by");
            return true;
        }

        bool GetMailPurchaseDepartment()
        {
            return true;
        }

        private string GetUserPost()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return db.AspNetUsers.First(a => a.Id == order.id_AspNetUsersCreate).Email;
                }
                catch
                {
                    return "";
                }
            }
        }
    }
}