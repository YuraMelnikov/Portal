using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Models;
using Wiki.Models.CMO;

namespace Wiki.Controllers
{
    public class CMO2Controller : Controller
    {
        private string getServer = "получены данные от сервера ";
        private string postServer = "отправлены данные от сервера ";
        private string getServerError = "ошибка получения данныех от сервера ";
        private string postServerError = "ошибка отправки данных от сервера ";
        
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private PortalKATEKEntities db = new PortalKATEKEntities();

        [Authorize(Roles = "Admin, KBMUser, KBM, KBMUser, Technologist, KBE, KBEUser, GR, OPTP, OTK, Manufacturing, Sklad, OP, OS")]
        public ActionResult Index()
        {
            int data = 0;
            string login = HttpContext.User.Identity.Name;
            data = Convert.ToInt32(db.AspNetUsers.Where(d => d.Email == login).First().Devision);
            if (data == 7)
                return RedirectToAction("ViewStartMenuOS", "CMO");
            else if (data == 15)
                return RedirectToAction("WorkDeskKO", "CMO2");
            else if (data == 13)
                return RedirectToAction("Report", "CMO2");
            else
                return RedirectToAction("ReportSmall", "CMO2");
        }

        [Authorize(Roles = "Admin, OS")]
        public ActionResult WorkDeskOS()
        {
            ElementsWorkDeskOS elements = new ElementsWorkDeskOS();
            return View(elements);
        }

        [Authorize(Roles = "Admin, KBM, KBMUser")]
        public ActionResult WorkDeskKO()
        {
            WorkDeskKO workDeskKO = new WorkDeskKO();
            return View(workDeskKO);
        }

        [Authorize(Roles = "Admin, KBMUser, KBM, KBMUser")]
        public ActionResult CreateOrder()
        {
            string login = HttpContext.User.Identity.Name;
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_PlanZakaz1 = new SelectList(db.CMO_TypeProduct.Where(d => d.active == true), "id", "name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(HttpPostedFileBase[] file1, int[] PlanZakaz, int[] CMO_TypeProduct)
        {
            string login = HttpContext.User.Identity.Name;
            CMO_Create cMO_Create = new CMO_Create(file1, PlanZakaz, CMO_TypeProduct, db.AspNetUsers.First(d => d.Email == login).Id);
            return RedirectToAction("WorkDeskKO", "CMO2");
        }

        [Authorize(Roles = "Admin, KBMUser, KBM, KBMUser")]
        public ActionResult CreateOrderDefault()
        {
            string login = HttpContext.User.Identity.Name;
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.CMO_Company1 = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrderDefault(HttpPostedFileBase[] file1, int[] PlanZakaz, int[] CMO_Company1)
        {
            string login = HttpContext.User.Identity.Name;
            int ind = CMO_Company1[0];
            CMO_Company cMO_Company = db.CMO_Company.Where(d => d.id == ind).First();
            CMO_Create cMO_Create = new CMO_Create(file1, PlanZakaz, db.AspNetUsers.First(d => d.Email == login).Id, cMO_Company);
            ViewBag.CMO_Company = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            return RedirectToAction("WorkDeskKO", "CMO2");
        }
        
        [Authorize(Roles = "Admin, Technologist, OS, KBM")]
        public ActionResult Report()
        {
            string login = HttpContext.User.Identity.Name;
            CMO_ReportList data = new CMO_ReportList();
            return View(data);
        }

        public ActionResult ReportSmall()
        {
            string login = HttpContext.User.Identity.Name;
            var data = db.CMO_Report.OrderByDescending(d => d.Id_Order).ToList();
            return View(data);
        }
        
        public ActionResult CloseOrder(int? id)
        {
            string login = HttpContext.User.Identity.Name;
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
            return View();
        }
        [HttpPost]
        public ActionResult CloseOrder(CMO_Order cMO_Order)
        {
            string login = HttpContext.User.Identity.Name;
            CMO_Order cMO = db.CMO_Order.Find(cMO_Order.id);
            cMO.dateCloseOrder = cMO_Order.dateCloseOrder;
            db.Entry(cMO).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewStartMenuOS", "CMO2");
        }

        public ActionResult UploadTenderProtocol()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadTenderProtocol(HttpPostedFileBase fileUpload)
        {
            if (fileUpload == null)
                return RedirectToAction("UploadTenderProtocol");
            ExcelParserCMO excel = new ExcelParserCMO(fileUpload);
            excel.GetData();
            return RedirectToAction("Index");
        }
        
        public ActionResult StartFirstTender(int? id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                CMO_Order cMO_Order = db.CMO_Order.Find(id);
                ViewBag.Order = cMO_Order.id;
                ViewBag.DateTimeCreate = cMO_Order.dateCreate;
                ViewBag.UserCreate = db.AspNetUsers.Find(cMO_Order.userCreate).CiliricalName.ToString();
                ViewBag.idTime = new SelectList(db.CMO_HoureTender.Where(d => d.active == true).OrderBy(d => d.count).ToList(), "id", "name", cMO_Order.idTime);
                logger.Debug(getServer + " (CMOController/StartFirstTender): " + login.ToString());
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(getServerError + " (CMOController/StartFirstTender): " + ex.Message.ToString());
                return RedirectToAction("Error", "CMO");
            }
        }

        [HttpPost]
        public ActionResult StartFirstTender(CMO_Order cMO_Order)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                ViewBag.idTime = new SelectList(db.CMO_HoureTender.Where(d => d.active == true).OrderBy(d => d.count).ToList(), "id", "name", cMO_Order.idTime);
                CMO_Create cMO_Create = new CMO_Create(cMO_Order.id);
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
                //ViewBag.idTime = new SelectList(db.CMO_HoureTender.Where(d => d.active == true).OrderBy(d => d.count).ToList(), "id", "name", cMO_Order.idTime);
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
                //ViewBag.idTime = new SelectList(db.CMO_HoureTender.Where(d => d.active == true).OrderBy(d => d.count).ToList(), "id", "name", cMO_Order.idTime);
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
    }
}