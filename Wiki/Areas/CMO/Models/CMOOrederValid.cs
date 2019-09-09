using NLog;
using System;
using System.IO;
using System.Web;
using Wiki.Models;

namespace Wiki.Areas.CMO.Models
{
    public class CMOOrederValid
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        PortalKATEKEntities db = new PortalKATEKEntities();
        int id_TypeReOrder = 10;
        CMO2_Order cMO2_Order;
        private HttpPostedFileBase[] fileUploadArray;

        public void UpdateOrder(CMO2_Order cMO2_Order, string login)
        {
            CMO2_Order order = db.CMO2_Order.Find(cMO2_Order.id);
            if(order.reOrder == true)
            {

            }
            else if (cMO2_Order.workIn == false)
            {
                order.id_CMO_Company = cMO2_Order.id_CMO_Company;
                order.workIn = true;
                order.workDateTime = cMO2_Order.workDateTime;
                order.workCost = cMO2_Order.workCost;
                order.workComplitet = true;
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                try
                {
                    new EmailCMO(cMO2_Order, login, 2);
                    logger.Debug("UpdateOrder / EmailCMO: " + order.id);
                }
                catch (Exception ex)
                {
                    logger.Error("UpdateOrder / EmailCMO: " + order.id + " | " + ex);
                }
            }
            else if (cMO2_Order.manufIn == false)
            {
                bool pushMail = true;
                if (order.manufComplited == true)
                    pushMail = false;
                order.id_CMO_Company = cMO2_Order.id_CMO_Company;
                order.manufIn = true;
                order.workDateTime = DateTime.Now;
                order.manufDate = cMO2_Order.manufDate;
                order.manufCost = cMO2_Order.manufCost;
                order.manufComplited = true;
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                if(pushMail == true)
                {
                    try
                    {
                        new EmailCMO(cMO2_Order, login, 3);
                        logger.Debug("UpdateOrder / EmailCMO: " + order.id);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("UpdateOrder / EmailCMO: " + order.id + " | " + ex);
                    }
                }

                else
                {
                    try
                    {
                        new EmailCMO(cMO2_Order, login, 5);
                        logger.Debug("UpdateOrder / EmailCMO: " + order.id);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("UpdateOrder / EmailCMO: " + order.id + " | " + ex);
                    }
                }
            }
            else if (cMO2_Order.finIn == false && cMO2_Order.finDate != null)
            {
                order.id_CMO_Company = cMO2_Order.id_CMO_Company;
                order.finIn = true;
                order.finDate = cMO2_Order.finDate;
                order.finCost = cMO2_Order.finCost;
                order.finComplited = true;
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else if (cMO2_Order.finIn == false && cMO2_Order.finDate == null && cMO2_Order.manufDate != null)
            {
                order.id_CMO_Company = cMO2_Order.id_CMO_Company;
                order.manufDate = cMO2_Order.manufDate;
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                try
                {
                    new EmailCMO(cMO2_Order, login, 5);
                    logger.Debug("UpdateOrder / EmailCMO: " + order.id);
                }
                catch (Exception ex)
                {
                    logger.Error("UpdateOrder / EmailCMO: " + order.id + " | " + ex);
                }
            }
            else
            {

            }
        }
        public void CreateOrder(int[] id_PlanZakaz, int[] id_CMO_TypeProduct, string login, HttpPostedFileBase[] fileUploadArray)
        {
            this.fileUploadArray = fileUploadArray;
            GetBasicFuild(login);
            GetDefaultWork();
            GetDefaultManuf();
            GetDefaultFin();
            cMO2_Order.reOrder = false;
            db.CMO2_Order.Add(cMO2_Order);
            db.SaveChanges();
            CreateFolderAndFileForPreOrder(cMO2_Order.id);
            foreach (var dataPZ in id_PlanZakaz)
            {
                foreach (var dataType in id_CMO_TypeProduct)
                {
                    CMO2_Position cMO2_Position = new CMO2_Position();
                    cMO2_Position.id_PZ_PlanZakaz = dataPZ;
                    cMO2_Position.id_CMO_TypeProduct = dataType;
                    cMO2_Position.id_CMO2 = cMO2_Order.id;
                    db.CMO2_Position.Add(cMO2_Position);
                }
            }
            db.SaveChanges();
            try
            {
                new EmailCMO(cMO2_Order, login, 1);
                logger.Debug("CreateOrder / EmailCMO: " + cMO2_Order.id);
            }
            catch (Exception ex)
            {
                logger.Error("CreateOrder / EmailCMO: " + cMO2_Order.id + " | " + ex);
            }
        }

        public void CreateReOrder(int[] id_PlanZakaz, int id_CMO_Company, string login, HttpPostedFileBase[] fileUploadArray)
        {
            this.fileUploadArray = fileUploadArray;
            GetBasicFuild(login);
            GetDefaultWork();
            GetDefaultManuf();
            GetDefaultFin();
            cMO2_Order.reOrder = true;
            cMO2_Order.id_CMO_Company = id_CMO_Company;
            db.CMO2_Order.Add(cMO2_Order);
            db.SaveChanges();
            CreateFolderAndFileForPreOrder(cMO2_Order.id);
            foreach (var dataPZ in id_PlanZakaz)
            {
                CMO2_Position cMO2_Position = new CMO2_Position();
                cMO2_Position.id_PZ_PlanZakaz = dataPZ;
                cMO2_Position.id_CMO_TypeProduct = id_TypeReOrder;
                cMO2_Position.id_CMO2 = cMO2_Order.id;
                db.CMO2_Position.Add(cMO2_Position);
            }
            db.SaveChanges();
            try
            {
                new EmailCMO(cMO2_Order, login, 4);
                logger.Debug("CreateReOrder / EmailCMO: " + cMO2_Order.id);
            }
            catch (Exception ex)
            {
                logger.Error("CreateReOrder / EmailCMO: " + cMO2_Order.id + " | " + ex);
            }
        }

        public CMO2_Order CMO2_Order { get => cMO2_Order; set => cMO2_Order = value; }

        bool GetBasicFuild(string login)
        {
            cMO2_Order = new CMO2_Order();
            cMO2_Order.dateTimeCreate = DateTime.Now;
            cMO2_Order.id_AspNetUsers_Create = new Login().LoginEmailToId(login);
            cMO2_Order.folder = "";
            return true;
        }

        bool GetDefaultWork()
        {
            cMO2_Order.workIn = false;
            cMO2_Order.workComplitet = false;
            cMO2_Order.workCost = 0;
            return true;
        }

        bool GetDefaultManuf()
        {
            cMO2_Order.manufComplited = false;
            cMO2_Order.manufIn = false;
            cMO2_Order.manufCost = 0;
            return true;
        }

        bool GetDefaultFin()
        {
            cMO2_Order.finComplited = false;
            cMO2_Order.finIn = false;
            cMO2_Order.finCost = 0;
            return true;
        }

        private void CreateFolderAndFileForPreOrder(int id)
        {
            string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\OrderCMO\\NewOrders\\" + id.ToString() + "\\";
            Directory.CreateDirectory(directory);
            SaveFileToServer(directory);
            cMO2_Order.folder = directory;
            db.Entry(cMO2_Order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        private void SaveFileToServer(string folderAdress)
        {
            for (int i = 0; i < fileUploadArray.Length; i++)
            {
                string fileReplace = Path.GetFileName(fileUploadArray[i].FileName);
                fileReplace = ToSafeFileName(fileReplace);
                var fileName = string.Format("{0}\\{1}", folderAdress, fileReplace);
                fileUploadArray[i].SaveAs(fileName);
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
    }
}