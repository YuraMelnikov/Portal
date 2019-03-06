using NLog;
using System;
using System.Linq;

namespace Wiki.Models
{
    public class SMRPNR
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string goodConstructor = "успешная передача/присвоение данных конструктора ";
        private string badConstructor = "ошибка передача/присвоение данных конструктора ";
        private string goodMethod = " метод отработал успешно ";
        private string badMethod = " метод завершился ошибкой ";
        private string goodDB = "успешное сохранение даных на сервере ";
        private string badDB = "ошибка сохранениея даных на сервере ";
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public SMRPNR(int idPZ_PlanZakaz)
        {
            PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(idPZ_PlanZakaz);
            try
            {
                if (pZ_PlanZakaz.SMRPNR != 1)
                {
                    if (db.SMRPNR_Order.Where(d => d.id_PZ_PlanZakaz == pZ_PlanZakaz.Id).Count() > 0)
                        GetTypeUpdate(pZ_PlanZakaz);
                    else
                        GetTypeCreate(pZ_PlanZakaz);
                }
                else
                {
                    logger.Debug(badConstructor + " (Modul SMRPNR - SMRPNR(int idPZ_PlanZakaz); задание на ШМР и ПНР не создано, в связи с тем, что ШМР и ПНР не выполняется");
                }
                logger.Debug(badConstructor + " (Modul SMRPNR - SMRPNR(int idPZ_PlanZakaz); следующий шаг");
            }
            catch(Exception ex)
            {
                logger.Error(goodConstructor + " (Modul SMRPNR - SMRPNR(int idPZ_PlanZakaz);" + ex.Message.ToString());
            }
        }

        public SMRPNR(SMRPNR_Work sMRPNR_Work)
        {
            try
            {
                if (sMRPNR_Work.id_SMRPNR_TypeWork == 1)
                    EditSMR(sMRPNR_Work);
                else
                    EditPNR(sMRPNR_Work);

                logger.Debug(badConstructor + " (Modul SMRPNR - SMRPNR(SMRPNR_Work sMRPNR_Work); следующий шаг");
            }
            catch (Exception ex)
            {
                logger.Error(goodConstructor + " (Modul SMRPNR - SMRPNR(SMRPNR_Work sMRPNR_Work);" + ex.Message.ToString());
            }
        }

        public SMRPNR(int idOrder, int idPlanZakaz)
        {
            try
            {
                SMRPNR_Order sMRPNR_Order = db.SMRPNR_Order.Find(idOrder);
                if (sMRPNR_Order.pNRnull == true)
                    DelitePNR(idOrder);
                if (sMRPNR_Order.oneContractToSMRPNR == true)
                    OneContractSMRAndPNR(idOrder);
                logger.Debug(goodDB + " (Modul SMRPNR - SMRPNR(int idOrder, int idPlanZakaz)");
            }
            catch(Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - SMRPNR(int idOrder, int idPlanZakaz))" + ex.Message.ToString());
            }
        }
        
        private void GetTypeCreate(PZ_PlanZakaz pZ_PlanZakaz)
        {
            if (pZ_PlanZakaz.SMRPNR == 2)
                CreateTwo(pZ_PlanZakaz);
            if (pZ_PlanZakaz.SMRPNR == 3)
                CreateThree(pZ_PlanZakaz);
            if (pZ_PlanZakaz.SMRPNR == 4)
                CreateFour(pZ_PlanZakaz);
        }

        private void GetTypeUpdate(PZ_PlanZakaz pZ_PlanZakaz)
        {
            if (pZ_PlanZakaz.SMRPNR == 2)
                UpdateTwo(pZ_PlanZakaz);
            if (pZ_PlanZakaz.SMRPNR == 3)
                UpdateThree(pZ_PlanZakaz);
            if (pZ_PlanZakaz.SMRPNR == 4)
                UpdateFour(pZ_PlanZakaz);
        }
        
        private void CreateTwo(PZ_PlanZakaz idPZ_PlanZakaz)
        {
            try
            {
                SMRPNR_Order sMRPNR_Order = CreateDefaultSMRPNR_Order(idPZ_PlanZakaz);
                db.SMRPNR_Order.Add(sMRPNR_Order);
                db.SaveChanges();
                SMRPNR_Work sMR_Work = GetNewTaskSMRPNR(sMRPNR_Order, db.PZ_PlanZakaz.Find(idPZ_PlanZakaz));
                db.SMRPNR_Work.Add(sMR_Work);
                db.SaveChanges();
                SMRPNR_Work pNR_Work = GetNewTaskSMRPNR(sMRPNR_Order, db.PZ_PlanZakaz.Find(idPZ_PlanZakaz));
                pNR_Work.contract = "";
                pNR_Work.contractAtt = "";
                pNR_Work.dateContract = null;
                pNR_Work.dateContractAtt = null;
                pNR_Work.id_SMRPNR_TypeWork = 2;
                db.SMRPNR_Work.Add(pNR_Work);
                db.SaveChanges();
                logger.Debug(goodDB + " (Modul SMRPNR - CreateTwo(PZ_PlanZakaz idPZ_PlanZakaz)");
            }
            catch(Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - CreateTwo(PZ_PlanZakaz idPZ_PlanZakaz)" + ex.Message.ToString());
            }
        }

        private void CreateThree(PZ_PlanZakaz idPZ_PlanZakaz)
        {
            try
            {
                SMRPNR_Order sMRPNR_Order = CreateDefaultSMRPNR_Order(idPZ_PlanZakaz);
                db.SMRPNR_Order.Add(sMRPNR_Order);
                db.SaveChanges();
                SMRPNR_Work sMR_Work = GetNewTaskSMRPNR(sMRPNR_Order, db.PZ_PlanZakaz.Find(idPZ_PlanZakaz));
                sMR_Work.contract = "";
                sMR_Work.contractAtt = "";
                sMR_Work.dateContract = null;
                sMR_Work.dateContractAtt = null;
                db.SMRPNR_Work.Add(sMR_Work);
                db.SaveChanges();
                SMRPNR_Work pNR_Work = GetNewTaskSMRPNR(sMRPNR_Order, db.PZ_PlanZakaz.Find(idPZ_PlanZakaz));
                pNR_Work.contract = "";
                pNR_Work.contractAtt = "";
                pNR_Work.dateContract = null;
                pNR_Work.dateContractAtt = null;
                pNR_Work.id_SMRPNR_TypeWork = 2;
                db.SMRPNR_Work.Add(pNR_Work);
                db.SaveChanges();
                logger.Debug(goodDB + " (Modul SMRPNR - CreateThree(PZ_PlanZakaz idPZ_PlanZakaz)");
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - CreateThree(PZ_PlanZakaz idPZ_PlanZakaz)" + ex.Message.ToString());
            }
        }

        private void CreateFour(PZ_PlanZakaz idPZ_PlanZakaz)
        {
            try
            {
                SMRPNR_Order sMRPNR_Order = CreateDefaultSMRPNR_Order(idPZ_PlanZakaz);
                sMRPNR_Order.oneContractToSMRPNR = true;
                db.SMRPNR_Order.Add(sMRPNR_Order);
                db.SaveChanges();
                SMRPNR_Work sMR_Work = GetNewTaskSMRPNR(sMRPNR_Order, db.PZ_PlanZakaz.Find(idPZ_PlanZakaz));
                db.SMRPNR_Work.Add(sMR_Work);
                db.SaveChanges();
                SMRPNR_Work pNR_Work = GetNewTaskSMRPNR(sMRPNR_Order, db.PZ_PlanZakaz.Find(idPZ_PlanZakaz));
                pNR_Work.id_SMRPNR_TypeWork = 2;
                db.SMRPNR_Work.Add(pNR_Work);
                db.SaveChanges();
                logger.Debug(goodDB + " (Modul SMRPNR - CreateFour(PZ_PlanZakaz idPZ_PlanZakaz)");
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - CreateFour(PZ_PlanZakaz idPZ_PlanZakaz)" + ex.Message.ToString());
            }
        }
        
        private void UpdateTwo(PZ_PlanZakaz idPZ_PlanZakaz)
        {
            try
            {
                SMRPNR_Order sMRPNR_Order = db.SMRPNR_Order.Where(d => d.id_PZ_PlanZakaz == idPZ_PlanZakaz.Id).First();
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(idPZ_PlanZakaz);
                sMRPNR_Order = UpdateDefaultSMRPNR_Order(pZ_PlanZakaz, sMRPNR_Order);
                db.Entry(sMRPNR_Order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                
                SMRPNR_Work sMR_Work = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == sMRPNR_Order.id & d.id_SMRPNR_TypeWork == 1).First();
                sMR_Work.id_PZ_Client = pZ_PlanZakaz.Client;
                sMR_Work.contract = pZ_PlanZakaz.timeContract;
                sMR_Work.dateContract = pZ_PlanZakaz.timeContractDate;
                sMR_Work.contractAtt = pZ_PlanZakaz.timeContract;
                sMR_Work.dateContractAtt = pZ_PlanZakaz.timeArrDate;
                db.Entry(sMR_Work).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                SMRPNR_Work pNR_Work = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == sMRPNR_Order.id & d.id_SMRPNR_TypeWork == 2).First();
                pNR_Work.id_PZ_Client = pZ_PlanZakaz.Client;
                pNR_Work.contract = null;
                pNR_Work.dateContract = null;
                pNR_Work.contractAtt = null;
                pNR_Work.dateContractAtt = null;
                db.Entry(pNR_Work).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                logger.Debug(goodDB + " (Modul SMRPNR - UpdateTwo(PZ_PlanZakaz idPZ_PlanZakaz)");
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - UpdateTwo(PZ_PlanZakaz idPZ_PlanZakaz)" + ex.Message.ToString());
            }
        }

        private void UpdateThree(PZ_PlanZakaz idPZ_PlanZakaz)
        {
            try
            {
                SMRPNR_Order sMRPNR_Order = db.SMRPNR_Order.Where(d => d.id_PZ_PlanZakaz == idPZ_PlanZakaz.Id).First();
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(idPZ_PlanZakaz);
                sMRPNR_Order = UpdateDefaultSMRPNR_Order(pZ_PlanZakaz, sMRPNR_Order);
                db.Entry(sMRPNR_Order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                SMRPNR_Work sMR_Work = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == sMRPNR_Order.id & d.id_SMRPNR_TypeWork == 1).First();
                sMR_Work.id_PZ_Client = pZ_PlanZakaz.Client;
                sMR_Work.contract = null;
                sMR_Work.dateContract = null;
                sMR_Work.contractAtt = null;
                sMR_Work.dateContractAtt = null;
                db.Entry(sMR_Work).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                SMRPNR_Work pNR_Work = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == sMRPNR_Order.id & d.id_SMRPNR_TypeWork == 2).First();
                pNR_Work.id_PZ_Client = pZ_PlanZakaz.Client;
                pNR_Work.contract = null;
                pNR_Work.dateContract = null;
                pNR_Work.contractAtt = null;
                pNR_Work.dateContractAtt = null;
                db.Entry(pNR_Work).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                logger.Debug(goodDB + " (Modul SMRPNR - UpdateThree(PZ_PlanZakaz idPZ_PlanZakaz)");
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - UpdateThree(PZ_PlanZakaz idPZ_PlanZakaz)" + ex.Message.ToString());
            }
        }

        private void UpdateFour(PZ_PlanZakaz idPZ_PlanZakaz)
        {
            try
            {
                SMRPNR_Order sMRPNR_Order = db.SMRPNR_Order.Where(d => d.id_PZ_PlanZakaz == idPZ_PlanZakaz.Id).First();
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(idPZ_PlanZakaz);
                sMRPNR_Order = UpdateDefaultSMRPNR_Order(pZ_PlanZakaz, sMRPNR_Order);
                db.Entry(sMRPNR_Order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                SMRPNR_Work sMR_Work = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == sMRPNR_Order.id & d.id_SMRPNR_TypeWork == 1).First();
                sMR_Work.id_PZ_Client = pZ_PlanZakaz.Client;
                sMR_Work.contract = pZ_PlanZakaz.timeContract;
                sMR_Work.dateContract = pZ_PlanZakaz.timeContractDate;
                sMR_Work.contractAtt = pZ_PlanZakaz.timeContract;
                sMR_Work.dateContractAtt = pZ_PlanZakaz.timeArrDate;
                db.Entry(sMR_Work).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                SMRPNR_Work pNR_Work = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == sMRPNR_Order.id & d.id_SMRPNR_TypeWork == 2).First();
                pNR_Work.id_PZ_Client = pZ_PlanZakaz.Client;
                pNR_Work.contract = pZ_PlanZakaz.timeContract;
                pNR_Work.dateContract = pZ_PlanZakaz.timeContractDate;
                pNR_Work.contractAtt = pZ_PlanZakaz.timeContract;
                pNR_Work.dateContractAtt = pZ_PlanZakaz.timeArrDate;
                db.Entry(pNR_Work).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                logger.Debug(goodDB + " (Modul SMRPNR - UpdateTwo(PZ_PlanZakaz idPZ_PlanZakaz)");
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - UpdateTwo(PZ_PlanZakaz idPZ_PlanZakaz)" + ex.Message.ToString());
            }
        }

        private SMRPNR_Order UpdateDefaultSMRPNR_Order(PZ_PlanZakaz idPZ_PlanZakaz, SMRPNR_Order sMRPNR_Order)
        {
            try
            {
                if (idPZ_PlanZakaz.Naznachenie == null)
                    sMRPNR_Order.@object = "";
                else
                    sMRPNR_Order.@object = idPZ_PlanZakaz.Naznachenie;
                sMRPNR_Order.oneContractToSMRPNR = false;
                logger.Debug(goodMethod + " (Modul SMRPNR - UpdateDefaultSMRPNR_Order(PZ_PlanZakaz idPZ_PlanZakaz, SMRPNR_Order sMRPNR_Order) идем следующий шаг");
                return sMRPNR_Order;
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul SMRPNR - UpdateDefaultSMRPNR_Order(PZ_PlanZakaz idPZ_PlanZakaz, SMRPNR_Order sMRPNR_Order)" + ex.Message.ToString());
                return null;
            }
        }

        private SMRPNR_Work GetNewTaskSMRPNR(SMRPNR_Order sMRPNR_Order, PZ_PlanZakaz pZ_PlanZakaz)
        {
            try
            {
                SMRPNR_Work sMRPNR_Work = new SMRPNR_Work();
                sMRPNR_Work.id_SMRPNR_TypeWork = 1;
                sMRPNR_Work.id_SMRPNR_Order = sMRPNR_Order.id;
                sMRPNR_Work.id_SMRPNR_SMRState = 1;
                sMRPNR_Work.id_PZ_Client = pZ_PlanZakaz.Client;
                sMRPNR_Work.contract = pZ_PlanZakaz.timeContract;
                sMRPNR_Work.dateContract = Convert.ToDateTime(pZ_PlanZakaz.timeContractDate);
                sMRPNR_Work.contractAtt = pZ_PlanZakaz.timeArr;
                sMRPNR_Work.dateContractAtt = Convert.ToDateTime(pZ_PlanZakaz.timeArrDate);
                logger.Debug(goodMethod + " (Modul SMRPNR - GetNewTaskSMRPNR(SMRPNR_Order sMRPNR_Order, PZ_PlanZakaz pZ_PlanZakaz) идем следующий шаг");
                return sMRPNR_Work;
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul SMRPNR - GetNewTaskSMRPNR(SMRPNR_Order sMRPNR_Order, PZ_PlanZakaz pZ_PlanZakaz)" + ex.Message.ToString());
                return null;
            }

        }

        private SMRPNR_Order CreateDefaultSMRPNR_Order(PZ_PlanZakaz idPZ_PlanZakaz)
        {
            try
            {
                SMRPNR_Order sMRPNR_Order = new SMRPNR_Order();
                sMRPNR_Order.id_PZ_PlanZakaz = idPZ_PlanZakaz.Id;
                if (idPZ_PlanZakaz.Naznachenie == null)
                    sMRPNR_Order.@object = "";
                else
                    sMRPNR_Order.@object = idPZ_PlanZakaz.Naznachenie;
                sMRPNR_Order.adress = "";
                sMRPNR_Order.oneContractToSMRPNR = false;
                sMRPNR_Order.costWhisNDS = 0;
                sMRPNR_Order.costWhisNDSPNR = 0;
                sMRPNR_Order.description = "";
                sMRPNR_Order.complited = false;
                sMRPNR_Order.pNRnull = false;
                logger.Debug(goodMethod + " (Modul SMRPNR - CreateDefaultSMRPNR_Order(PZ_PlanZakaz idPZ_PlanZakaz) идем следующий шаг");
                return sMRPNR_Order;
            }
            catch (Exception ex)
            {
                logger.Error(badMethod + " (Modul SMRPNR - CreateDefaultSMRPNR_Order(PZ_PlanZakaz idPZ_PlanZakaz)" + ex.Message.ToString());
                return null;
            }
        }

        private void DelitePNR(int idOrder)
        {
            try
            {
                SMRPNR_Work sMRPNR_Work = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == idOrder & d.id_SMRPNR_TypeWork == 2).FirstOrDefault();
                if (sMRPNR_Work != null)
                {
                    db.SMRPNR_Work.Remove(sMRPNR_Work);
                    db.SaveChanges();
                }
                logger.Debug(goodDB + " (Modul SMRPNR - DelitePNR(int idOrder)");
            }
            catch(Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - DelitePNR(int idOrder)" + ex.Message.ToString());
            }
        }

        private void OneContractSMRAndPNR(int idOrder)
        {
            try
            {
                SMRPNR_Work sMRPNR_WorkSMR = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == idOrder & d.id_SMRPNR_TypeWork == 1).FirstOrDefault();
                SMRPNR_Work sMRPNR_WorkPNR = db.SMRPNR_Work.Where(d => d.id_SMRPNR_Order == idOrder & d.id_SMRPNR_TypeWork == 2).FirstOrDefault();
                if (sMRPNR_WorkSMR != null & sMRPNR_WorkPNR != null)
                {
                    sMRPNR_WorkPNR.contract = sMRPNR_WorkSMR.contract;
                    sMRPNR_WorkPNR.contractAtt = sMRPNR_WorkSMR.contractAtt;
                    sMRPNR_WorkPNR.dateContractAtt = sMRPNR_WorkSMR.dateContractAtt;
                    sMRPNR_WorkPNR.dateContract = sMRPNR_WorkSMR.dateContract;
                    sMRPNR_WorkPNR.id_PZ_Client = sMRPNR_WorkSMR.id_PZ_Client;
                    db.Entry(sMRPNR_WorkPNR).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                logger.Debug(goodDB + " (Modul SMRPNR - OneContractSMRAndPNR(int idOrder)");
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - OneContractSMRAndPNR(int idOrder)" + ex.Message.ToString());
            }
        }
        
        private void EditSMR(SMRPNR_Work sMRPNR_Work)
        {
            try
            {
                if (sMRPNR_Work.dateFirstCall == null)
                    sMRPNR_Work.id_SMRPNR_SMRState = 2;
                if (sMRPNR_Work.dateFirstCall != null)
                    sMRPNR_Work.id_SMRPNR_SMRState = 3;
                if (sMRPNR_Work.datePlan != null & sMRPNR_Work.datePlan < DateTime.Now)
                    sMRPNR_Work.id_SMRPNR_SMRState = 4;
                if (sMRPNR_Work.dateFirstCall != null & sMRPNR_Work.dateFirstCall < DateTime.Now & sMRPNR_Work.datePlan < DateTime.Now)
                    sMRPNR_Work.id_SMRPNR_SMRState = 5;
                if (sMRPNR_Work.dateTechDoc != null)
                    sMRPNR_Work.id_SMRPNR_SMRState = 6;
                if (sMRPNR_Work.dateFinDoc != null)
                    sMRPNR_Work.id_SMRPNR_SMRState = 7;
                if (sMRPNR_Work.dateGetMoney != null)
                {
                    sMRPNR_Work.getCashComplited = true;
                    sMRPNR_Work.id_SMRPNR_SMRState = 8;
                }
                db.Entry(sMRPNR_Work).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                SMRPNR_Order sMRPNR_Order = db.SMRPNR_Order.Find(sMRPNR_Work.id_SMRPNR_Order);
                if (sMRPNR_Order.oneContractToSMRPNR == true)
                    OneContractSMRAndPNR(sMRPNR_Order.id);
                logger.Debug(goodDB + " (Modul SMRPNR - EditSMR(SMRPNR_Work sMRPNR_Work)");
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - EditSMR(SMRPNR_Work sMRPNR_Work)" + ex.Message.ToString());
            }
        }

        private void EditPNR(SMRPNR_Work sMRPNR_Work)
        {
            try
            {
                if (sMRPNR_Work.dateFirstCall == null)
                    sMRPNR_Work.id_SMRPNR_SMRState = 2;
                if (sMRPNR_Work.dateFirstCall != null)
                    sMRPNR_Work.id_SMRPNR_SMRState = 3;
                if (sMRPNR_Work.datePlan != null & sMRPNR_Work.datePlan < DateTime.Now)
                    sMRPNR_Work.id_SMRPNR_SMRState = 9;
                if (sMRPNR_Work.dateFirstCall != null & sMRPNR_Work.dateFirstCall < DateTime.Now & sMRPNR_Work.datePlan < DateTime.Now)
                    sMRPNR_Work.id_SMRPNR_SMRState = 5;
                if (sMRPNR_Work.dateTechDoc != null)
                    sMRPNR_Work.id_SMRPNR_SMRState = 6;
                if (sMRPNR_Work.dateFinDoc != null)
                    sMRPNR_Work.id_SMRPNR_SMRState = 7;
                if (sMRPNR_Work.dateGetMoney != null)
                {
                    sMRPNR_Work.getCashComplited = true;
                    sMRPNR_Work.id_SMRPNR_SMRState = 8;
                }
                db.Entry(sMRPNR_Work).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                logger.Debug(goodDB + " (Modul SMRPNR - EditPNR(SMRPNR_Work sMRPNR_Work)");
            }
            catch (Exception ex)
            {
                logger.Error(badDB + " (Modul SMRPNR - EditPNR(SMRPNR_Work sMRPNR_Work)" + ex.Message.ToString());
            }
        }
    }
}