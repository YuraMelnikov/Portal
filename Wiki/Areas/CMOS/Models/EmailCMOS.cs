using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.CMOS.Models
{
    public class EmailCMOS : EmailClient
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        int stepNumber;
        string subject;
        string body;
        string login;
        List<string> mailToList;
        CMOSPreOrder preOrder;
        CMOSOrder order;
        DateTime? datePlanningGetMaterials;
        private new readonly PortalKATEKEntities db = new PortalKATEKEntities();

        public EmailCMOS(CMOSPreOrder preOrder, string login, int stepNumber)
        {
            mailToList = new List<string>();
            try
            {
                this.login = login;
                this.stepNumber = stepNumber;
                this.preOrder = db.CMOSPreOrder.Find(preOrder.id);
                mail.From = new MailAddress(login);
                GetMailListCreate();
                GetMailPM();
                GetSubject();
                GetBody();
                SendEmail();
                logger.Debug("EmailCMOS: " + preOrder.id);
            }
            catch (Exception ex)
            {
                logger.Error("EmailCMOS: " + preOrder.id + " | " + ex);
            }
        }

        public EmailCMOS(CMOSOrder order, string login, int stepNumber)
        {
            mailToList = new List<string>();
            try
            {
                this.login = login;
                this.stepNumber = stepNumber;
                this.order = db.CMOSOrder.Find(order.id);
                mail.From = new MailAddress(login);
                if (stepNumber == 0) //Create - 0
                {
                    GetMailListCreate();
                    GetMailPM();
                }
                else if (stepNumber == 2) //work - 2
                {
                    GetMailList();
                    GetMailClient();
                    GetMailPM();
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
                    GetMailPM();
                }
                else if (stepNumber == 5)
                {
                    GetMailPM();
                }
                else if (stepNumber == 6) //Remove
                {
                    GetMailClient();
                    GetMailListCreate();
                    GetMailPM();
                }
                else if (stepNumber == 7) //stickers
                {
                    //GetMailList();
                    GetMailListStock();
                    GetMailPM();
                }
                else
                {

                }
                GetSubject();
                GetBody();
                SendEmail();
                logger.Debug("EmailCMOS: " + order.id);
            }
            catch (Exception ex)
            {
                logger.Error("EmailCMOS: " + order.id + " | " + ex);
            }
        }

        public EmailCMOS(CMOSOrder order, string login, int stepNumber, DateTime? datePlanningGetMaterials)
        {
            mailToList = new List<string>();
            try
            {
                this.datePlanningGetMaterials = datePlanningGetMaterials;
                this.login = login;
                this.stepNumber = stepNumber;
                this.order = db.CMOSOrder.Find(order.id);
                mail.From = new MailAddress(login);
                GetMailList();
                GetMailClient();
                GetMailPM();
                GetSubject();
                GetBody();
                SendEmail();
                logger.Debug("EmailCMOS: " + order.id);
            }
            catch (Exception ex)
            {
                logger.Error("EmailCMOS: " + order.id + " | " + ex);
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
            if (stepNumber == 0)
            {
                subject = "Размещен новый предзаказ деталей: " + GetPreorderName();
            }
            else if (stepNumber == 2)
            {
                subject = "Размещен новый заказ деталей: " + order.id + " " + GetPositionsOrderName();
            }
            else if (stepNumber == 4)
            {
                subject = "Размещен дозаказ деталей: " + order.id;
            }
            else if (stepNumber == 6)
            {
                subject = "Удален заказ деталей железа №: " + order.id + " " + GetPositionsOrderName();
            }
            else
            {
                subject = "Заказ деталей: " + order.id;
            }
            subject = subject.Replace(Environment.NewLine, "");
            return true;
        }

        private string  GetPositionsOrderName()
        {
            string res = "(";
            var list = db.CMOSOrderPreOrder
                .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                .Where(a => a.id_CMOSOrder == order.id)
                .ToList();
            foreach (var prd in list)
            {
                res += prd.id + " - " + prd.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + ": " + prd.CMOSPreOrder.CMO_TypeProduct.name + "; ";
            }
            return res + ")";
        }

        private string GetPreorderName()
        {
            return db.PZ_PlanZakaz.Find(preOrder.id_PZ_PlanZakaz).PlanZakaz.ToString() + " - " + db.CMO_TypeProduct.Find(preOrder.id_CMO_TypeProduct).name;
        }

        private bool GetBody()
        {
            if (stepNumber == 0)
            {
                body = "Добрый день!" + "<br/>" + "Размещаем предзаказ деталей №: " + preOrder.id + "<br/>";
                body += GetPreorderName() + "<br/>";
                body += "Просьба сформировать заказ.";
            }
            //else if (stepNumber == 1)
            //{
            //    body = "Добрый день!" + "<br/>" + "Размещаем заказ деталей №: " + order.id + "<br/>";
            //    body += GetPlanZakazs() + "<br/>";
            //    body += "Просьба сформировать заявку на первый этап торгов.";
            //}
            else if (stepNumber == 2)
            {
                body = "Добрый день!" + "<br/>" + "Размещаем заказ деталей №: " + order.id + "<br/>";
                body += "Прошу прислать сроки готовности заказа: " + order.workDate.ToString().Substring(0, 16) + "<br/>";
                if(datePlanningGetMaterials != null)
                {
                    body += "Требуемая дата поставки: " + datePlanningGetMaterials.Value.ToShortDateString() + "<br/>";
                }
                body += "Внутренний номер зказа: " + order.id.ToString() + ", просим прописывать в накладной." + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" + "Гришель Дмитрий Петрович" + "<br/>" + "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                        "Тел:  +375 17 366 90 67(вн. 329)" + "<br/>" + "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" + "Skype: sitek_dima" + "<br/>" +
                        "gdp@katek.by";
            }
            else if (stepNumber == 3)
            {
                body = "Добрый день!" + "<br/>";
                body += "Запускаем заказ в работу, требуемая дата изготовления: " + order.manufDate.ToString().Substring(0, 10) + "<br/>";
                body += "Внутренний номер зказа: " + order.id.ToString() + ", просим прописывать в накладной." + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" + "Гришель Дмитрий Петрович" + "<br/>" + "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                        "Тел:  +375 17 366 90 67(вн. 329)" + "<br/>" + "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" + "Skype: sitek_dima" + "<br/>" +
                        "gdp@katek.by";
            }
            else if (stepNumber == 4)
            {
                body = "Добрый день!" + "<br/>" + "Размещаем ДОЗАКАЗ деталей №: " + order.id + "<br/>";
                body += "Прошу прислать сроки готовности дозаказа: " + order.workDate.ToString().Substring(0, 16) + "<br/>";
                body += "Внутренний номер дозаказа: " + order.id.ToString() + ", просим прописывать в накладной." + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" + "Гришель Дмитрий Петрович" + "<br/>" + "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                        "Тел:  +375 17 366 90 67(вн. 329)" + "<br/>" + "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" + "Skype: sitek_dima" + "<br/>" +
                        "gdp@katek.by";
            }
            else if (stepNumber == 5)
            {
                body = "Добрый день!" + "<br/>";
                body += "Изменена плановая дата изготовления: " + order.manufDate.ToString().Substring(0, 10) + "<br/>" + "<br/>";
                body += "С уважением," + "<br/>" + "Гришель Дмитрий Петрович" + "<br/>" + "Начальник отдела по материально - техническому снабжению" + "<br/>" +
                        "Тел:  +375 17 366 90 67(вн. 329)" + "<br/>" + "Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35" + "<br/>" + "Skype: sitek_dima" + "<br/>" +
                        "gdp@katek.by";
            }
            else if (stepNumber == 6)
            {
                body += "Удален заказ железа №: " + order.id + "<br/>" + "<br/>";
            }
            return true;
        }

        bool GetMailListCreate()
        {
            mailToList.Add("xan@katek.by");
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
            mailToList.Add("gdp@katek.by");
            mailToList.Add("bav@katek.by");
            return true;
        }

        bool GetMailListStock()
        {
            mailToList.Add("snv@katek.by");
            mailToList.Add("sim@katek.by");
            return true;
        }

        bool GetMailClient()
        {
            var query = db.CMO_CompanyMailList.Where(d => d.id_CMO_Company == order.cMO_CompanyId && d.active == true).ToList();
            foreach (var data in query)
            {
                mailToList.Add(data.email);
            }
            return true;
        }

        bool GetMailPM()
        {
            //mailToList.Add("koag@katek.by");
            mailToList.Add("myi@katek.by");
            return true;
        }

        private List<string> GetFileArray()
        {
            if(stepNumber == 0)
                return Directory.GetFiles(preOrder.folder).ToList();
            else
                return Directory.GetFiles(order.folder).ToList();
        }
    }
}