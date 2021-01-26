using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Wiki.Models;
using System.Data.Entity;
using System.IO;

namespace Wiki.Areas.Plexiglass.Models
{
    public class EmailPlexiglas : EmailClient
    {
        #region Fields

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        int _stepNumber;
        string _subject;
        string _body;
        string _login;
        List<string> _mailToList;
        PlexiglassOrder _order;
        private new readonly PortalKATEKEntities _db = new PortalKATEKEntities();

        #endregion

        public EmailPlexiglas(PlexiglassOrder order, string login, int stepNumber)
        {
            _mailToList = new List<string>();
            _order = order;
            _login = login;
            _stepNumber = stepNumber;
            try
            {
                mail.From = new MailAddress(_login);
                if (_stepNumber == 0) //Create - 0
                {
                    //GetMailListCreate();
                    GetMailPM();
                    //GetMailClient();
                }
                else if (_stepNumber == 1) //Remove - 2
                {
                    //GetMailListCreate();
                    GetMailPM();
                    //GetMailClient();
                }
                else
                {
                }
                GetSubject();
                GetBody();
                SendEmail();
                _logger.Debug("EmailPlexiglas: " + order.id);
            }
            catch (Exception ex)
            {
                _logger.Error("EmailPlexiglas: " + order.id + " | " + ex);
            }
        }

        private bool GetSubject()
        {
            if (_stepNumber == 0)
            {
                _subject = "Заказ деталей из поликарбоната: " + GetPositionsOrderName();
            }
            else if (_stepNumber == 1)
            {
                _subject = "ОТМЕНЕН заказ деталей из поликарбоната: " + _order.id + " " + GetPositionsOrderName();
            }
            else 
            {
            }
            _subject = _subject.Replace(Environment.NewLine, "");
            return true;
        }

        private bool GetBody()
        {
            if (_stepNumber == 0)
            {
                _body = "Добрый день!" + "<br/>" + "Размещаем заказ деталей из поликарбоната 3мм.";
            }
            else if (_stepNumber == 1)
            {
                _body = "Добрый день!" + "<br/>" + "ОТМЕНЯЕМ заказ деталей из поликарбоната 3мм.";
            }
            else 
            {
            }
            return true;
        }

        private void SendEmail()
        {
            foreach (var data in GetFileArray())
            {
                mail.Attachments.Add(new Attachment(data));
            }
            foreach (var dataUser in _mailToList)
            {
                mail.To.Add(new MailAddress(dataUser));
            }
            mail.IsBodyHtml = true;
            mail.Subject = _subject;
            mail.Body = _body;
            client.Send(mail);
            mail.Dispose();
        }

        private List<string> GetFileArray()
        {
            var fileList = Directory.GetFiles(_order.folder).ToList();
            return fileList;
        }

        private string GetPositionsOrderName()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                PlexiglassOrder ord = db.PlexiglassOrder
                    .AsNoTracking()
                    .Include(a => a.CMO_TypeProduct)
                    .Include(a => a.PZ_PlanZakaz)
                    .Where(a => a.id == _order.id)
                    .First();

                return " " + ord.CMO_TypeProduct.name + " №" + ord.PZ_PlanZakaz.PlanZakaz.ToString();
            }
        }

        #region MailList

        private bool GetMailListCreate()
        {
            _mailToList.Add("xan@katek.by");
            _mailToList.Add("gdp@katek.by");
            _mailToList.Add("moi@katek.by");

            _mailToList.Add("vi@katek.by");
            _mailToList.Add("nrf@katek.by");

            _mailToList.Add("naa@katek.by");
            return true;
        }

        private bool GetMailClient()
        {
            var query = db.PlexiglassCompany
                .AsNoTracking()
                .Where(d => d.id == _order.id_PlexiglassCompany)
                .ToList();
            foreach (var data in query)
            {
                _mailToList.Add(data.email);
            }
            return true;
        }

        private bool GetMailPM()
        {
            _mailToList.Add("myi@katek.by");
            return true;
        }

        #endregion
    }
}