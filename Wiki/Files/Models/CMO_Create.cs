﻿ using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wiki.Models
{
    public class CMO_Create
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private string goodConstructor = "успешная передача/присвоение данных конструктора ";
        private string badConstructor = "ошибка передача/присвоение данных конструктора ";
        private string goodDB = "успешное сохранение даных на сервере ";
        private string badDB = "ошибка сохранениея даных на сервере ";
        
        private PortalKATEKEntities db = new PortalKATEKEntities();
        private HttpPostedFileBase[] fileUploadArray;
        private int[] pZiDArray;
        private int[] typeProductArray;
        private string loginIdCreate;
        private int idOrder;
        private int idTender;
        
        //private string listMail_CMO_Create = "myi@katek.by; gdp@katek.by; Antipov@katek.by; vi@katek.by; yaa@katek.by; nrf@katek.by";

        public CMO_Create (int idOrder)
        {
            this.idOrder = idOrder;
        }
        
        public CMO_Create (HttpPostedFileBase[] fileUploadArray, int[] pZiDArray, int[] typeProductArray, string loginIdCreate)
        {
            try
            {
                this.fileUploadArray = fileUploadArray;
                this.pZiDArray = pZiDArray;
                this.typeProductArray = typeProductArray;
                this.loginIdCreate = loginIdCreate;
                logger.Debug(goodConstructor + "CMO_Create (HttpPostedFileBase[] fileUploadArray, int[] pZiDArray, int[] typeProductArray, string loginIdCreate)" 
                    + loginIdCreate.ToString());
            }
            catch(Exception ex)
            {
                logger.Error(badConstructor + "CMO_Create (HttpPostedFileBase[] fileUploadArray, int[] pZiDArray, int[] typeProductArray, string loginIdCreate)"
                    + loginIdCreate.ToString() + ex.Message.ToString());
            }

            CreateOrder();
            CreateFolderAndFileForOrder();
            CreateCMO_PositionOrder();
            PushMailToOsOnCreateOrder();
        }

        private void CreateOrder()
        {
            CMO_Order cMO_Order = new CMO_Order();
            try
            {
                cMO_Order.idTime = 1;
                cMO_Order.dateCreate = DateTime.Now;
                cMO_Order.userCreate = this.loginIdCreate;
                db.CMO_Order.Add(cMO_Order);
                db.SaveChanges();
                logger.Debug(goodDB + "private void CreateOrder()" + loginIdCreate.ToString());
            }
            catch(Exception ex)
            {
                logger.Error(badDB + "private void CreateOrder()" + loginIdCreate.ToString() + ex.Message.ToString());
            }
            this.idOrder = cMO_Order.id;
        }
        
        private void CreateFolderAndFileForOrder()
        {
            string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\OrderCMO\\" + idOrder.ToString() + "\\";
            try
            {
                Directory.CreateDirectory(directory);
                logger.Debug("успешно создана папка с заявкой железа " + "CreateFolderAndFileForOrder() - 1.Directory.CreateDirectory " + loginIdCreate.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("ошибка! создания папки с заявкой железа " + "CreateFolderAndFileForOrder() - 1.Directory.CreateDirectory " + loginIdCreate.ToString() + ex.Message.ToString());
            }
            try
            {
                SaveFileToServer(directory);
                logger.Debug("успешно загружен файл в папку " + "CreateFolderAndFileForOrder() - 1.1.Directory.CreateDirectory " + loginIdCreate.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("ошибка! загружен файл в папку " + "CreateFolderAndFileForOrder() - 1.1.Directory.CreateDirectory " + loginIdCreate.ToString() + ex.Message.ToString());
            }
            CreateCMO_FileOrder();
            try
            {
                CMO_Order cMO_Order = db.CMO_Order.Find(idOrder);
                cMO_Order.folder = directory;
                db.Entry(cMO_Order).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug("успешно создана папка с заявкой железа " + "CreateFolderAndFileForOrder() - 3.db.SaveChanges() " + loginIdCreate.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("ошибка! создания папки с заявкой железа " + "CreateFolderAndFileForOrder() - 3.db.SaveChanges() " + loginIdCreate.ToString() + ex.Message.ToString());
            }
        }

        private void CreateCMO_FileOrder()
        {
            CMO_FileOrder cMO_FileOrder = new CMO_FileOrder();
            try
            {
                for (int i = 0; i < fileUploadArray.Length; i++)
                {
                    cMO_FileOrder.filname = fileUploadArray[i].FileName;
                    cMO_FileOrder.id_CMO_Order = idOrder;
                    db.CMO_FileOrder.Add(cMO_FileOrder);
                    db.SaveChanges();
                    logger.Debug(goodDB + "2. CreateCMO_FileOrder() " + loginIdCreate.ToString());
                }
            }
            catch(Exception ex)
            {
                logger.Error(badDB + "CreateCMO_FileOrder()" + loginIdCreate.ToString() + ex.Message.ToString());
            }
        }

        private void CreateCMO_PositionOrder()
        {
            for (int i = 0; i < pZiDArray.Length; i++)
            {
                string folderPz = GetAddressFolder(pZiDArray[i]);
                SaveFileToServer(folderPz);
                for (int j = 0; j < typeProductArray.Length; j++)
                {
                    CreatePositionOrder(folderPz, pZiDArray[i], typeProductArray[j]);
                }
            }
        }
        
        public CMO_Create(HttpPostedFileBase[] fileUploadArray, int[] pZiDArray, string loginIdCreate, CMO_Company cMO_Company)
        {
            this.fileUploadArray = fileUploadArray;
            this.pZiDArray = pZiDArray;
            this.loginIdCreate = loginIdCreate;
            CreateOrder(true, cMO_Company.id);
            CreateFolderAndFileForOrder();
            CreateCMO_PositionOrder(10);
            string bodyMail = GetBodyMailDefault();
            EmailModel emailModel = new EmailModel();
            List<string> recipientList = new List<string>();
            recipientList.Add("myi@katek.by");
            recipientList.Add("gdp@katek.by");
            recipientList.Add("Antipov@katek.by");
            recipientList.Add("vi@katek.by");
            recipientList.Add("yaa@katek.by");
            recipientList.Add("nrf@katek.by");

            foreach (var listMailCompany in db.CMO_CompanyMailList.Where(d => d.id_CMO_Company == cMO_Company.id & d.active == true).ToList())
            {
                recipientList.Add(listMailCompany.email.ToString());
            }

            string mailUploadData = db.AspNetUsers.Where(d => d.Email == loginIdCreate).First().Email;
            emailModel.SendEmail(recipientList.ToArray(), GetSubjectMailDefault(), bodyMail, GetFileArray(), mailUploadData);
            CreateCMO_Mail(4, "", bodyMail, GetFileArray());
        }

        public void CMO_StartFirstTender(DateTime houreForInf)
        {
            DateTime finisfPlan = new DateTime();
            finisfPlan = CreateCMO_Tender(1, houreForInf);
            CreateCMO_UploadResult(GetListCompany());
            PushMailToFirstTender(finisfPlan);
            UpdateOrderBeforeFirtst();
        }
        
        public void CMO_FinishFirstStartSecond(DateTime houreForInf)
        {
            DateTime finisfPlan = new DateTime();
            CloseFirstTenderInOrder();
            CloseFirstTenderInTender();
            CloseFirstTenderInUpload();
            double cost = GetMinCostTender();
            int datetimeComplited = GetMinDateComplitedTender();
            finisfPlan = CreateCMO_Tender(2, houreForInf);
            CreateCMO_UploadResult(GetListCompanySecondTender());
            PushMailToSecondTender(finisfPlan, cost, datetimeComplited);
        }
        
        public void CMO_FinishTender()
        {
            CloseWinnerTenderInOrder();
            CloseSecondTenderInTender();
            CloseFirstTenderInUpload();
            CreateCMO_Tender(3);
            CreateCMO_UploadResult();
            PushMailToWinnerTender();
        }

        private void CloseWinnerTenderInOrder()
        {
            double minData = db.CMO_UploadResult.Where(d => d.CMO_Tender.id_CMO_Order == idOrder & d.CMO_Tender.id_CMO_TypeTask == 2 & d.cost > 0).Min(d => d.cost);
            CMO_UploadResult getCMOUploadLead = db.CMO_UploadResult.Where(d => d.CMO_Tender.id_CMO_Order == idOrder & d.CMO_Tender.id_CMO_TypeTask == 2 & d.cost > 0 & d.cost == minData).First();
            CMO_Order cMO_Order = db.CMO_Order.Find(idOrder);
            cMO_Order.datetimeWinTenderFinish = DateTime.Now;
            cMO_Order.companyWin = getCMOUploadLead.id_CMO_Company;
            db.Entry(cMO_Order).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void CloseWinnerTenderInTender()
        {
            CMO_Tender cMO_Tender = db.CMO_Tender.Where(d => d.id_CMO_Order == idOrder & d.id_CMO_TypeTask == 2).First();
            cMO_Tender.close = true;
            cMO_Tender.closeDateTime = DateTime.Now;
            db.Entry(cMO_Tender).State = EntityState.Modified;
            db.SaveChanges();
            idTender = cMO_Tender.id;

        }
        
        private void CloseSecondTenderInOrder()
        {
            CMO_Order cMO_Order = db.CMO_Order.Find(idOrder);
            cMO_Order.datetimeSecondTenderFinish = DateTime.Now;
            db.Entry(cMO_Order).State = EntityState.Modified;
            db.SaveChanges();
        }
        
        private void CloseFirstTenderInOrder()
        {
            CMO_Order cMO_Order = db.CMO_Order.Find(idOrder);
            cMO_Order.datetimeFirstTenderFinish = DateTime.Now;
            db.Entry(cMO_Order).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void CloseFirstTenderInTender()
        {
            CMO_Tender cMO_Tender = db.CMO_Tender.Where(d => d.id_CMO_Order == idOrder & d.id_CMO_TypeTask == 1).First();
            cMO_Tender.close = true;
            cMO_Tender.closeDateTime = DateTime.Now;
            idTender = cMO_Tender.id;
            db.Entry(cMO_Tender).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void CloseSecondTenderInTender()
        {
            CMO_Tender cMO_Tender = db.CMO_Tender.Where(d => d.id_CMO_Order == idOrder & d.id_CMO_TypeTask == 2).First();
            cMO_Tender.close = true;
            cMO_Tender.closeDateTime = DateTime.Now;
            db.Entry(cMO_Tender).State = EntityState.Modified;
            db.SaveChanges();
            idTender = cMO_Tender.id;
        }

        private void CloseFirstTenderInUpload()
        {
            var cMO_Tender = db.CMO_UploadResult.Where(d => d.id_CMO_Tender == idTender & d.dateTimeUpload == null).ToList();
            foreach (var data in cMO_Tender)
            {
                CMO_UploadResult tenderU = db.CMO_UploadResult.Find(data.id);
                db.CMO_UploadResult.Remove(tenderU);
                db.SaveChanges();
            }
        }
        
        private void PushMailToOsOnCreateOrder()
        {
            try
            {
                string subject = "Размещен новый заказ деталей ";
                string body = "Размещаем заказ деталей\n";
                body += "Сотрудник сформировавший заявку: " + db.AspNetUsers.Where(d => d.Id == loginIdCreate).First().CiliricalName + "\n\n";
                var cmoPositionList = db.CMO_PositionOrder.Include(d => d.CMO_TypeProduct).Where(d => d.id_CMO_Order == idOrder).ToList();
                foreach (var data in cmoPositionList)
                {
                    subject += "план-заказ № " + data.PZ_PlanZakaz.PlanZakaz.ToString() + " " + data.CMO_TypeProduct.name.ToString();
                    body += "план-заказ № " + data.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + data.CMO_TypeProduct.name.ToString();
                }
                body += "\n\nПросьба сформировать заявку на первый этап торгов";
                subject += " (заявка №: " + idOrder.ToString() + ")";


                string str = "";
                str = subject.Replace("\n", " ").Replace("\r", " ");





                EmailModel emailModel = new EmailModel();
                List<string> recipientList = new List<string>();
                recipientList.Add("myi@katek.by");
                recipientList.Add("gdp@katek.by");
                recipientList.Add("Antipov@katek.by");
                recipientList.Add("vi@katek.by");
                recipientList.Add("yaa@katek.by");
                recipientList.Add("nrf@katek.by");
                //string mailUploadData = db.AspNetUsers.Where(d => d.Email == loginIdCreate).First().Email;
                emailModel.SendEmail(recipientList.ToArray(), str, body);
                logger.Debug("PushMailToOsOnCreateOrder " + str + " " + body);
            }
            catch(Exception ex)
            {
                logger.Error("PushMailToOsOnCreateOrder НЕ ОТПРАВЛЕНО" + idOrder.ToString() + ex.Message.ToString());
                EmailModel emailModel = new EmailModel();
                emailModel.SendEmailOnePerson("myi@katek.by; myi@katek.by", "Не отправлено письмо!", "PushMailToOsOnCreateOrder " + idOrder.ToString());
            }

        }

        private void UpdateOrderBeforeFirtst()
        {
            CMO_Order cMO_Order = db.CMO_Order.Find(idOrder);
            cMO_Order.firstTenderStart = true;
            db.Entry(cMO_Order).State = EntityState.Modified;
            db.SaveChanges();
        }
        
        private void PushMailToFirstTender(DateTime finisfPlan)
        {
            string bodyMail = GetBodyMailFirstTender(finisfPlan);
            EmailModel emailModel = new EmailModel();
            //emailModel.SendEmailOnePerson("myi@katek.by", GetSubjectMailFirstTender(), bodyMail, GetFileArray());
            List<string> recipientList = new List<string>();
            recipientList.Add("myi@katek.by");
            recipientList.Add("gdp@katek.by");
            recipientList.Add("Antipov@katek.by");
            recipientList.Add("katek@armis.by");
            recipientList.Add("gratius@yandex.ru");
            recipientList.Add("ecowood @tut.by");
            emailModel.SendEmail(recipientList.ToArray(), GetSubjectMailFirstTender(), bodyMail, GetFileArray(), "gdp@katek.by");
            CreateCMO_Mail(1, "myi@katek.by", bodyMail, GetFileArray());
        }
        
        private void PushMailToSecondTender(DateTime finisfPlan, double cost, int datetimeComplited)
        {
            string bodyMail = GetBodyMailSecondTender(finisfPlan, cost, datetimeComplited);
            EmailModel emailModel = new EmailModel();

            List<string> recipientList = new List<string>();
            recipientList.Add("myi@katek.by");
            recipientList.Add("gdp@katek.by");
            recipientList.Add("Antipov@katek.by");
            recipientList.Add("katek@armis.by");
            recipientList.Add("gratius@yandex.ru");
            recipientList.Add("ecowood @tut.by");
            emailModel.SendEmail(recipientList.ToArray(), GetSubjectMailFirstTender(), bodyMail, GetFileArray(), "gdp@katek.by");
            CreateCMO_Mail(2, "myi@katek.by", bodyMail);
        }

        private void PushMailToWinnerTender()
        {
            string bodyMail = GetBodyMailWinnerTender();
            EmailModel emailModel = new EmailModel();
            List<string> recipientList = new List<string>();
            recipientList.Add("myi@katek.by");
            recipientList.Add("gdp@katek.by");
            recipientList.Add("Antipov@katek.by");
            recipientList.Add("katek@armis.by");
            recipientList.Add("gratius@yandex.ru");
            recipientList.Add("ecowood @tut.by");
            emailModel.SendEmail(recipientList.ToArray(), GetSubjectMailFirstTender(), bodyMail, GetFileArray(), "gdp@katek.by");
            CreateCMO_Mail(3, "myi@katek.by", bodyMail);
        }

        private double GetMinCostTender()
        {
            double cost = db.CMO_UploadResult.Where(d => d.id_CMO_Tender == idTender & d.dateTimeUpload != null).Min(d => d.cost);
            return cost;
        }

        private int GetMinDateComplitedTender()
        {
            int cost = (int)db.CMO_UploadResult.Where(d => d.id_CMO_Tender == idTender & d.dateTimeUpload != null).Min(d => d.day);
            return cost;
        }

        private string GetBodyMailDefault()
        {
            string body = "Добрый день!\nРазмещаем дозаказ деталей № :" + idOrder.ToString();
            var cmoPositionList = db.CMO_PositionOrder.Where(d => d.id_CMO_Order == idOrder).ToList();
            body += "\n";
            foreach (var data in cmoPositionList)
            {
                body += "план-заказ № " + data.PZ_PlanZakaz.PlanZakaz.ToString();
            }
            return body;
        }

        private string GetBodyMailFirstTender(DateTime finisfPlan)
        {
            string body = "Добрый день!\nРазмещаем заказ деталей № :" + idOrder.ToString() + "\n\nПрошу прислать КП на:\n";
            var cmoPositionList = db.CMO_PositionOrder.Where(d => d.id_CMO_Order == idOrder).ToList();
            foreach (var data in cmoPositionList)
            {
                body += "план-заказ № " + data.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + data.CMO_TypeProduct.name.ToString();
            }
            body += "\n\n" + "Крайний срок предоставления данных: " + finisfPlan.ToString().Substring(0, 16);
            body += "\n\n\nС уважением,\nГришель Дмитрий Петрович\nНачальник отдела по материально - техническому снабжению\n" +
                    "Тел:  +375 17 366 90 67(вн. 329)\nМоб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35\nSkype: sitek_dima\n" +
                    "gdp@katek.by";
            return body;
        }

        private string GetSubjectMailFirstTender()
        {
            string subject = "Заказ деталей: ";
            var cmoPositionList = db.CMO_PositionOrder.Where(d => d.id_CMO_Order == idOrder).ToList();
            foreach (var data in cmoPositionList)
            {
                subject += "план-заказ № " + data.PZ_PlanZakaz.PlanZakaz.ToString() + " " + data.CMO_TypeProduct.name.ToString();
            }
            subject = " (заявка № " + idOrder.ToString() + ")";

            string str = "";
            str = subject.Replace("\n", " ").Replace("\r", " ");




            return str;
        }

        public void TestDebug()
        {
            string bodyMail = GetBodyMailFirstTender(DateTime.Now);
            EmailModel emailModel = new EmailModel();
            List<string> recipientList = new List<string>();
            recipientList.Add("myi@katek.by");

            string subject = "Заказ деталей: ";
            var cmoPositionList = db.CMO_PositionOrder.Where(d => d.id_CMO_Order == idOrder).ToList();
            foreach (var data in cmoPositionList)
            {
                subject += "план-заказ № " + data.PZ_PlanZakaz.PlanZakaz.ToString() + " " + data.CMO_TypeProduct.name.ToString();
            }




            emailModel.SendEmail(recipientList.ToArray(), subject, bodyMail, GetFileArray(), "gdp@katek.by");


        }





        private string GetSubjectMailDefault()
        {
            string subject = "Дозаказ деталей № " + idOrder.ToString();
            return subject;
        }

        private string GetBodyMailSecondTender(DateTime finisfPlan, double cost, int dateComplited)
        {
            string body = "Добрый день!\nПо итогам торгов стоимость заказа (№ " + idOrder.ToString() + "): " + cost.ToString() + " б.р. б/ндс, срок изготовления: " + dateComplited.ToString() + "дн." + "\n";
            body += "\n\n" + "Прошу прислать минимально возможную цену: " + finisfPlan.ToString().Substring(0, 16);
            body += "\n\n\n\nС уважением,\nГришель Дмитрий Петрович\nНачальник отдела по материально - техническому снабжению\n" +
                    "Тел:  +375 17 366 90 67(вн. 329)\nМоб.: МТС + 375 29 561 98 28, velcom + 375 29 350 68 35\nSkype: sitek_dima\n" +
                    "gdp@katek.by";
            return body;
        }

        private string GetBodyMailWinnerTender()
        {
            CMO_UploadResult cMO_UploadResult = db.CMO_UploadResult.Include(d => d.CMO_Company).Where(d => d.id_CMO_Tender == idTender).First();
            string body = "Добрый день!\nПо итогам торгов победил: " + cMO_UploadResult.CMO_Company.name + "\n\nСтоимость заказа " + cMO_UploadResult.cost.ToString() + " б.р. б/ндс, срок изготовления " + cMO_UploadResult.day.ToString() + "дн.";
            return body;
        }
        
        private void CreateCMO_PositionOrder(int x)
        {
            for (int i = 0; i < pZiDArray.Length; i++)
            {
                string folderPz = GetAddressFolder(pZiDArray[i]);
                SaveFileToServer(folderPz);
                CreatePositionOrder(folderPz, pZiDArray[i]);
            }
        }
        
        private void CreateOrder(bool bit, int winCompanyId)
        {
            CMO_Order cMO_Order = new CMO_Order();
            cMO_Order.companyWin = winCompanyId;
            cMO_Order.dateCloseOrder = DateTime.Now;
            cMO_Order.idTime = 1;
            cMO_Order.dateCreate = DateTime.Now;
            cMO_Order.userCreate = this.loginIdCreate;
            cMO_Order.firstTenderStart = true;
            db.CMO_Order.Add(cMO_Order);
            db.SaveChanges();
            this.idOrder = cMO_Order.id;
        }

        private void CreatePositionOrder(string folderPz, int idPz, int type)
        {
            CMO_PositionOrder cMO_PositionOrder = new CMO_PositionOrder();
            cMO_PositionOrder.id_CMO_Order = this.idOrder;
            cMO_PositionOrder.id_PZ_PlanZakaz = idPz;
            cMO_PositionOrder.id_CMO_TypeProduct = type;
            cMO_PositionOrder.folder = folderPz;
            db.CMO_PositionOrder.Add(cMO_PositionOrder);
            db.SaveChanges();
        }

        private void CreatePositionOrder(string folderPz, int idPz)
        {
            CMO_PositionOrder cMO_PositionOrder = new CMO_PositionOrder();
            cMO_PositionOrder.id_CMO_Order = this.idOrder;
            cMO_PositionOrder.id_PZ_PlanZakaz = idPz;
            cMO_PositionOrder.id_CMO_TypeProduct = 10;
            cMO_PositionOrder.folder = folderPz;
            db.CMO_PositionOrder.Add(cMO_PositionOrder);
            db.SaveChanges();
        }

        private string GetAddressFolder(int pz)
        {
            string adress;
            adress = db.PZ_PlanZakaz.Find(pz).Folder + "\\12_КД\\Заказ железа\\";
            return adress;
        }

        private void SaveFileToServer(string folderAdress)
        {
            for (int i = 0; i < fileUploadArray.Length; i++)
            {
                try
                {
                    string fileReplace = System.IO.Path.GetFileName(fileUploadArray[i].FileName);
                    fileReplace = ToSafeFileName(fileReplace);
                    var fileName = string.Format("{0}\\{1}", folderAdress, fileReplace);
                    fileUploadArray[i].SaveAs(fileName);
                }
                catch(Exception ex)
                {
                    logger.Error(badDB + "1/1/1. SaveFileToServer(string folderAdress) " + ex.Message.ToString());
                }
            }
        }

        private string ToSafeFileName(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
        
        private void CreateCMO_Tender(int id_CMO_TypeTask)
        {
            double minData = db.CMO_UploadResult.Where(d => d.CMO_Tender.id_CMO_Order == idOrder & d.CMO_Tender.id_CMO_TypeTask == 2 & d.cost > 0).Min(d => d.cost);
            CMO_UploadResult getCMOUploadLead = db.CMO_UploadResult.Where(d => d.CMO_Tender.id_CMO_Order == idOrder & d.CMO_Tender.id_CMO_TypeTask == 2 & d.cost > 0 & d.cost == minData).First();

            CMO_Tender cMO_Tender = new CMO_Tender();
            cMO_Tender.id_CMO_Order = idOrder;
            cMO_Tender.id_CMO_TypeTask = id_CMO_TypeTask;
            cMO_Tender.close = true;
            cMO_Tender.finishPlanClose = Convert.ToDateTime(getCMOUploadLead.dateComplited);

            db.CMO_Tender.Add(cMO_Tender);
            db.SaveChanges();
            idTender = cMO_Tender.id;
        }

        private DateTime CreateCMO_Tender(int id_CMO_TypeTask, DateTime addHoure)
        {
            CMO_Tender cMO_Tender = new CMO_Tender();
            cMO_Tender.id_CMO_Order = idOrder;
            cMO_Tender.id_CMO_TypeTask = id_CMO_TypeTask;
            cMO_Tender.finishPlanClose = addHoure;
            //cMO_Tender.finishPlanClose = GetFinishPlanDate(addHoure);
            cMO_Tender.close = false;
            db.CMO_Tender.Add(cMO_Tender);
            db.SaveChanges();
            this.idTender = cMO_Tender.id;
            return cMO_Tender.finishPlanClose;
        }
        
        private DateTime GetFinishPlanDate(int addHoure)
        {
            DateTime finishPlan = DateTime.Now.AddHours(addHoure);
            if (finishPlan.Hour > 17)
                finishPlan = new DateTime(finishPlan.Year, finishPlan.Month, finishPlan.Day, 17, 30, 00);
            if (finishPlan.Hour < 8)
                finishPlan = new DateTime(finishPlan.Year, finishPlan.Month, finishPlan.Day, 8, 30, 00);
            return finishPlan;
        }

        private DateTime CreateCMO_UploadResult()
        {
            double minData = db.CMO_UploadResult.Where(d => d.CMO_Tender.id_CMO_Order == idOrder & d.CMO_Tender.id_CMO_TypeTask == 2 & d.cost > 0).Min(d => d.cost);
            CMO_UploadResult getCMOUploadLead = db.CMO_UploadResult.Where(d => d.CMO_Tender.id_CMO_Order == idOrder & d.CMO_Tender.id_CMO_TypeTask == 2 & d.cost > 0 & d.cost == minData).First();

            CMO_UploadResult cMO_UploadResult = new CMO_UploadResult();
            cMO_UploadResult.id_CMO_Tender = idTender;
            cMO_UploadResult.dateTimeUpload = DateTime.Now;
            cMO_UploadResult.cost = getCMOUploadLead.cost;
            cMO_UploadResult.dateComplited = DateTime.Now.AddDays((int)getCMOUploadLead.day);
            cMO_UploadResult.day = getCMOUploadLead.day;
            cMO_UploadResult.id_CMO_Company = getCMOUploadLead.id_CMO_Company;
            db.CMO_UploadResult.Add(cMO_UploadResult);
            db.SaveChanges();
            return Convert.ToDateTime(cMO_UploadResult.dateComplited);
        }

        private void CreateCMO_UploadResult(List <int> companyList)
        {
            foreach(var data in companyList)
            {
                CMO_UploadResult cMO_UploadResult = new CMO_UploadResult();
                cMO_UploadResult.id_CMO_Tender = idTender;
                cMO_UploadResult.cost = 0;
                cMO_UploadResult.id_CMO_Company = data;
                db.CMO_UploadResult.Add(cMO_UploadResult);
                db.SaveChanges();
            }
        }
        
        private List <int> GetListCompany()
        {
            List<int> listCompany = new List<int>();
            foreach (var data in db.CMO_Company.Where(d => d.active == true))
            {
                listCompany.Add(data.id);
            }
            return listCompany;
        }

        private List<int> GetListCompanySecondTender()
        {
            List<int> listCompany = new List<int>();
            int idTender = db.CMO_Tender.Where(d => d.id_CMO_Order == idOrder & d.id_CMO_TypeTask == 1).First().id;
            var listUpload = db.CMO_UploadResult.Where(d => d.dateTimeUpload != null & d.id_CMO_Tender == idTender).ToList();

            foreach (var data in listUpload)
            {
                listCompany.Add(data.id_CMO_Company);
            }
            return listCompany;
        }

        private void CreateCMO_Mail(int id_TypeTask, string email, string body)
        {
            CMO_Mail cMO_Mail = new CMO_Mail();
            cMO_Mail.id_CMO_Order = idOrder;
            cMO_Mail.id_CMO_TypeTask = id_TypeTask;
            cMO_Mail.dateTimePush = DateTime.Now;
            cMO_Mail.email = email;
            cMO_Mail.body = body;
            cMO_Mail.listFileName = "";
            db.CMO_Mail.Add(cMO_Mail);
            db.SaveChanges();
        }

        private void CreateCMO_Mail(int id_TypeTask, string email, string body, List<string> attachment)
        {
            CMO_Mail cMO_Mail = new CMO_Mail();
            cMO_Mail.id_CMO_Order = idOrder;
            cMO_Mail.id_CMO_TypeTask = id_TypeTask;
            cMO_Mail.dateTimePush = DateTime.Now;
            cMO_Mail.email = email;
            cMO_Mail.body = body;
            foreach (var data in attachment)
            {
                cMO_Mail.listFileName += data;
            }
            db.CMO_Mail.Add(cMO_Mail);
            db.SaveChanges();
        }

        private List <string> GetFileArray()
        {
            CMO_Order cMO_Order = db.CMO_Order.Find(idOrder);
            var fileList = Directory.GetFiles(cMO_Order.folder).ToList();
            return fileList;
        }
    }
}