using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wiki.Models;

namespace Wiki.Areas.CMO.Models
{
    public class SandwichPanelCRUD
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private HttpPostedFileBase[] fileUploadArray;
        SandwichPanel sandwichPanel;

        public void CreateOrder(int[] id_PlanZakaz, string login, HttpPostedFileBase[] fileUploadArray)
        {

            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                this.fileUploadArray = fileUploadArray;
                GetBasicFuild(login);
                db.SandwichPanel.Add(sandwichPanel);
                db.SaveChanges();


                try
                {
                    //new EmailCMO(cMO2_Order, login, 1);
                    logger.Debug("CreateOrder / EmailCMO: " + sandwichPanel.id);
                }
                catch (Exception ex)
                {
                    logger.Error("CreateOrder / EmailCMO: " + sandwichPanel.id + " | " + ex);
                }
            }




                //GetBasicFuild(login);
                //GetDefaultWork();
                //GetDefaultManuf();
                //GetDefaultFin();
                //cMO2_Order.reOrder = false;
                //db.CMO2_Order.Add(cMO2_Order);
                //db.SaveChanges();
                //CreateFolderAndFileForPreOrder(cMO2_Order.id);
                //foreach (var dataPZ in id_PlanZakaz)
                //{
                //    foreach (var dataType in id_CMO_TypeProduct)
                //    {
                //        CMO2_Position cMO2_Position = new CMO2_Position();
                //        cMO2_Position.id_PZ_PlanZakaz = dataPZ;
                //        cMO2_Position.id_CMO_TypeProduct = dataType;
                //        cMO2_Position.id_CMO2 = cMO2_Order.id;
                //        db.CMO2_Position.Add(cMO2_Position);
                //    }
                //}
                //db.SaveChanges();

        }

        bool GetBasicFuild(string login)
        {
            sandwichPanel = new SandwichPanel();
            sandwichPanel.datetimeCreate = DateTime.Now;
            sandwichPanel.id_AspNetUsersCreate = new Login().LoginEmailToId(login);
            sandwichPanel.numberOrder = "";
            sandwichPanel.onApprove = false;
            sandwichPanel.onCorrection = false;
            sandwichPanel.onCustomer = false;
            sandwichPanel.onGetDateComplited = false;
            sandwichPanel.onComplited = false;
            return true;
        }










    }
}