using NLog;
using System;

namespace Wiki.Models
{
    public class ReclamationOrder
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private PortalKATEKEntities db = new PortalKATEKEntities();

        private string goodDB = "успешное сохранение даных на сервере ";
        private string badDB = "ошибка сохранениея даных на сервере ";


        public ReclamationOrder(int[] pZiDArray, string loginIdCreate, Reclamation_Order reclamation_Order)
        {
            try
            {
                reclamation_Order.dateCreate = DateTime.Now;
                reclamation_Order.userCreate = loginIdCreate;
                reclamation_Order.close = false;
                reclamation_Order.closeSMR = false;
                db.Reclamation_Order.Add(reclamation_Order);
                db.SaveChanges();
                logger.Debug(goodDB + " (Model - ReclamationOrder; db.Reclamation_Order.Add(reclamation_Order): " + loginIdCreate);
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Model - ReclamationOrder; db.Reclamation_Order.Add(reclamation_Order): " + loginIdCreate + ex.Message.ToString());
            }
            try
            {
                for (int i = 0; i < pZiDArray.Length; i++)
                {
                    Reclamation_PZOrder reclamation_PZOrder = new Reclamation_PZOrder();
                    reclamation_PZOrder.id_PZ_PlanZakaz = pZiDArray[i];
                    reclamation_PZOrder.id_Reclamation_Order = reclamation_Order.id;
                    db.Reclamation_PZOrder.Add(reclamation_PZOrder);
                    db.SaveChanges();
                    logger.Debug(goodDB + " (Model - ReclamationOrder; db.Reclamation_PZOrder.Add(reclamation_PZOrder): " + loginIdCreate);
                }
            }
            catch (Exception ex)
            {
                logger.Error(badDB + "Model - ReclamationOrder; db.Reclamation_PZOrder.Add(reclamation_PZOrder) " + loginIdCreate + ex.Message.ToString());
            }
        }
    }
}