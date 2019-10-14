using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.CMO.Models
{
    public class EmailCMO : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        int stepNumber;
        string subject;
        string body;
        string login;
        List<string> mailToList;
        CMO2_Order order;
        private new readonly PortalKATEKEntities db = new PortalKATEKEntities();
        public EmailCMO(CMO2_Order order, string login, int stepNumber)
        {
            mailToList = new List<string>();
            try
            {
                this.login = login;
                this.stepNumber = stepNumber;
                this.order = db.CMO2_Order.Find(order.id);
                mail.From = new MailAddress(login);
                if (stepNumber == 1) //Create - 1
                {
                    GetMailListCreate();
                }
                else if (stepNumber == 2) //work - 2
                {
                    GetMailList();
                    GetMailClient();
                }
                else if (stepNumber == 3) //manuf - 3
                {
                    GetMailList();
                    GetMailClient();
                    GetMailPM();
                }
                else if (stepNumber == 4) // Create ReOrder - 4
                {
                    GetMailListCreate();
                    GetMailClient();
                }
                else if (stepNumber == 5) 
                {
                    GetMailPM();
                }
                else
                {

                }
                GetSubject();
                GetBody();
                SendEmail();

                logger.Debug("EmailCMO: " + order.id);
            }
            catch (Exception ex)
            {
                logger.Error("EmailCMO: " + order.id + " | " + ex);
            }
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

        private bool GetSubject()
        {
            if (stepNumber == 1)
            {
                subject = "Размещен новый заказ деталей: " + GetPlanZakazs();
            }
            else
            {
                subject = "Заказ деталей: " + GetPlanZakazs();
            }
            subject = subject.Replace(System.Environment.NewLine, "");
            return true;
        }

        private string GetPlanZakazs()
        {
            string typeName = "";
            foreach (var data in order.CMO2_Position.GroupBy(d => d.CMO_TypeProduct.name).ToList())
            {
                typeName += data.First().CMO_TypeProduct.name + ", ";
            }
            typeName = typeName.Substring(0, typeName.Length - 2);
            string pzs = "";
            foreach (var data in order.CMO2_Position.GroupBy(d => d.PZ_PlanZakaz.PlanZakaz).ToList())
            {
                pzs += data.First().PZ_PlanZakaz.PlanZakaz.ToString() + " - " + typeName + "; ";
            }
            pzs = pzs.Substring(0, pzs.Length - 2);
            pzs += " (заявка № " + order.id.ToString() + ")";
            return pzs;
        }

        private bool GetBody()
        {
            if (stepNumber == 1)
            {
                body = "Добрый день!" + "<br/>" + "Размещаем заказ деталей №: " + order.id + "<br/>";
                body += GetPlanZakazs() + "<br/>";
                body += "Просьба сформировать заявку на первый этап торгов.";
            }
            else if (stepNumber == 2)
            {
                body = "Добрый день!" + "<br/>" + "Размещаем заказ деталей №: " + order.id + "<br/>";
                body += GetPlanZakazs() + "<br/>";
                body += "Прошу прислать сроки готовности заказа: " + order.workDateTime.ToString().Substring(0, 16) + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" + "Гришель Дмитрий Петрович" + "<br/>" + "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                        "Тел:  +375 17 366 90 67(вн. 329)" + "<br/>" + "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" + "Skype: sitek_dima" + "<br/>" +
                        "gdp@katek.by";
            }
            else if(stepNumber == 3)
            {
                body = "Добрый день!" + "<br/>";
                body += "Запускаем заказ в работу, требуемая дата изготовления: " + order.manufDate.ToString().Substring(0, 10) + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" + "Гришель Дмитрий Петрович" + "<br/>" + "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                        "Тел:  +375 17 366 90 67(вн. 329)" + "<br/>" + "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" + "Skype: sitek_dima" + "<br/>" +
                        "gdp@katek.by";
            }
            else if (stepNumber == 4)
            {
                body = "<p>Добрый день!</p>";
                body += "Размещаем дозаказ деталей №: " + order.id;
                subject = "Размещаем дозаказ деталей №: " + order.id;
            }
            else if (stepNumber == 5)
            {
                body = "Добрый день!" + "<br/>";
                body += "Изменена плановая дата изготовления: " + order.manufDate.ToString().Substring(0, 10) + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" + "Гришель Дмитрий Петрович" + "<br/>" + "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                        "Тел:  +375 17 366 90 67(вн. 329)" + "<br/>" + "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" + "Skype: sitek_dima" + "<br/>" +
                        "gdp@katek.by";
            }
            return true;
        }

        bool GetMailListCreate()
        {
            mailToList.Add("xan@katek.by");
            mailToList.Add("myi@katek.by");
            mailToList.Add("gdp@katek.by");
            mailToList.Add("bav@katek.by");
            mailToList.Add("vi@katek.by");
            mailToList.Add("nrf@katek.by");
            mailToList.Add("goa@katek.by");
            return true;
        }

        bool GetMailList()
        {
            mailToList.Add("xan@katek.by");
            mailToList.Add("myi@katek.by");
            mailToList.Add("gdp@katek.by");
            mailToList.Add("bav@katek.by");
            return true;
        }

        bool GetMailClient()
        {
            var query = db.CMO_CompanyMailList.Where(d => d.id_CMO_Company == order.id_CMO_Company && d.active == true).ToList();
            foreach (var data in query)
            {
                mailToList.Add(data.email);
            }
            return true;
        }

        bool GetMailPM()
        {
            mailToList.Add("koag@katek.by");
            mailToList.Add("bav@katek.by");
            mailToList.Add("myi@katek.by");
            mailToList.Add("gea@katek.by");
            return true;
        }

        private List<string> GetFileArray()
        {
            var fileList = Directory.GetFiles(order.folder).ToList();
            return fileList;
        }
    }
}