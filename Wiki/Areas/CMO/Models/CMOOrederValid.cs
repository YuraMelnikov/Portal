using System;
using Wiki.Models;

namespace Wiki.Areas.CMO.Models
{
    public class CMOOrederValid
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        int id_TypeReOrder = 10;
        CMO2_Order cMO2_Order;
        public void CreateOrder(int[] id_PlanZakaz, int[] id_CMO_TypeProduct, string login)
        {
            GetBasicFuild(login);
            GetDefaultWork();
            GetDefaultManuf();
            GetDefaultFin();
            cMO2_Order.reOrder = false;
            db.CMO2_Order.Add(cMO2_Order);
            db.SaveChanges();
            foreach(var dataPZ in id_PlanZakaz)
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
        }

        public void CreateReOrder(int[] id_PlanZakaz, int id_CMO_Company, string login)
        {
            GetBasicFuild(login);
            GetDefaultWork();
            GetDefaultManuf();
            GetDefaultFin();
            cMO2_Order.reOrder = true;
            cMO2_Order.id_CMO_Company = id_CMO_Company;
            db.CMO2_Order.Add(cMO2_Order);
            db.SaveChanges();
            foreach (var dataPZ in id_PlanZakaz)
            {
                CMO2_Position cMO2_Position = new CMO2_Position();
                cMO2_Position.id_PZ_PlanZakaz = dataPZ;
                cMO2_Position.id_CMO_TypeProduct = id_TypeReOrder;
                cMO2_Position.id_CMO2 = cMO2_Order.id;
                db.CMO2_Position.Add(cMO2_Position);
            }
            db.SaveChanges();
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
    }
}