using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class CMOController : Controller
    {
        private readonly string getServer = "получены данные от сервера ";
        private readonly string postServer = "отправлены данные от сервера ";
        private readonly string getServerError = "ошибка получения данныех от сервера ";
        private readonly string postServerError = "ошибка отправки данных от сервера ";

        private PortalKATEKEntities db = new PortalKATEKEntities();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Authorize(Roles = "Admin, KBMUser, KBM, KBMUser, Technologist, KBE, KBEUser, GR, OPTP, OTK, Manufacturing, Sklad, OP, OS")]
        public ActionResult Index()
        {
            int data = 0;
            try
            {
                string login = HttpContext.User.Identity.Name;
                data = Convert.ToInt32(db.AspNetUsers.Where(d => d.Email == login).First().Devision);
                logger.Debug("получен логин при входе в модуль ЦМО (CMOController/Index): " + login.ToString());
            }
            catch (Exception ex)
            {
                logger.Error("ошибка входа в модуль ЦМО (CMOController/Index): " + ex.Message.ToString());
            }
            if (data == 7)
                return RedirectToAction("ViewStartMenuOS", "CMO");
            if (data == 15)
                return RedirectToAction("ViewStartMenuKO", "CMO");
            else
                return RedirectToAction("TestTableOS", "CMO");
        }

        //[Authorize(Roles = "Admin, OS")]
        //public ActionResult ViewStartMenuOS()
        //{
        //    string login = HttpContext.User.Identity.Name;
        //    ViewOSCMO viewOSCMO = new ViewOSCMO();
        //    try
        //    {

        //        var defaultOrderNotComplited = db.CMO_Order
        //            .Where(d => d.datetimeFirstTenderFinish == null)
        //            .Where(d => d.dateCloseOrder != null)
        //            .Where(d => d.companyWin > 0)
        //            .ToList();
        //        var activeOrder = db.CMO_PreOrder.Where(d => d.firstTenderStart == false).ToList();
        //        var activeFirstUpload = db.CMO_UploadResult.Where(d => d.dateComplited == null & d.CMO_Tender.id_CMO_TypeTask == 1).ToList();
        //        var activeFirstTender = db.CMO_Tender.Where(d => d.id_CMO_TypeTask == 1 & d.close == false).ToList();
        //        var activeSecondUpload = db.CMO_UploadResult.Where(d => d.dateComplited == null & d.CMO_Tender.id_CMO_TypeTask == 2).ToList();
        //        var activeSecondTender = db.CMO_Tender.Where(d => d.id_CMO_TypeTask == 2 & d.close == false).ToList();
        //        var activeOrderClose = db.CMO_Report.Where(d => d.FactFinishDate == null & d.PlanFinishWork1 != null).ToList();
        //        viewOSCMO.ActiveOrder = activeOrder;
        //        viewOSCMO.ActiveFirstUpload = activeFirstUpload;
        //        viewOSCMO.ActiveFirstTender = activeFirstTender;
        //        viewOSCMO.ActiveSecondUpload = activeSecondUpload;
        //        viewOSCMO.ActiveSecondTender = activeSecondTender;
        //        viewOSCMO.ActiveOrderClose = activeOrderClose;
        //        viewOSCMO.DefaultOrderNotComplited = defaultOrderNotComplited;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("ошибка получения данных от сервера (CMOController/ViewStartMenuOS): " + ex.Message.ToString());
        //    }
        //    return View(viewOSCMO);
        //}
        //[Authorize(Roles = "Admin, KBM, KBMUser")]
        //public ActionResult ViewStartMenuKO()
        //{
        //    string login = HttpContext.User.Identity.Name;
        //    try
        //    {
        //        var data = db.CMO_Order.OrderByDescending(d => d.id);
        //        logger.Debug("получены данные от сервера (CMOController/ViewStartMenuKO): " + login.ToString());
        //        return View(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("ошибка получения данных от сервера (CMOController/ViewStartMenuKO): " + ex.Message.ToString());
        //        return RedirectToAction("Error", "CMO");
        //    }
        //}

        [Authorize(Roles = "Admin, Technologist, OS")]
        public ActionResult TestTableOS()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                var data = db.CMO_Report.OrderByDescending(d => d.Id_Order).ToList();
                logger.Debug("получены данные от сервера (CMOController/ViewStartMenuKO): " + login.ToString());
                return View(data);
            }
            catch (Exception ex)
            {
                logger.Error("ошибка получения данных от сервера (CMOController/ViewStartMenuKO): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        [Authorize(Roles = "Admin, KBMUser, KBM, KBMUser, Technologist, KBE, KBEUser, GR, OPTP, OTK, Manufacturing, Sklad, OP, OS")]
        public ActionResult TestTableOS2()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {


                var data = db.CMO_Order.Where(d => d.CMO_Tender.Count > 0).OrderByDescending(d => d.CMO_PositionOrder.FirstOrDefault().PZ_PlanZakaz.PlanZakaz).ToList();
                logger.Debug("получены данные от сервера (CMOController/ViewStartMenuKO): " + login.ToString());
                return View(data);
            }
            catch (Exception ex)
            {
                logger.Error("ошибка получения данных от сервера (CMOController/ViewStartMenuKO): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        public ActionResult Error()
        {
            string login = HttpContext.User.Identity.Name;
            logger.Fatal("(CMOController/Error)");
            return RedirectToAction("ViewStartMenuOS", "CMO");
        }

        [Authorize(Roles = "Admin, KBMUser, KBM, KBMUser")]
        public ActionResult CreateOrder()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
                ViewBag.id_PlanZakaz1 = new SelectList(db.CMO_TypeProduct.Where(d => d.active == true), "id", "name");
                logger.Debug("получены данные от сервера (CMOController/CreateOrder): " + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error("ошибка получения данных от сервера (CMOController/CreateOrder): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(HttpPostedFileBase[] file1, int[] PlanZakaz, int[] CMO_TypeProduct)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                foreach (var x in db.AspNetUsers)
                {
                    if (login == x.UserName.ToString())
                    {
                        login = x.Id;
                        break;
                    }
                }
                CMO_Create cMO_Create = new CMO_Create(file1, PlanZakaz, CMO_TypeProduct, login);
                logger.Debug("отправка данных на сервер (CMOController/CreateOrder): " + login.ToString());
                return RedirectToAction("ViewStartMenuKO", "CMO");
            }
            catch (Exception ex)
            {
                logger.Error("ошибка отправки данных от сервера (CMOController/CreateOrder): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        [Authorize(Roles = "Admin, KBMUser, KBM, KBMUser")]
        public ActionResult CreateOrderDefault()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
                ViewBag.CMO_Company1 = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
                logger.Debug(getServer + " (CMOController/CreateOrderDefault): " + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + " (CMOController/CreateOrderDefault): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrderDefault(HttpPostedFileBase[] file1, int[] PlanZakaz, int[] CMO_Company1)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                int ind = CMO_Company1[0];
                CMO_Company cMO_Company = db.CMO_Company.Where(d => d.id == ind).First();
                foreach (var x in db.AspNetUsers)
                {
                    if (login == x.UserName.ToString())
                    {
                        login = x.Id;
                        break;
                    }
                }
                CMO_Create cMO_Create = new CMO_Create(file1, PlanZakaz, login, cMO_Company);
                ViewBag.CMO_Company = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
                logger.Debug(postServer + " (CMOController/CreateOrderDefault): " + login.ToString());
                return RedirectToAction("ViewStartMenuKO", "CMO");
            }
            catch (Exception ex)
            {
                logger.Error(postServerError + " (CMOController/CreateOrderDefault): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        public ActionResult StartFirstTender()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                List<CMO_PreOrderView> cMO_PreOrderViews = new List<CMO_PreOrderView>();
                foreach (var preOreder in db.CMO_PreOrder.Where(d => d.firstTenderStart == false).ToList())
                {
                    cMO_PreOrderViews.Add(new CMO_PreOrderView(preOreder));
                }
                ViewBag.preOrder = new SelectList(cMO_PreOrderViews, "Id", "NamePositions");
                logger.Debug(getServer + "CMOController - StartFirstTender: " + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + "CMOController - StartFirstTender: " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        public ActionResult StartFirstTender(int[] preOrder, CMO_Order cMO_Order)
        {
            if(preOrder == null)
                return RedirectToAction("ViewStartMenuOS", "CMO");
            if(cMO_Order.dateCreate < DateTime.Now)
                return RedirectToAction("ViewStartMenuOS", "CMO");
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_PreOrder[] cMO_PreOrders = new CMO_PreOrder[preOrder.Length];
                for (int i = 0; i < preOrder.Length; i++)
                {
                    int index = preOrder[i];
                    cMO_PreOrders[i] = db.CMO_PreOrder.First(d => d.id == index);
                }
                CMO_Create cMO_Create = new CMO_Create(cMO_PreOrders, login);
                cMO_Create.CMO_StartFirstTender(cMO_Order.dateCreate);
                logger.Debug(postServer + " (CMOController/StartFirstTender): " + login.ToString());
                return RedirectToAction("ViewStartMenuOS", "CMO");
            }
            catch (Exception ex)
            {
                logger.Error(postServerError + " (CMOController/StartFirstTender): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        public ActionResult UploadDataCompany(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_UploadResult cMO_UploadResult = db.CMO_UploadResult.Find(id);
                ViewBag.Order = cMO_UploadResult.CMO_Tender.id_CMO_Order.ToString();
                ViewBag.Company = cMO_UploadResult.CMO_Company.name.ToString();
                string dataPositionOrder = "";

                var dataListPosition = cMO_UploadResult.CMO_Tender.CMO_Order.CMO_PositionOrder.ToList();
                foreach (var data in dataListPosition)
                {
                    dataPositionOrder += data.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + data.CMO_TypeProduct.name + ";";
                }

                ViewBag.Position = dataPositionOrder;
                logger.Debug(getServer + " (CMOController/UploadDataCompany): " + login.ToString());
                return View(cMO_UploadResult);
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + " (CMOController/UploadDataCompany): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDataCompany(CMO_UploadResult cMO_UploadResult)
        {
            string login = HttpContext.User.Identity.Name;

            if(cMO_UploadResult.cost == 0)
                return RedirectToAction("UploadDataCompany", "CMO", new { cMO_UploadResult.id});
            if (cMO_UploadResult.day == 0 || cMO_UploadResult.day == null)
                return RedirectToAction("UploadDataCompany", "CMO", new { cMO_UploadResult.id });
            try
            {
                cMO_UploadResult.dateComplited = DateTime.Now.AddDays((int)cMO_UploadResult.day);
                cMO_UploadResult.dateTimeUpload = DateTime.Now;
                db.Entry(cMO_UploadResult).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug(postServer + " (CMOController/UploadDataCompany): " + login.ToString());
                return RedirectToAction("ViewStartMenuOS", "CMO");
            }
            catch (Exception ex)
            {
                logger.Error(postServerError + " (CMOController/UploadDataCompany): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        public ActionResult FinishFirstTender(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_Order cMO_Order = db.CMO_Order.Find(id);
                ViewBag.numOrder = cMO_Order.id.ToString();
                ViewBag.DateTimeCreate = cMO_Order.dateCreate;
                ViewBag.UserCreate = db.AspNetUsers.Find(cMO_Order.userCreate).CiliricalName.ToString();
                ViewBag.idTime = new SelectList(db.CMO_HoureTender.Where(d => d.active == true).OrderBy(d => d.count).ToList(), "id", "name", cMO_Order.idTime);
                ViewBag.tableResult = db.CMO_UploadResult.Where(d => d.CMO_Tender.CMO_Order.id == cMO_Order.id & d.CMO_Tender.id_CMO_TypeTask == 1).ToList();
                List<CMO_Description> cMO_DescriptionsList = new List<CMO_Description>();
                foreach (var data in cMO_Order.CMO_PositionOrder.ToList())
                {
                    cMO_DescriptionsList.Add(new CMO_Description(data.PZ_PlanZakaz.PlanZakaz.ToString(), data.CMO_TypeProduct.name));
                }
                ViewBag.OrderDescription = cMO_DescriptionsList;
                logger.Debug(getServer + " (CMOController/FinishFirstTender): " + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + " (CMOController/FinishFirstTender): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        public ActionResult FinishFirstTender(CMO_Order cMO_Order)
        {
            string login = HttpContext.User.Identity.Name;

            int idTender = db.CMO_Tender
                .Where(d => d.id_CMO_Order == cMO_Order.id)
                .Where(d => d.id_CMO_TypeTask == 1)
                .First().id;
            if(db.CMO_UploadResult.Where(d => d.id_CMO_Tender == idTender & d.dateTimeUpload != null).Max(d => d.cost) == 0)
                return RedirectToAction("ViewStartMenuOS", "CMO");

            try
            {
                ViewBag.idTime = new SelectList(db.CMO_HoureTender.Where(d => d.active == true).OrderBy(d => d.count).ToList(), "id", "name", cMO_Order.idTime);
                CMO_Create cMO_Create = new CMO_Create(cMO_Order.id);
                cMO_Create.CMO_FinishFirstStartSecond(cMO_Order.dateCreate);
                logger.Debug(postServer + " (CMOController/FinishFirstTender): " + login.ToString());
                return RedirectToAction("ViewStartMenuOS", "CMO");
            }
            catch (Exception ex)
            {
                logger.Error(postServerError + " (CMOController/FinishFirstTender): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        public ActionResult FinishSecondTender(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_Order cMO_Order = db.CMO_Order.Find(id);
                ViewBag.numOrder = cMO_Order.id.ToString();
                ViewBag.DateTimeCreate = cMO_Order.dateCreate;
                ViewBag.UserCreate = db.AspNetUsers.Find(cMO_Order.userCreate).CiliricalName.ToString();
                ViewBag.tableResult = db.CMO_UploadResult.Where(d => d.CMO_Tender.CMO_Order.id == cMO_Order.id & d.CMO_Tender.id_CMO_TypeTask == 2).ToList();
                List<CMO_Description> cMO_DescriptionsList = new List<CMO_Description>();
                foreach (var data in cMO_Order.CMO_PositionOrder.ToList())
                {
                    cMO_DescriptionsList.Add(new CMO_Description(data.PZ_PlanZakaz.PlanZakaz.ToString(), data.CMO_TypeProduct.name));
                }
                ViewBag.OrderDescription = cMO_DescriptionsList;
                logger.Debug(getServer + " (CMOController/FinishSecondTender): " + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + " (CMOController/FinishSecondTender): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        public ActionResult FinishSecondTender(CMO_Order cMO_Order)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                int idTender = db.CMO_Tender
                    .Where(d => d.id_CMO_Order == cMO_Order.id)
                    .Where(d => d.id_CMO_TypeTask == 2)
                    .First().id;
                if (db.CMO_UploadResult.Where(d => d.id_CMO_Tender == idTender & d.dateTimeUpload != null).Max(d => d.cost) == 0)
                    return RedirectToAction("ViewStartMenuOS", "CMO");


                CMO_Create cMO_Create = new CMO_Create(cMO_Order.id);
                cMO_Create.CMO_FinishTender();
                logger.Debug(postServer + " (CMOController/FinishSecondTender): " + login.ToString());
                return RedirectToAction("ViewStartMenuOS", "CMO");
            }
            catch (Exception ex)
            {
                logger.Error(postServerError + " (CMOController/FinishSecondTender): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        public ActionResult CloseOrder(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_Order cMO_Order = db.CMO_Order.Find(id);
                ViewBag.numOrder = cMO_Order.id.ToString();
                ViewBag.DateTimeCreate = cMO_Order.dateCreate;
                ViewBag.UserCreate = db.AspNetUsers.Find(cMO_Order.userCreate).CiliricalName.ToString();
                List<CMO_Description> cMO_DescriptionsList = new List<CMO_Description>();
                foreach (var data in cMO_Order.CMO_PositionOrder.ToList())
                {
                    cMO_DescriptionsList.Add(new CMO_Description(data.PZ_PlanZakaz.PlanZakaz.ToString(), data.CMO_TypeProduct.name));
                }
                ViewBag.OrderDescription = cMO_DescriptionsList;
                logger.Debug(getServer + " (CMOController/CloseOrder): " + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + " (CMOController/CloseOrder): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        public ActionResult CloseOrder(CMO_Order cMO_Order)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_Order cMO = db.CMO_Order.Find(cMO_Order.id);
                cMO.dateCloseOrder = cMO_Order.dateCloseOrder;
                db.Entry(cMO).State = EntityState.Modified;
                db.SaveChanges();
                logger.Debug(postServer + " (CMOController/ViewStartMenuKO): " + login.ToString());
                return RedirectToAction("ViewStartMenuOS", "CMO");
            }
            catch (Exception ex)
            {
                logger.Error(postServerError + " (CMOController/ViewStartMenuKO): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        
        public ActionResult EditUploadResult(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_UploadResult cMO_UploadResult = db.CMO_UploadResult.Find(id);
                logger.Debug(getServer + " (CMOController/EditUploadResult(int? id)): " + login.ToString());
                return View(cMO_UploadResult);
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + " (CMOController/EditUploadResult(int? id)): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        public ActionResult EditUploadResult(CMO_UploadResult cMO_UploadResult)
        {
            string login = HttpContext.User.Identity.Name;

            if (cMO_UploadResult.cost == 0)
                return RedirectToAction("EditUploadResult", "CMO", new { cMO_UploadResult.id });
            if (cMO_UploadResult.day == 0 || cMO_UploadResult.day == null)
                return RedirectToAction("EditUploadResult", "CMO", new { cMO_UploadResult.id });
            try
            {
                db.Entry(cMO_UploadResult).State = EntityState.Modified;
                db.SaveChanges();

                CMO_Tender cMO_Tender = db.CMO_Tender.Find(cMO_UploadResult.id_CMO_Tender);
                CMO_Order cMO_Order = db.CMO_Order.Find(cMO_Tender.id_CMO_Order);

                if (cMO_Order.datetimeFirstTenderFinish == null)
                {
                    logger.Debug(postServer + " (CMOController/EditUploadResult): " + login.ToString());
                    return RedirectToAction("FinishFirstTender", "CMO", new { cMO_Order.id });
                }
                else
                {
                    logger.Debug(postServer + " (CMOController/EditUploadResult): " + login.ToString());
                    return RedirectToAction("FinishSecondTender", "CMO", new { cMO_Order.id });
                }
            }
            catch (Exception ex)
            {
                logger.Error(postServerError + " (CMOController/EditUploadResult): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }



        }
        
        public ActionResult AppDefaultOrder(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_Order cmo_Order = db.CMO_Order.Find(id);
                logger.Debug(getServer + " (CMOController/AppDefaultOrder(int? id)): " + login.ToString());
                return View(cmo_Order);
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + " (CMOController/AppDefaultOrder(int? id)): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
        [HttpPost]
        public ActionResult AppDefaultOrder(CMO_Order cmo_Order)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                db.Entry(cmo_Order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewStartMenuOS", "CMO");
            }
            catch (Exception ex)
            {
                logger.Error(postServerError + " (CMOController/AppDefaultOrder): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }
    }
}