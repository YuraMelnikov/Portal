using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ProjectServer.Client;

using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.Entity;
using Microsoft.SharePoint.Client;

namespace Wiki.Models
{
    public class RKD
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string goodConstructor = "успешная передача/присвоение данных конструктора ";
        private string badConstructor = "ошибка передача/присвоение данных конструктора ";
        private string goodMethod = " метод отработал успешно ";
        private string badMethod = " метод завершился ошибкой ";
        private string badDB = "ошибка сохранениея даных на сервере ";
        private PortalKATEKEntities db = new PortalKATEKEntities();

        private int id_PZ_PlaznZakaz;
        private int id_RKD_Order;
        private Guid projectUID;

        public RKD(int id_PZ_PlznZakaz)
        {
            this.id_PZ_PlaznZakaz = id_PZ_PlznZakaz;
        }

        // Create order before create new PZ
        public void CreateRKDOrder()
        {
            try
            {
                RKD_Order rKD_Order = new RKD_Order();
                rKD_Order.id_PZ_PlanZakaz = id_PZ_PlaznZakaz;
                rKD_Order.id_RKD_Institute = 1;
                rKD_Order.datePlanCritical = DateTime.Now.AddDays(34);
                db.RKD_Order.Add(rKD_Order);
                db.SaveChanges();
                CreateTasks();
                CreateNULLVersion();
                Create_NULLRKD_TaskVersion();
                //UpdateTasksInProject();
                GetRKDDump();
                logger.Debug(goodConstructor + " (Modul RKD - CreateRKDOrder()");
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - CreateRKDOrder(); " + ex.Message.ToString());
            }
        }

        //1
        public void UploadNewVersionRDK(HttpPostedFileBase[] fileUpload, RKD_Order rKD_Order, string num, int versionDay, string login)
        {
            try
            {
                int test1 = num.IndexOf('.');
                int test2 = num.Length;



                short num1 = (short)Convert.ToInt32(num.Substring(0, num.IndexOf('.')));
                short num2 = (short)Convert.ToInt32(num.Substring(num.IndexOf('.') + 1, num.Length - num.IndexOf('.') - 1));
                int id_lastVersion = db.RKD_Version.Where(d => d.id_RKD_Order == rKD_Order.id & d.activeVersion == true).First().id;
                DeactiveAllVersion();
                RKD_Version rKD_Version = new RKD_Version();
                rKD_Version.id_RKD_VersionWork = 2;
                rKD_Version.id_RKD_Order = rKD_Order.id;
                rKD_Version.numberVersion1 = num1;
                rKD_Version.numberVersion2 = num2;
                rKD_Version.id_RKD_VersionDay = db.RKD_VersionDay.Find(versionDay).id;
                rKD_Version.activeVersion = true;
                db.RKD_Version.Add(rKD_Version);
                db.SaveChanges();
                CreateFolderSaveFile(fileUpload, 1, login, rKD_Version.id);
                PushMail(fileUpload, 1);
                CreateTaskVersion(id_lastVersion, rKD_Version.id, login);
                UpdateTaskUID();
                GetRKDDump();
                logger.Debug(goodMethod + " (Modul RKD - UploadNewVersionRDK(HttpPostedFileBase[] fileUpload, RKD_Order rKD_Order, int num1, int num2, int version, string login)");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - UploadNewVersionRDK(HttpPostedFileBase[] fileUpload, RKD_Order rKD_Order, int num1, int num2, int version, string login); " + ex.Message.ToString());
            }
        }
        
        //2
        public void ErrorNewVersionRDK(int id_Order, string login)
        {
            try
            {
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 4;
                db.Entry(rKD_Version).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                PushMailNotFile(2);
                CreateRKD_Mail_Version(rKD_Version.id, 2, login);
                logger.Debug(goodMethod + " (Modul RKD - ErrorNewVersionRDK(int id_Order, string login) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - ErrorNewVersionRDK(int id_Order, string login) " + ex.Message.ToString());
            }
        }

        //3
        public void UploadVersionRDK(HttpPostedFileBase[] fileUpload, RKD_Order rKD_Order, int num1, int num2, int version, string login)
        {
            try
            {
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == rKD_Order.id).First();
                rKD_Version.id_RKD_VersionWork = 2;
                db.Entry(rKD_Version).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                CreateFolderSaveFile(fileUpload, 2, login, rKD_Version.id);
                PushMail(fileUpload, 2);
                logger.Debug(goodMethod + " (Modul RKD - UploadVersionRDK(HttpPostedFileBase[] fileUpload, RKD_Order rKD_Order, int num1, int num2, int version, string login) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - UploadVersionRDK(HttpPostedFileBase[] fileUpload, RKD_Order rKD_Order, int num1, int num2, int version, string login) " + ex.Message.ToString());
            }
        }

        //4
        public void UploadClient(int id_Order, string login, int day, HttpPostedFileBase[] fileUpload)
        {
            try
            {
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 5;
                db.Entry(rKD_Version).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                CreateFolderSaveFile(fileUpload, 4, login, rKD_Version.id);
                PushMailNotFile(4);
                CreateRKD_Mail_Version(rKD_Version.id, 4, login, day);
                logger.Debug(goodMethod + " (Modul RKD - UploadClient(int id_Order, string login, int day)) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - UploadClient(int id_Order, string login, int day)) " + ex.Message.ToString());
            }
        }

        //6
        public void PlusClientDay(int id_Order, string login, int day)
        {
            try
            {
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 5;
                db.Entry(rKD_Version).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                PushMailNotFile(6);
                CreateRKD_Mail_Version(rKD_Version.id, 6, login, day);
                logger.Debug(goodMethod + " (Modul RKD - UploadClient(int id_Order, string login, int day)) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - UploadClient(int id_Order, string login, int day)) " + ex.Message.ToString());
            }
        }

        //7
        public void GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)
        {
            try
            {
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 7;
                db.Entry(rKD_Version).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                CreateFolderSaveFile(fileUpload, 7, login, rKD_Version.id);
                PushMail(fileUpload, 7);
                CreateRKD_Mail_Version(rKD_Version.id, 7, login);
                logger.Debug(goodMethod + " (Modul RKD - GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)) " + ex.Message.ToString());
            }
        }

        //8
        public void GetGoodList(int id_Order, string login, HttpPostedFileBase[] fileUpload)
        {
            try
            {
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 10;
                db.Entry(rKD_Version).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                CreateFolderSaveFile(fileUpload, 8, login, rKD_Version.id);
                PushMail(fileUpload, 8);
                CreateRKD_Mail_Version(rKD_Version.id, 8, login);
                logger.Debug(goodMethod + " (Modul RKD - GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)) " + ex.Message.ToString());
            }
        }

        private void CreateTaskVersion(int id_lastVersion, int idVersion, string login)
        {
            try
            {
                var listTaskVersion = db.RKD_TaskVersion.Where(d => d.id_RKD_Version == id_lastVersion).ToList();
                foreach (var data in listTaskVersion)
                {
                    RKD_TaskVersion rKD_Version = new RKD_TaskVersion();
                    rKD_Version.id_RKD_Version = idVersion;
                    rKD_Version.id_RKD_Task = data.id_RKD_Task;
                    rKD_Version.finishDate = data.finishDate;
                    rKD_Version.tpComplited = data.tpComplited;
                    rKD_Version.allComplited = data.allComplited;
                    db.RKD_TaskVersion.Add(rKD_Version);
                    db.SaveChanges();
                    RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                    rKD_HistoryTaskVersion.id_RKD_TaskVersion = idVersion;
                    rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                    rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                    rKD_HistoryTaskVersion.finishDate = data.finishDate;
                    rKD_HistoryTaskVersion.tpComplited = data.tpComplited;
                    rKD_HistoryTaskVersion.allComplited = data.allComplited;
                    db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                    db.SaveChanges();
                }
                logger.Debug(goodMethod + " (Modul RKD - CreateTaskVersion(int id_lastVersion, int idVersion)");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - CreateTaskVersion(int id_lastVersion, int idVersion); " + ex.Message.ToString());
            }
        }

        private void CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version, string login, int id_rKD_Version)
        {
            try
            {
                string directory = db.Folder.First().adres.ToString() + "\\_ЗАКАЗЫ\\" + db.PZ_PlanZakaz.Find(id_PZ_PlaznZakaz).Folder + "\\05_РКД\\Переписка\\";
                directory += DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_";
                if (DateTime.Now.Month.ToString().Length == 1)
                    directory += "0" + DateTime.Now.Day.ToString();
                else
                    directory += DateTime.Now.Day.ToString();
                directory += "v." + db.RKD_Version.Find(id_rKD_Version).numberVersion1.ToString() + "." + db.RKD_Version.Find(id_rKD_Version).numberVersion2.ToString();
                directory += " " + db.TypeRKD_Mail_Version.Find(id_TypeRKD_Mail_Version).name;
                try
                {
                    Directory.CreateDirectory(directory);
                    logger.Debug(goodMethod + " 1. CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version)");
                }
                catch (Exception ex)
                {
                    logger.Error(badMethod + " 1. CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version) " + ex.Message.ToString());
                }

                try
                {
                    SaveFileToServer(fileUpload, directory);
                    logger.Debug(goodMethod + " 2. CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version)");
                }
                catch (Exception ex)
                {
                    logger.Error(badMethod + " 2. CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version) " + ex.Message.ToString());
                }
                try
                {
                    CreateRKD_Mail_Version(id_rKD_Version, id_TypeRKD_Mail_Version, directory, login);
                    logger.Debug(goodMethod + " 3. CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version)");
                }
                catch (Exception ex)
                {
                    logger.Error(badMethod + " 3. CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version) " + ex.Message.ToString());
                }
                logger.Debug(goodMethod + " (Modul RKD - CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version)");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version) " + ex.Message.ToString());
            }
        }

        private void CreateRKD_Mail_Version(int id_Version, int id_type, string login)
        {
            try
            {
                RKD_Mail_Version rKD_Mail_Version = new RKD_Mail_Version();
                rKD_Mail_Version.id_RKD_Version = id_Version;
                rKD_Mail_Version.id_TypeRKD_Mail_Version = id_type;
                rKD_Mail_Version.dateTimeUpload = DateTime.Now;
                rKD_Mail_Version.linkFile = "";
                rKD_Mail_Version.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                db.RKD_Mail_Version.Add(rKD_Mail_Version);
                db.SaveChanges();
                logger.Debug(goodMethod + " 3. CreateRKD_Mail_Version(int id_Version, int id_type, string directory)");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " 3. CreateRKD_Mail_Version(int id_Version, int id_type, string directory) " + ex.Message.ToString());
            }
        }

        private void CreateRKD_Mail_Version(int id_Version, int id_type, string login, int day)
        {
            try
            {
                RKD_Mail_Version rKD_Mail_Version = new RKD_Mail_Version();
                rKD_Mail_Version.id_RKD_Version = id_Version;
                rKD_Mail_Version.id_TypeRKD_Mail_Version = id_type;
                rKD_Mail_Version.dateTimeUpload = DateTime.Now;
                rKD_Mail_Version.linkFile = "";
                rKD_Mail_Version.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                db.RKD_Mail_Version.Add(rKD_Mail_Version);
                db.SaveChanges();
                RKD_Mail_TimeForComplited rKD_Mail_TimeForComplited = new RKD_Mail_TimeForComplited();
                rKD_Mail_TimeForComplited.id_RKD_Mail_Version = rKD_Mail_Version.id;
                rKD_Mail_TimeForComplited.day = day;
                db.RKD_Mail_TimeForComplited.Add(rKD_Mail_TimeForComplited);
                db.SaveChanges();
                logger.Debug(goodMethod + " CreateRKD_Mail_Version(int id_Version, int id_type, string login, int day)");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " CreateRKD_Mail_Version(int id_Version, int id_type, string login, int day) " + ex.Message.ToString());
            }
        }

        private void CreateRKD_Mail_Version(int id_Version, int id_type, string directory, string login)
        {
            try
            {
                RKD_Mail_Version rKD_Mail_Version = new RKD_Mail_Version();
                rKD_Mail_Version.id_RKD_Version = id_Version;
                rKD_Mail_Version.id_TypeRKD_Mail_Version = id_type;
                rKD_Mail_Version.dateTimeUpload = DateTime.Now;
                rKD_Mail_Version.linkFile = directory;
                rKD_Mail_Version.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                db.RKD_Mail_Version.Add(rKD_Mail_Version);
                db.SaveChanges();
                logger.Debug(goodMethod + " 3. CreateRKD_Mail_Version(int id_Version, int id_type, string directory)");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " 3. CreateRKD_Mail_Version(int id_Version, int id_type, string directory) " + ex.Message.ToString());
            }
        }

        private void SaveFileToServer(HttpPostedFileBase[] fileUpload, string directory)
        {
            for (int i = 0; i < fileUpload.Length; i++)
            {
                try
                {
                    string fileReplace = System.IO.Path.GetFileName(fileUpload[i].FileName);
                    fileReplace = ToSafeFileName(fileReplace);
                    var fileName = string.Format("{0}\\{1}", directory, fileReplace);
                    fileUpload[i].SaveAs(fileName);
                    logger.Debug(goodMethod + " SaveFileToServer(HttpPostedFileBase[] fileUpload, string folderAdress)");
                }
                catch (Exception ex)
                {
                    logger.Error(badDB + " SaveFileToServer(HttpPostedFileBase[] fileUpload, string folderAdress) " + ex.Message.ToString());
                }
            }
        }

        private void PushMailNotFile(int id_TypeRKD_Mail_Version)
        {
            string subject = "";
            string body = "";
            subject += "Заказ №: " + db.PZ_PlanZakaz.Find(id_PZ_PlaznZakaz).PlanZakaz.ToString() + " - ";
            subject += db.TypeRKD_Mail_Version.Find(id_TypeRKD_Mail_Version).name.ToString();
            body += subject;
            EmailModel emailModel = new EmailModel();
            List<string> recipientList = new List<string>();
            foreach (var data in db.RKD_PostList.Where(d => d.id_TypeRKD_Mail_Version == id_TypeRKD_Mail_Version).ToList())
            {
                recipientList.Add(data.email);
            }
            emailModel.SendEmail(recipientList.ToArray(), subject, body);
        }

        private void PushMail(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version)
        {
            string subject = "";
            string body = "";
            subject += "Заказ №: " + db.PZ_PlanZakaz.Find(id_PZ_PlaznZakaz).PlanZakaz.ToString() + " - ";
            subject += db.TypeRKD_Mail_Version.Find(id_TypeRKD_Mail_Version).name.ToString();
            body += subject;
            EmailModel emailModel = new EmailModel();
            List<string> recipientList = new List<string>();
            foreach (var data in db.RKD_PostList.Where(d => d.id_TypeRKD_Mail_Version == id_TypeRKD_Mail_Version).ToList())
            {
                recipientList.Add(data.email);
            }
            emailModel.SendEmail(recipientList.ToArray(), subject, body, fileUpload);
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

        private void DeactiveAllVersion()
        {
            try
            {
                int findIdOrder = db.RKD_Order.Where(d => d.id_PZ_PlanZakaz == id_PZ_PlaznZakaz).First().id;
                var listUpdate = db.RKD_Version.Where(d => d.id_RKD_Order == findIdOrder & d.activeVersion == true).ToList();
                if (listUpdate != null)
                {
                    foreach (var data in listUpdate)
                    {
                        data.activeVersion = false;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                logger.Debug(goodMethod + " (Modul RKD - DeactiveAllVersion()");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - DeactiveAllVersion() " + ex.Message.ToString());
            }
        }

        private void Create_NULLRKD_TaskVersion()
        {
            try
            {
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true).First();
                var taskList = db.RKD_Task.Where(d => d.id_RKD_Order == id_RKD_Order).ToList();
                foreach (var data in taskList)
                {
                    RKD_TaskVersion rKD_TaskVersion = new RKD_TaskVersion();
                    rKD_TaskVersion.id_RKD_Task = data.id;
                    rKD_TaskVersion.id_RKD_Version = rKD_Version.id;
                    rKD_TaskVersion.finishDate = DateTime.Now.AddDays(34); // вот тут нужно подумать!
                    rKD_TaskVersion.tpComplited = false;
                    rKD_TaskVersion.allComplited = false;
                    db.RKD_TaskVersion.Add(rKD_TaskVersion);
                    db.SaveChanges();
                    RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                    rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                    rKD_HistoryTaskVersion.id_AspNetUser_Upload = "8dbd0ecb-e88c-47b8-951b-3f1b5ea10cde";
                    rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                    rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                    rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                    rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                    db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                    db.SaveChanges();
                }
                logger.Debug(goodConstructor + " (Modul RKD - Create_NULLRKD_TaskVersion()");
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - Create_NULLRKD_TaskVersion(); " + ex.Message.ToString());
            }
        }

        private void GetId_RKD_Order()
        {
            try
            {
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Where(d => d.Id == id_PZ_PlaznZakaz).First();
                id_RKD_Order = db.RKD_Order.Where(d => d.id_PZ_PlanZakaz == id_PZ_PlaznZakaz).First().id;
                projectUID = pZ_PlanZakaz.ProjectUID.Value;
                logger.Debug(goodConstructor + " (Modul RKD - GetId_RKD_Order()");
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - GetId_RKD_Order(); " + ex.Message.ToString());
            }
        }

        private void UpdateTaskUID()
        {
            try
            {
                var projectTaskList = db.RKD_TaskUIDProjectUD.Where(d => d.ProjectUID == projectUID).ToList();
                foreach (var dataProject in projectTaskList)
                {
                    RKD_Task rKD_Task = db.RKD_Task.Where(d => d.id_RKD_Order == id_RKD_Order & d.id_RKD_TypeTask == dataProject.RKD).First();
                    if (rKD_Task != null)
                    {
                        rKD_Task.UID_Task = dataProject.TaskUID;
                        db.Entry(rKD_Task).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                logger.Debug(goodConstructor + " (Modul RKD - UpdateTaskUID()");
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - UpdateTaskUID(); " + ex.Message.ToString());
            }
        }

        public void CreateTasks()
        {
            try
            {
                GetId_RKD_Order();
                var defaultTaskList = db.RKD_TypeTask.Where(d => d.active == true).ToList();
                foreach (var data in defaultTaskList)
                {
                    RKD_Task rKD_Task = new RKD_Task();
                    rKD_Task.id_RKD_Order = id_RKD_Order;
                    rKD_Task.id_RKD_TypeTask = data.id;
                    db.RKD_Task.Add(rKD_Task);
                    db.SaveChanges();
                }
                UpdateTaskUID();
                logger.Debug(goodConstructor + " (Modul RKD - CreateTasks()");
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - CreateTasks(); " + ex.Message.ToString());
            }
        }

        private void CreateNULLVersion()
        {
            try
            {
                RKD_Version rKD_Version = new RKD_Version();
                rKD_Version.id_RKD_Order = id_RKD_Order;
                rKD_Version.activeVersion = true;
                rKD_Version.numberVersion1 = 0;
                rKD_Version.numberVersion2 = 0;
                rKD_Version.id_RKD_VersionDay = 1;
                rKD_Version.id_RKD_VersionWork = 12;
                db.RKD_Version.Add(rKD_Version);
                db.SaveChanges();
                logger.Debug(goodMethod + " (Modul RKD - CreateNULLVersion()");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - CreateNULLVersion(); " + ex.Message.ToString());
            }
        }

        public RKD(HttpPostedFileBase[] fileExcel, string path)
        {
            try
            {
                ImportExcelData(fileExcel, path);
                logger.Debug(goodConstructor + " (Modul RKD - RKD(HttpPostedFileBase[] fileExcel)");
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - RKD(HttpPostedFileBase[] fileExcel); " + ex.Message.ToString());
            }

        }

        private void ImportExcelData(HttpPostedFileBase[] fileExcel, string path)
        {
            try
            {
                fileExcel[0].SaveAs(path);
                Excel.Application ObjWorkExcel = new Excel.Application();
                Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(path);
                Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
                var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
                int getColumn = 0;
                string data = "";
                bool l = false;
                for (int i = 3; l == false; i++)
                {
                    data = ObjWorkSheet.Cells[4, i].Text.ToString();

                    if (data == "")
                    {
                        getColumn = i - 1;
                        l = true;
                    }
                }
                List<Despatching> listData = new List<Despatching>();
                try
                {
                    for (int i = 6; i <= lastCell.Row; i++)
                    {
                        listData.Add(new Despatching(Convert.ToInt32(ObjWorkSheet.Cells[i, 2].Text.ToString().Substring(0, 4)), ObjWorkSheet.Cells[i, getColumn].Text.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    string errror = "";
                    errror = ex.ToString();
                    errror += "";
                    ObjWorkBook.Close(false, Type.Missing, Type.Missing);
                    ObjWorkExcel.Quit();
                    GC.Collect();
                }
                DateTime dateProtocol = GetDateDespatchingProtocol(ObjWorkSheet.Cells[4, getColumn].Text.ToString());
                ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
                ObjWorkExcel.Quit(); // выйти из экселя
                GC.Collect(); // убрать за собой
                foreach (var dataList in listData)
                {
                    CreateDespatching(dataList.NumberPlanZakaz, dataList.DataText, dateProtocol);
                }
                ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
                ObjWorkExcel.Quit(); // выйти из экселя
                GC.Collect(); // убрать за собой
                logger.Debug(goodConstructor + " Modul RKD - ImportExcelData(HttpPostedFileBase[] fileExcel, string path)");
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - ImportExcelData(HttpPostedFileBase[] fileExcel, string path) " + ex.Message.ToString());
            }
        }

        private void CreateDespatching(int planZakaz, string dataText, DateTime dateProtocol)
        {
            try
            {
                RKD_Despatching rKD_Despatching = new RKD_Despatching();
                rKD_Despatching.id_RKD_Order = db.RKD_Order.Where(d => d.PZ_PlanZakaz.PlanZakaz == planZakaz).First().id;
                rKD_Despatching.dateEvent = dateProtocol;
                rKD_Despatching.text = dataText;
                db.RKD_Despatching.Add(rKD_Despatching);
                db.SaveChanges();
                logger.Debug(goodConstructor + " Modul RKD - CreateDespatching(int planZakaz, string dataText, DateTime dateProtocol)");
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - CreateDespatching(int planZakaz, string dataText, DateTime dateProtocol); " + ex.Message.ToString());
            }
        }

        private DateTime GetDateDespatchingProtocol(string data)
        {
            try
            {
                data = data.Substring(data.Length - 6, 6);
                data += DateTime.Now.Year.ToString();
                DateTime dateProtocol;
                dateProtocol = Convert.ToDateTime(data);
                logger.Debug(goodConstructor + " Modul RKD - GetDateDespatchingProtocol(string data)");
                return dateProtocol;
            }
            catch (Exception ex)
            {
                logger.Error(badConstructor + " (Modul RKD - GetDateDespatchingProtocol(string data); " + ex.Message.ToString());
                return DateTime.Now;
            }
        }

        public void UpdateTasksInProject()
        {
            ProjectContext projContext = new ProjectContext("http://tpserver/pwa/");
            var projCollection = projContext.LoadQuery(projContext.Projects.Where(p => p.Name == "Test"));
            projContext.ExecuteQuery();
            foreach (PublishedProject pubProj in projCollection)
            {
                DraftProject projCheckedOut = pubProj.CheckOut();
                TaskCreationInformation newTask = new TaskCreationInformation();
                newTask.Name = "Тестовая задача Андрея";
                newTask.IsManual = false;
                newTask.Duration = "3d";
                newTask.Start = DateTime.Today;
                projCheckedOut.Tasks.Add(newTask);
                projCheckedOut.StartDate = DateTime.Now.AddYears(1);
                projCheckedOut.Publish(true);
                QueueJob qJob = projContext.Projects.Update();
                JobState jobState = projContext.WaitForQueue(qJob, 10);
            }
        }

        public string GetNumberNewVer()
        {
            string data = "";
            RKD_Version lastVersion = db.RKD_Order.Where(d => d.id_PZ_PlanZakaz == id_PZ_PlaznZakaz).First().RKD_Version.Where(d => d.activeVersion == true).First();
            if (lastVersion.numberVersion1 == 0)
            {
                data = "1.0";
            }
            else
            {
                data = lastVersion.numberVersion1.ToString() + "." + (lastVersion.numberVersion2 + 1).ToString();
            }
            return data;
        }

        private void GetRKDDump()
        {
            try
            {
                var listTask = db.RKD_TaskVersion.Where(d => d.RKD_Version.activeVersion == true).ToList();
                foreach (var data in listTask)
                {
                    RKD_Damp rKD_Damp = new RKD_Damp();
                    rKD_Damp.publish = false;
                    rKD_Damp.dateTimeClose = DateTime.Now;
                    rKD_Damp.datetimeCreate = DateTime.Now;
                    rKD_Damp.ProjectUID = projectUID;
                    rKD_Damp.TaskUID = data.RKD_Task.UID_Task.Value;
                    rKD_Damp.date = data.finishDate;
                    db.RKD_Damp.Add(rKD_Damp);
                    db.SaveChanges();
                }
                logger.Debug(goodMethod + " (Modul RKD - GetRKDDump()");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - GetRKDDump(); " + ex.Message.ToString());
            }
        }
    }
}














//public RKD(int id_RKD_Order, string login, DateTime dateUpload)
//{
//    try
//    {
//        if(db.RKD_Version.Where(d => d.id_RKD_Order == id_RKD_Order).Count() == 0)
//            CreateNewRKD_Version(id_RKD_Order, login, dateUpload);
//        else
//            UpdateNewRKD_Version(id_RKD_Order, login, dateUpload);
//        logger.Debug(goodConstructor + " (Modul RKD -  RKD(int id_RKD_Order, string login, DateTime dateUpload, int[] typeTask)");
//    }
//    catch (Exception ex)
//    {
//        logger.Error(badConstructor + " (Modul RKD -  RKD(int id_RKD_Order, string login, DateTime dateUpload, int[] typeTask); " + ex.Message.ToString());
//    }
//}

//private void CreateNewRKD_Version(int id_RKD_Order, string login, DateTime dateUpload)
//{
//    try
//    {
//        RKD_Version rKD_Version = new RKD_Version();
//        rKD_Version.id_RKD_Order = id_RKD_Order;
//        rKD_Version.numberVersion = db.RKD_Version.Where(d => d.id_RKD_Order == id_RKD_Order).Count() + 1;
//        rKD_Version.dateUpload = DateTime.Now;
//        rKD_Version.userUpload = login;
//        rKD_Version.customdateUpload = dateUpload;
//        db.RKD_Version.Add(rKD_Version);
//        db.SaveChanges();
//        CreateAllTask(rKD_Version.id);
//        logger.Debug(goodMethod + " (Modul RKD - CreateNewRKD_Version(int id_RKD_Order)");
//    }
//    catch (Exception ex)
//    {
//        logger.Error(badMethod + " (Modul RKD - CreateNewRKD_Version(int id_RKD_Order); " + ex.Message.ToString());
//    }
//}

//private void CreateAllTask(int id_RKD_Version)
//{
//    try
//    {
//        foreach(var data in db.RKD_TypeTask.ToList())
//        {
//            RKD_Task rKD_Task = new RKD_Task();
//            rKD_Task.id_RKD_Version = id_RKD_Version;
//            rKD_Task.id_RKD_TypeTask = data.id;
//            rKD_Task.complited = false;
//            db.RKD_Task.Add(rKD_Task);
//            db.SaveChanges();
//        }
//        logger.Debug(goodMethod + " (Modul RKD - CreateAllTask(int id_RKD_Version)");
//    }
//    catch (Exception ex)
//    {
//        logger.Error(badMethod + " (Modul RKD - CreateAllTask(int id_RKD_Version); " + ex.Message.ToString());
//    }
//}

//private void UpdateNewRKD_Version(int id_RKD_Order, string login, DateTime dateUpload)
//{
//    try
//    {
//        RKD_Version rKD_Version = new RKD_Version();
//        rKD_Version.id_RKD_Order = id_RKD_Order;
//        rKD_Version.numberVersion = db.RKD_Version.Where(d => d.id_RKD_Order == id_RKD_Order).Count() + 1;
//        rKD_Version.dateUpload = DateTime.Now;
//        rKD_Version.userUpload = login;
//        rKD_Version.customdateUpload = dateUpload;
//        db.RKD_Version.Add(rKD_Version);
//        db.SaveChanges();
//        UpdateAllTask(rKD_Version.id);
//        logger.Debug(goodMethod + " (Modul RKD - UpdateNewRKD_Version(int id_RKD_Order, string login, DateTime dateUpload)");
//    }
//    catch (Exception ex)
//    {
//        logger.Error(badMethod + " (Modul RKD - UpdateNewRKD_Version(int id_RKD_Order, string login, DateTime dateUpload); " + ex.Message.ToString());
//    }
//}

//private void UpdateAllTask(int id_RKD_Version)
//{
//    try
//    {
//        RKD_Version rKD_Version = db.RKD_Version.Find(id_RKD_Version);
//        RKD_Version datax = db.RKD_Version.Where(d => d.numberVersion == rKD_Version.numberVersion - 1).First();
//        foreach (var data in db.RKD_Task.Where(d => d.id_RKD_Version == datax.id).ToList())
//        {
//            RKD_Task rKD_Task = new RKD_Task();
//            rKD_Task.id_RKD_Version = id_RKD_Version;
//            rKD_Task.id_RKD_TypeTask = data.id_RKD_TypeTask;
//            rKD_Task.complited = data.complited;
//            rKD_Task.dateUpload = data.dateUpload;
//            rKD_Task.userUpload = data.userUpload;
//            rKD_Task.customdateUpload = data.customdateUpload;
//            db.RKD_Task.Add(rKD_Task);
//            db.SaveChanges();
//        }
//        logger.Debug(goodMethod + " (Modul RKD - UpdateAllTask(int id_RKD_Version)");
//    }
//    catch (Exception ex)
//    {
//        logger.Error(badMethod + " (Modul RKD - UpdateAllTask(int id_RKD_Version); " + ex.Message.ToString());
//    }
//}

//private void ImportExcelDataTMP(HttpPostedFileBase[] fileExcel, string path)
//{
//    try
//    {
//        fileExcel[0].SaveAs(path);
//        Excel.Application ObjWorkExcel = new Excel.Application();
//        Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(path);
//        Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
//        var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
//        int getColumn = 0;
//        string data = "";
//        bool l = false;
//        for (int i = 3; l == false; i++)
//        {
//            data = ObjWorkSheet.Cells[4, i].Text.ToString();
//            if (data == "")
//            {
//                getColumn = i - 1;
//                l = true;
//            }
//        }
//        List<DespatchingTmp> listData = new List<DespatchingTmp>();
//        for (int k = getColumn; k > 2; k--)
//        {

//            try
//            {
//                for (int i = 6; i <= lastCell.Row; i++)
//                {
//                    listData.Add(new DespatchingTmp(

//                        Convert.ToInt32(ObjWorkSheet.Cells[i, 2].Text.ToString().Substring(0, 4)), 
//                        ObjWorkSheet.Cells[i, k].Text.ToString(), 
//                        GetDateDespatchingProtocol(ObjWorkSheet.Cells[4, k].Text.ToString())
//                        )
//                        );
//                }
//            }
//            catch (Exception ex)
//            {
//                string errror = "";
//                errror = ex.ToString();
//                errror += "";
//                ObjWorkBook.Close(false, Type.Missing, Type.Missing);
//                ObjWorkExcel.Quit();
//                GC.Collect();
//            }
//        }
//        ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
//        ObjWorkExcel.Quit(); // выйти из экселя
//        GC.Collect(); // убрать за собой
//        foreach (var dataList in listData)
//        {
//            CreateDespatching(dataList.NumberPlanZakaz, dataList.DataText, dataList.Date);
//        }
//        ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
//        ObjWorkExcel.Quit(); // выйти из экселя
//        GC.Collect(); // убрать за собой
//        logger.Debug(goodConstructor + " Modul RKD - ImportExcelDataTMP(HttpPostedFileBase[] fileExcel, string path)");
//    }
//    catch (Exception ex)
//    {
//        logger.Error(badConstructor + " (Modul RKD - ImportExcelDataTMP(HttpPostedFileBase[] fileExcel, string path) " + ex.Message.ToString());
//    }
//}