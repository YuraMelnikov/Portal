﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;

namespace Wiki.Areas.CMO.Models
{
    public class EmailCMO : EmailClient
    {
        int stepNumber;
        string subject;
        string body;
        string login;
        List<string> mailToList;
        CMO2_Order order;
        readonly PortalKATEKEntities db = new PortalKATEKEntities();
        public EmailCMO(CMO2_Order order, string login, int stepNumber)
        {
            try
            {
                this.login = login;
                this.stepNumber = stepNumber;
                this.order = db.CMO2_Order.Find(order.id);
                mail.From = new MailAddress(login);
                if (stepNumber == 1) //Create - 1
                {
                    //GetMailListCreate();
                }
                else if (stepNumber == 2) //work - 2
                {
                    //GetMailList();
                    //GetMailClient();
                }
                else if (stepNumber == 3) //manuf - 3
                {
                    //GetMailList();
                    //GetMailClient();
                }
                else if(stepNumber == 4) // Create ReOrder - 4
                {
                    //GetMailListCreate();
                    //GetMailClient();
                }
                else
                {
                }
                GetSubject();
                GetBody();
                SendEmail();
            }
            catch
            {

            }
        }

        void SendEmail()
        {
            foreach (var data in GetFileArray())
            {
                mail.Attachments.Add(new Attachment(data));
            }
            //foreach (var dataUser in mailToList)
            //{
            //    mail.To.Add(new MailAddress(dataUser));
            //}
            mail.To.Add(new MailAddress("myi@katek.by"));
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
                body = @"<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 style='mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'><tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'><td colspan=2 style='background:#124280;padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif;color:white'>";
                body += "Размещаем заказ деталей</br>";
                body += @"</span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr>";
                body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Описание заявки: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + GetPlanZakazs();
                body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> № заявки: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + order.id.ToString();
                body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Сотрудник сформировавший заявку: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + order.AspNetUsers.CiliricalName;
                body += @"</table>";
                body += @"</br></br></br></br><p>Просьба сформировать заявку на первый этап торгов</p>";
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
                body = "<p>Добрый день!</p>";
                body += "<p>Запускаем заказ в работу: </p>";
                body += @"<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 style='mso-cellspacing:0cm;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'><tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes'><td colspan=2 style='background:#124280;padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif;color:white'>";
                body += subject;
                body += @"</span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr>";
                body += @"</span><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span></tr><tr style='mso-yfti-irow:2'><td nowrap valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal align=right style='text-align:right'><span style='mso-bookmark:_MailOriginal'><b><span style='font-family:Arial,sans-serif'> Описание заявки: </span></b><o:p></o:p></span></p></td><span style='mso-bookmark:_MailOriginal'></span><td valign=top style='padding:2.25pt 2.25pt 2.25pt 2.25pt'><p class=MsoNormal><span style='mso-bookmark:_MailOriginal'><span style='font-family:Arial,sans-serif'>" + GetPlanZakazs();
                body += @"</table>";
                body += "<p></p><p></p><p></p>";
                body += @"Требуемая дата изготовления: " + order.manufDate.ToString().Substring(0, 10);
                body += "<p></p><p></p><p></p>";
                body += "<p>С уважением,</p>";
                body += "<p>Гришель Дмитрий Петрович</p>";
                body += "<p>Начальник отдела по материально - техническому снабжению</p>";
                body += "<p>Тел: +375 17 366 90 67(вн. 329)</p>";
                body += "<p>Моб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35</p>";
                body += "<p>Skype: sitek_dima</p>";
                body += "<p>gdp@katek.by</p>";
            }
            else if (stepNumber == 4)
            {
                body = "<p>Добрый день!</p>";
                body += "Размещаем дозаказ деталей №: " + order.id;
                subject = "Размещаем дозаказ деталей №: " + order.id;
            }
            return true;
        }

        bool GetMailListCreate()
        {
            mailToList = new List<string>();
            mailToList.Add("myi@katek.by");
            mailToList.Add("gdp@katek.by");
            mailToList.Add("Antipov@katek.by");
            mailToList.Add("vi@katek.by");
            mailToList.Add("yaa@katek.by");
            mailToList.Add("nrf@katek.by");
            return true;
        }

        bool GetMailList()
        {
            mailToList = new List<string>();
            mailToList.Add("myi@katek.by");
            mailToList.Add("gdp@katek.by");
            mailToList.Add("Antipov@katek.by");
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
            mailToList = new List<string>();
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