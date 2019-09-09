using NLog;
using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.Entity;

namespace Wiki.Models
{
    public class RKD
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string goodMethod = " метод отработал успешно ";
        private string badMethod = " метод завершился ошибкой ";
        private string badDB = "ошибка сохранениея даных на сервере ";
        private PortalKATEKEntities db = new PortalKATEKEntities();

        private int countDayPlusBeforeCreatePZ = 20;
        private int countDayBeforeUoloadClient = 0;
        private int countDayBeforErrorList = 25;
        private int id_PZ_PlaznZakaz;
        public int id_RKD_Order;
        private Guid projectUID;
        private string loginEmail;

        public void Tmp()
        {
            id_RKD_Order = 243;
            CreateTaskVersion(196, 224, "vi@katek.by");
        }

        public RKD()
        {

        }
        
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
                RKD_GIP gip = new RKD_GIP
                {
                    id_RKD_Order = rKD_Order.id,
                    id_UserKBE = "8363828f-bba2-4a89-8ed8-d7f5623b4fa8",
                    id_UserKBM = "4f91324a-1918-4e62-b664-d8cd89a19d95"
                };
                db.RKD_GIP.Add(gip);
                db.SaveChanges();
                CreateTasks();
                CreateNULLVersion();
                Create_NULLRKD_TaskVersion();
                logger.Debug("RKD - CreateRKDOrder " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - CreateRKDOrder " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
            }
        }

        //1 Upload First New version RKD (GIP)
        public void UploadNewVersionRDK(RKD_Order rKD_Order, string num, string folderRKD, string login)
        {
            try
            {
                loginEmail = login;
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
                rKD_Version.id_RKD_VersionDay = 1;
                rKD_Version.planFinishDate = DateTime.Now.AddDays(1);
                rKD_Version.activeVersion = true;
                db.RKD_Version.Add(rKD_Version);
                db.SaveChanges();
                CreateRKD_Mail_Version(rKD_Version.id, 1, folderRKD, login);
                CreateTaskVersion(id_lastVersion, rKD_Version.id, login);
                UpdateTaskUID();
                EmailFirstToTPVersion mail = new EmailFirstToTPVersion(rKD_Order, login);
                mail.CreateAndSendMail(folderRKD);
                logger.Debug("RKD - UploadNewVersionRDK " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - UploadNewVersionRDK " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
            }
        }
        
        //2 - вернуть РКД на доработку в КО
        public void ErrorNewVersionRDK(int id_Order, string login, string errorBody)
        {
            try
            {
                loginEmail = login;
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 4;
                rKD_Version.planFinishDate = DateTime.Now.AddDays(1);
                db.Entry(rKD_Version).State = EntityState.Modified;
                db.SaveChanges();
                CreateRKD_Mail_Version(rKD_Version.id, 2, login);
                EmailRevisionToKO email = new EmailRevisionToKO(db.RKD_Order.Find(id_Order), login);
                email.CreateAndSendMail(errorBody);
                logger.Debug(goodMethod + " (Modul RKD - ErrorNewVersionRDK(int id_Order, string login) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - ErrorNewVersionRDK(int id_Order, string login) " + ex.Message.ToString());
            }
        }

        //3 - загрузка исправленной версии РКД КО для согласования в ТП
        public void UploadVersionRDK(RKD_Order rKD_Order, string folderRKD, string login)
        {
            try
            {
                loginEmail = login;
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == rKD_Order.id).First();
                rKD_Version.id_RKD_VersionWork = 2;
                rKD_Version.id_RKD_VersionDay = 1;
                rKD_Version.planFinishDate = DateTime.Now.AddDays(1);
                db.Entry(rKD_Version).State = EntityState.Modified;
                db.SaveChanges();
                CreateRKD_Mail_Version(rKD_Version.id, 3, folderRKD, login);
                EmailToTPVersion mail = new EmailToTPVersion(rKD_Order, login);
                mail.CreateAndSendMail(folderRKD);
                logger.Debug(goodMethod + " (Modul RKD - UploadVersionRDK(HttpPostedFileBase[] fileUpload, RKD_Order rKD_Order, int num1, int num2, int version, string login) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - UploadVersionRDK(HttpPostedFileBase[] fileUpload, RKD_Order rKD_Order, int num1, int num2, int version, string login) " + ex.Message.ToString());
            }
        }

        //4 - отправка версии РКД Заказчику/ПИ
        public void UploadClient(int id_Order, string login, int day)
        {
            try
            {
                loginEmail = login;
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 5;
                rKD_Version.planFinishDate = DateTime.Now.AddDays(db.RKD_VersionDay.Find(day).countDay);
                db.Entry(rKD_Version).State = EntityState.Modified;
                db.SaveChanges();
                Update_RKD_TaskVerdion(day, rKD_Version.id, login);
                CreateRKD_Mail_Version(rKD_Version.id, 4, login, day);
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - UploadClient(int id_Order, string login, int day)) " + ex.Message.ToString());
            }
        }

        //6 - прибавить время на ожидание ответа от Заказчика/ПИ
        public void PlusClientDay(int id_Order, string login, DateTime day)
        {
            try
            {
                loginEmail = login;
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 5;
                rKD_Version.planFinishDate = day;
                db.Entry(rKD_Version).State = EntityState.Modified;
                db.SaveChanges();
                Update_RKD_TaskVerdion(day, rKD_Version.id, login);
                CreateRKD_Mail_Version(rKD_Version.id, 6, "", login);
                logger.Debug(goodMethod + " (Modul RKD - PlusClientDay(int id_Order, string login, DateTime day)) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - UploadClient(int id_Order, string login, int day)) " + ex.Message.ToString());
            }
        }

        //7 - получены замечания от Заказчика/ПИ
        public void GetErrorList(int id_Order, string login, string folderRKD, string descriptionError)
        {
            try
            {
                loginEmail = login;
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 8;
                rKD_Version.planFinishDate = DateTime.Now;
                db.Entry(rKD_Version).State = EntityState.Modified;
                db.SaveChanges();
                CreateRKD_Mail_Version(rKD_Version.id, 7, login);
                Update_RKD_TaskVerdion(DateTime.Now.AddDays(countDayBeforErrorList), rKD_Version.id, loginEmail);
                EmailPutErrorClient mail = new EmailPutErrorClient(db.RKD_Order.Find(id_Order), login, folderRKD);
                mail.CreateAndSendMail(descriptionError);
                logger.Debug(goodMethod + " (Modul RKD - GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)) " + ex.Message.ToString());
            }
        }

        //8 - получено официальное согласование от Заказчика/ПИ
        public void GetGoodList(int id_Order, string login, string folderRKD)
        {
            try
            {
                loginEmail = login;
                RKD_Version rKD_Version = db.RKD_Version.First(d => d.activeVersion == true & d.id_RKD_Order == id_Order);
                rKD_Version.id_RKD_VersionWork = 10;
                db.Entry(rKD_Version).State = EntityState.Modified;
                db.SaveChanges();
                CreateRKD_Mail_Version(rKD_Version.id, 8, login);
                Update_RKD_TaskVerdion(DateTime.Now, rKD_Version.id, loginEmail);

                EmailSuccessRKD mail = new EmailSuccessRKD(db.RKD_Order.Find(id_Order), login);
                mail.CreateAndSendMail("");

                logger.Debug(goodMethod + " (Modul RKD - GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - GetErrorList(int id_Order, string login, HttpPostedFileBase[] fileUpload)) " + ex.Message.ToString());
            }
        }
        //8 - приняли решение о неофициальном согласовании РКД
        public void GetPreGoodList(int id_Order, string login)
        {
            try
            {
                loginEmail = login;
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                rKD_Version.id_RKD_VersionWork = 16;
                db.Entry(rKD_Version).State = EntityState.Modified;
                db.SaveChanges();
                CreateRKD_Mail_Version(rKD_Version.id, 9, login);
                Update_RKD_TaskVerdion(DateTime.Now, rKD_Version.id, loginEmail);
                logger.Debug(goodMethod + " RKD - GetPreGoodList(int id_Order, string login) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " RKD - GetPreGoodList(int id_Order, string login) " + ex.Message.ToString());
            }
        }
        
        //8 - обновить глобально статусы задач (КО-ОС-ПО)
        public void GetPreGoodList(int id_Order, string login, int typeTask)
        {
            try
            {
                loginEmail = login;
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_Order).First();
                Update_RKD_TaskVerdion(rKD_Version.id, loginEmail, typeTask);
                logger.Debug(goodMethod + " RKD - GetPreGoodList(int id_Order, string login) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " RKD - GetPreGoodList(int id_Order, string login) " + ex.Message.ToString());
            }
        }
        
        // РКД направлено на доработку в КО и ожидаем новую версию
        public void LoadNewVersionRKD(RKD_Order rKD_Order, string login, DateTime planFinish)
        {
            try
            {
                loginEmail = login;
                RKD_Order myRKD = db.RKD_Order.Find(rKD_Order.id);
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == myRKD.id).First();
                rKD_Version.id_RKD_VersionWork = 8;
                rKD_Version.planFinishDate = planFinish;
                db.Entry(rKD_Version).State = EntityState.Modified;
                db.SaveChanges();
                Update_RKD_TaskVerdion(rKD_Order.datePlanCritical, rKD_Version.id, login);
                CreateRKD_Mail_Version(rKD_Version.id, 10, "", login);
                logger.Debug(goodMethod + " LoadNewVersionRKD(RKD_Order rKD_Order, string login) ");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " LoadNewVersionRKD(RKD_Order rKD_Order, string login) " + ex.Message.ToString());
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
                    rKD_Version.uslovnoeComplited = data.uslovnoeComplited;
                    rKD_Version.manufComplited = data.manufComplited;
                    rKD_Version.osComplited = data.osComplited;
                    db.RKD_TaskVersion.Add(rKD_Version);
                    db.SaveChanges();
                    RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                    rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_Version.id;
                    rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                    rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                    rKD_HistoryTaskVersion.finishDate = rKD_Version.finishDate;
                    rKD_HistoryTaskVersion.tpComplited = rKD_Version.tpComplited;
                    rKD_HistoryTaskVersion.osComplited = rKD_Version.osComplited;
                    rKD_HistoryTaskVersion.uslovnoeComplited = rKD_Version.uslovnoeComplited;
                    rKD_HistoryTaskVersion.manufComplited = rKD_Version.manufComplited;
                    rKD_HistoryTaskVersion.allComplited = rKD_Version.allComplited;
                    db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                    db.SaveChanges();
                    db.SaveChanges();
                }
                logger.Debug(goodMethod + " (Modul RKD - CreateTaskVersion(int id_lastVersion, int idVersion)");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - CreateTaskVersion(int id_lastVersion, int idVersion); " + ex.Message.ToString());
            }
        }

        private string CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version, string login, int id_rKD_Version)
        {
            try
            {
                string directory = db.PZ_PlanZakaz.Find(id_PZ_PlaznZakaz).Folder + "\\05_РКД\\Переписка\\";
                directory += DateTime.Now.Year.ToString() + "_";
                if (DateTime.Now.Month.ToString().Length == 1)
                    directory += "0" + DateTime.Now.Month.ToString();
                else
                    directory += DateTime.Now.Month.ToString();
                directory += "_";
                if (DateTime.Now.Day.ToString().Length == 1)
                    directory += "0" + DateTime.Now.Day.ToString();
                else
                    directory += DateTime.Now.Day.ToString();
                directory += " v." + db.RKD_Version.Find(id_rKD_Version).numberVersion1.ToString() + "." + db.RKD_Version.Find(id_rKD_Version).numberVersion2.ToString();
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
                return directory;
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul RKD - CreateFolderSaveFile(HttpPostedFileBase[] fileUpload, int id_TypeRKD_Mail_Version) " + ex.Message.ToString());
                return null;
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

        private void CreateRKD_Mail_TimeForComplited(int id_Version, int day)
        {
            try
            {
                RKD_Mail_TimeForComplited rKD_Mail_TimeForComplited = new RKD_Mail_TimeForComplited();
                rKD_Mail_TimeForComplited.id_RKD_Mail_Version = db.RKD_Mail_Version.Where(d => d.id_RKD_Version == id_Version & d.id_TypeRKD_Mail_Version == 4).First().id;
                rKD_Mail_TimeForComplited.day = day;
                db.RKD_Mail_TimeForComplited.Add(rKD_Mail_TimeForComplited);
                db.SaveChanges();
                logger.Debug(goodMethod + " CreateRKD_Mail_TimeForComplited");
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " CreateRKD_Mail_TimeForComplited " + ex.Message.ToString());
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

        private void GetFileListUploadData(int id_rKD_Mail_Version, string directory)
        {
            try
            {
                foreach (var file in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
                {
                    RKD_FileMailVersion rKD_FileMailVersion = new RKD_FileMailVersion();
                    rKD_FileMailVersion.id_RKD_Mail_Version = id_rKD_Mail_Version;
                    rKD_FileMailVersion.dateTimeLastUpdateFile = System.IO.File.GetLastWriteTime(file);
                    rKD_FileMailVersion.nameFile = file.ToString();
                    db.RKD_FileMailVersion.Add(rKD_FileMailVersion);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " RKD - GetFileListUploadData " + ex.Message.ToString());
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
                int findIdOrder = db.RKD_Order.First(d => d.id_PZ_PlanZakaz == id_PZ_PlaznZakaz).id;
                var listUpdate = db.RKD_Version.Where(d => d.id_RKD_Order == findIdOrder & d.activeVersion == true).ToList();
                if (listUpdate != null)
                {
                    foreach (var data in listUpdate)
                    {
                        data.activeVersion = false;
                        db.Entry(data).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                logger.Debug("RKD - DeactiveAllVersion " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - DeactiveAllVersion " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
            }
        }

        public void Create_NULLRKD_TaskVersion()
        {
            try
            {
                RKD_Version rKD_Version = db.RKD_Version.Where(d => d.activeVersion == true & d.id_RKD_Order == id_RKD_Order).First();
                var taskList = db.RKD_Task.Where(d => d.id_RKD_Order == id_RKD_Order).ToList();
                foreach (var data in taskList)
                {
                    RKD_TaskVersion rKD_TaskVersion = new RKD_TaskVersion();
                    rKD_TaskVersion.id_RKD_Task = data.id;
                    rKD_TaskVersion.id_RKD_Version = rKD_Version.id;
                    rKD_TaskVersion.finishDate = DateTime.Now.AddDays(countDayPlusBeforeCreatePZ);
                    rKD_TaskVersion.tpComplited = false;
                    rKD_TaskVersion.allComplited = false;
                    rKD_TaskVersion.uslovnoeComplited = false;
                    rKD_TaskVersion.osComplited = false;
                    rKD_TaskVersion.manufComplited = false;
                    db.RKD_TaskVersion.Add(rKD_TaskVersion);
                    db.SaveChanges();
                    RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                    rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                    rKD_HistoryTaskVersion.id_AspNetUser_Upload = "8dbd0ecb-e88c-47b8-951b-3f1b5ea10cde";
                    rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                    rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                    rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                    rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                    rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                    rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                    db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                    db.SaveChanges();
                    //if (rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value != null ||
                    //    rKD_TaskVersion.RKD_Task.UID_Task.Value != null)
                    //{
                    //    TaskForPWA taskForPwa = new TaskForPWA();
                    //    taskForPwa.CreateTaskRKD(rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value, rKD_TaskVersion.RKD_Task.UID_Task.Value, DateTime.Now, 100);
                    //}
                }
                logger.Debug("RKD - Create_NULLRKD_TaskVersion " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - Create_NULLRKD_TaskVersion " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
            }
        }

        private void GetId_RKD_Order()
        {
            try
            {
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Where(d => d.Id == id_PZ_PlaznZakaz).First();
                id_RKD_Order = db.RKD_Order.Where(d => d.id_PZ_PlanZakaz == id_PZ_PlaznZakaz).First().id;
                projectUID = pZ_PlanZakaz.ProjectUID.Value;
                logger.Debug("RKD - GetId_RKD_Order " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - GetId_RKD_Order " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
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
                    if (rKD_Task.UID_Task == null)
                    {
                        db.RKD_Task.Remove(rKD_Task);
                        db.SaveChanges();
                    }
                }
                var deleteData = db.RKD_Task.Where(d => d.UID_Task == null & d.RKD_Order.id_PZ_PlanZakaz == id_PZ_PlaznZakaz).ToList();
                foreach (var data in deleteData)
                {
                    db.RKD_Task.Remove(data);
                    db.SaveChanges();
                }
                logger.Debug("RKD - UpdateTaskUID " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - UpdateTaskUID " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
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
                logger.Debug("RKD - CreateTasks " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - CreateTasks " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
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
                logger.Debug("RKD - CreateNULLVersion " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - CreateNULLVersion " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
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
                logger.Debug("RKD - CreateDespatching " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - CreateDespatching " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
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
                logger.Debug("RKD - GetDateDespatchingProtocol " + id_RKD_Order.ToString());
                return dateProtocol;
            }
            catch (Exception ex)
            {
                logger.Error("RKD - GetDateDespatchingProtocol " + id_RKD_Order.ToString() + " " + ex.Message.ToString());
                return DateTime.Now;
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

        public void Update_RKD_TaskVerdion(int day, int id_RKD_Version, string login)
        {
            try
            {
                var rKD_VersionList = db.RKD_TaskVersion.Where(d => d.id_RKD_Version == id_RKD_Version && d.manufComplited == false && d.RKD_Version.activeVersion == true).ToList();
                int dayForSideClient = db.RKD_VersionDay.Find(day).countDay;
                foreach (var data in rKD_VersionList)
                {
                    RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(data.id);
                    rKD_TaskVersion.finishDate = DateTime.Now.AddDays(countDayBeforeUoloadClient + dayForSideClient);
                    db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                    db.SaveChanges();
                    RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                    rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                    rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                    rKD_HistoryTaskVersion.uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited;
                    rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                    rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                    rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                    rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                    rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                    rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                    db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                    db.SaveChanges();
                }
                logger.Debug("RKD - Update_RKD_TaskVerdion " + id_RKD_Order.ToString());
            }
            catch(Exception ex)
            {
                logger.Error("RKD - Update_RKD_TaskVerdion type1" + id_RKD_Order.ToString() + " " + ex.Message.ToString());
            }
        }

        private void Update_RKD_TaskVerdion(DateTime day, int id_RKD_Version, string login)
        {
            try
            {
                var rKD_VersionList = db.RKD_TaskVersion.Where(d => d.id_RKD_Version == id_RKD_Version && d.RKD_Version.activeVersion == true).ToList();
                foreach (var data in rKD_VersionList)
                {
                    RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(data.id);
                    rKD_TaskVersion.allComplited = true;
                    rKD_TaskVersion.osComplited = true;
                    rKD_TaskVersion.manufComplited = true;
                    rKD_TaskVersion.uslovnoeComplited = true;
                    rKD_TaskVersion.tpComplited = true;
                    rKD_TaskVersion.finishDate = day;
                    db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                    db.SaveChanges();
                    try
                    {
                        if (rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID != null ||
                            rKD_TaskVersion.RKD_Task.UID_Task.Value != null)
                        {
                            TaskForPWA taskForPwa = new TaskForPWA();
                            taskForPwa.CreateTaskRKD(rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value, rKD_TaskVersion.RKD_Task.UID_Task.Value, day, 100);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("RKD - Update_RKD_TaskVerdion  type2 - PWA!!!!" + id_RKD_Order.ToString() + " " + ex.Message.ToString());
                    }

                    RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion
                    {
                        allComplited = rKD_TaskVersion.allComplited,
                        dateTimeUpdate = DateTime.Now,
                        finishDate = rKD_TaskVersion.finishDate,
                        tpComplited = rKD_TaskVersion.tpComplited,
                        manufComplited = rKD_TaskVersion.manufComplited,
                        osComplited = rKD_TaskVersion.osComplited,
                        uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited,
                        id_AspNetUser_Upload = db.AspNetUsers.First(d => d.Email == login).Id,
                        id_RKD_TaskVersion = rKD_TaskVersion.id
                    };
                    db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                    db.SaveChanges();
                }
                logger.Debug("RKD - Update_RKD_TaskVerdion " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - Update_RKD_TaskVerdion  type2" + id_RKD_Order.ToString() + " " + ex.Message.ToString());
            }
        }

        private void Update_RKD_TaskVerdion(int id_RKD_Version, string login, int typeTask)
        {
            try
            {
                var rKD_VersionList = db.RKD_TaskVersion.Where(d => d.id_RKD_Version == id_RKD_Version & d.manufComplited == false & d.RKD_Version.activeVersion == true).ToList();
                if (typeTask == 1)
                {
                    foreach (var data in rKD_VersionList)
                    {
                        RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(data.id);
                        rKD_TaskVersion.uslovnoeComplited = true;
                        db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                        db.SaveChanges();
                        try
                        {
                            if (rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID != null ||
                                rKD_TaskVersion.RKD_Task.UID_Task.Value != null)
                            {
                                TaskForPWA taskForPwa = new TaskForPWA();
                                taskForPwa.CreateTaskRKD(rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value, rKD_TaskVersion.RKD_Task.UID_Task.Value, rKD_TaskVersion.finishDate, 100);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("RKD - Update_RKD_TaskVerdion  type2 - PWA!!!!" + id_RKD_Order.ToString() + " " + ex.Message.ToString());
                        }
                        RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                        rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                        rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                        rKD_HistoryTaskVersion.uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited;
                        rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                        rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                        rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                        rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                        rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.First(d => d.Email == login).Id;
                        rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                        db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                        db.SaveChanges();
                    }
                }
                if (typeTask == 2)
                {
                    foreach (var data in rKD_VersionList)
                    {
                        RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(data.id);
                        rKD_TaskVersion.uslovnoeComplited = true;
                        rKD_TaskVersion.osComplited = true;
                        db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                        db.SaveChanges();
                        try
                        {
                            if (rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID != null ||
                                rKD_TaskVersion.RKD_Task.UID_Task.Value != null)
                            {
                                TaskForPWA taskForPwa = new TaskForPWA();
                                taskForPwa.CreateTaskRKD(rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value, rKD_TaskVersion.RKD_Task.UID_Task.Value, rKD_TaskVersion.finishDate, 100);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("RKD - Update_RKD_TaskVerdion  type2 - PWA!!!!" + id_RKD_Order.ToString() + " " + ex.Message.ToString());
                        }
                        RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                        rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                        rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                        rKD_HistoryTaskVersion.uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited;
                        rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                        rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                        rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                        rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                        rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                        rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                        db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                        db.SaveChanges();
                    }
                }
                if (typeTask == 3)
                {
                    foreach (var data in rKD_VersionList)
                    {
                        RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(data.id);
                        rKD_TaskVersion.finishDate = DateTime.Now;
                        rKD_TaskVersion.uslovnoeComplited = true;
                        rKD_TaskVersion.osComplited = true;
                        rKD_TaskVersion.manufComplited = true;
                        db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                        db.SaveChanges();
                        try
                        {
                            if (rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID != null ||
                                rKD_TaskVersion.RKD_Task.UID_Task.Value != null)
                            {
                                TaskForPWA taskForPwa = new TaskForPWA();
                                taskForPwa.CreateTaskRKD(rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value, rKD_TaskVersion.RKD_Task.UID_Task.Value, rKD_TaskVersion.finishDate, 100);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("RKD - Update_RKD_TaskVerdion  type2 - PWA!!!!" + id_RKD_Order.ToString() + " " + ex.Message.ToString());
                        }
                        RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                        rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                        rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                        rKD_HistoryTaskVersion.uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited;
                        rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                        rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                        rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                        rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                        rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                        rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                        db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                        db.SaveChanges();
                    }
                }
                logger.Debug("RKD - Update_RKD_TaskVerdion " + id_RKD_Order.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("RKD - Update_RKD_TaskVerdion  type3" + id_RKD_Order.ToString() + " " + ex.Message.ToString());
            }
        }
        
        // изменение задачи в зависимости от принятого решения (КО-ОС-ПО)
        public void UpdateRKD_TaskVersionComplited(int id, string login, int typeTask)
        {
            if (typeTask == 0)
            {
                RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(id);
                rKD_TaskVersion.finishDate = DateTime.Now;
                rKD_TaskVersion.uslovnoeComplited = true;
                db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                db.SaveChanges();
                RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                rKD_HistoryTaskVersion.uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited;
                rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                db.SaveChanges();
            }
            if (typeTask == 1)
            {
                RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(id);
                rKD_TaskVersion.finishDate = DateTime.Now;
                rKD_TaskVersion.uslovnoeComplited = true;
                db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                db.SaveChanges();
                RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                rKD_HistoryTaskVersion.uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited;
                rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                db.SaveChanges();

                if (rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID != null ||
                    rKD_TaskVersion.RKD_Task.UID_Task.Value != null)
                {
                    TaskForPWA taskForPwa = new TaskForPWA();
                    taskForPwa.CreateTaskRKD(rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value, rKD_TaskVersion.RKD_Task.UID_Task.Value, DateTime.Now, 100);
                }
            }
            if (typeTask == 2)
            {
                RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(id);
                rKD_TaskVersion.finishDate = DateTime.Now;
                rKD_TaskVersion.uslovnoeComplited = true;
                rKD_TaskVersion.osComplited = true;
                db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                db.SaveChanges();
                RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                rKD_HistoryTaskVersion.uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited;
                rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                db.SaveChanges();
                if (rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID != null ||
                    rKD_TaskVersion.RKD_Task.UID_Task.Value != null)
                {
                    TaskForPWA taskForPwa = new TaskForPWA();
                    taskForPwa.CreateTaskRKD(rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value, rKD_TaskVersion.RKD_Task.UID_Task.Value, DateTime.Now, 100);
                }
            }
            if (typeTask == 3)
            {
                RKD_TaskVersion rKD_TaskVersion = db.RKD_TaskVersion.Find(id);
                rKD_TaskVersion.finishDate = DateTime.Now;
                rKD_TaskVersion.uslovnoeComplited = true;
                rKD_TaskVersion.osComplited = true;
                rKD_TaskVersion.manufComplited = true;
                db.Entry(rKD_TaskVersion).State = EntityState.Modified;
                db.SaveChanges();
                RKD_HistoryTaskVersion rKD_HistoryTaskVersion = new RKD_HistoryTaskVersion();
                rKD_HistoryTaskVersion.allComplited = rKD_TaskVersion.allComplited;
                rKD_HistoryTaskVersion.dateTimeUpdate = DateTime.Now;
                rKD_HistoryTaskVersion.finishDate = rKD_TaskVersion.finishDate;
                rKD_HistoryTaskVersion.tpComplited = rKD_TaskVersion.tpComplited;
                rKD_HistoryTaskVersion.id_AspNetUser_Upload = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                rKD_HistoryTaskVersion.id_RKD_TaskVersion = rKD_TaskVersion.id;
                rKD_HistoryTaskVersion.uslovnoeComplited = rKD_TaskVersion.uslovnoeComplited;
                rKD_HistoryTaskVersion.osComplited = rKD_TaskVersion.osComplited;
                rKD_HistoryTaskVersion.manufComplited = rKD_TaskVersion.manufComplited;
                db.RKD_HistoryTaskVersion.Add(rKD_HistoryTaskVersion);
                db.SaveChanges();
                if (rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID != null ||
                    rKD_TaskVersion.RKD_Task.UID_Task.Value != null)
                {
                    TaskForPWA taskForPwa = new TaskForPWA();
                    taskForPwa.CreateTaskRKD(rKD_TaskVersion.RKD_Version.RKD_Order.PZ_PlanZakaz.ProjectUID.Value, rKD_TaskVersion.RKD_Task.UID_Task.Value, DateTime.Now, 100);
                }
            }
        }
        
        private string GetFolderLinkPDisk(string link)
        {
            if (link.Length < 5)
                return link;
            else
            {
                string data = link.Substring(0, 1);
                if (data == "p")
                {
                    link = link.Substring(1, link.Length - 1);
                    link = "\\192.168.1.30\\m$\\" + link;
                }
                return link;
            }
        }
    }
}
