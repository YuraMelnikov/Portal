using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Wiki.Models;

namespace Wiki.Controllers
{
    public class CMO3Controller : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        [Authorize(Roles = "Admin, OS")]
        public ActionResult ViewStartMenuOS()
        {
            string login = HttpContext.User.Identity.Name;
            ViewOSCMO viewOSCMO = new ViewOSCMO();
            try
            {
                viewOSCMO.DefaultOrderNotComplited = db.CMO_Order
                    .Where(d => d.datetimeFirstTenderFinish == null)
                    .Where(d => d.dateCloseOrder != null)
                    .Where(d => d.companyWin > 0)
                    .ToList();

                viewOSCMO.ActiveOrder = db.CMO_PreOrder
                    .Where(d => d.firstTenderStart == false)
                    .ToList();

                viewOSCMO.ActiveOrderClose = db.CMO_Report
                    .Where(d => d.FactFinishDate == null & d.PlanFinishWork1 != null)
                    .ToList();

                viewOSCMO.ActiveSecondUpload = db.CMO_UploadResult
                    .Where(d => d.CMO_Tender.close == false && d.CMO_Tender.id_CMO_TypeTask != 3)
                    .ToList();
            }
            catch
            {
            }
            return View(viewOSCMO);
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
                ViewBag.companyWin = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(x => x.name), "id", "name");
                return View();
            }
            catch
            {
                return RedirectToAction("Error", "CMO3");
            }
        }
        [HttpPost]
        public ActionResult StartFirstTender(int[] preOrder, CMO_Order cMO_Order)
        {
            if (preOrder == null)
                return RedirectToAction("ViewStartMenuOS", "CMO3");
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
                cMO_Create.CMO_CreateOrderForCMO(cMO_Order.companyWin.Value, cMO_Order.idTime, cMO_Order.dateCreate);
                return RedirectToAction("ViewStartMenuOS", "CMO3");
            }
            catch
            {
                return RedirectToAction("Error", "CMO3");
            }
        }

        public ActionResult Error()
        {
            return RedirectToAction("ViewStartMenuOS", "CMO3");
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
                return View(cMO_UploadResult);
            }
            catch
            {
                return RedirectToAction("Error", "CMO3");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDataCompany(CMO_UploadResult cMO_UploadResult)
        {
            string login = HttpContext.User.Identity.Name;
            if (cMO_UploadResult.cost == 0)
                return RedirectToAction("UploadDataCompany", "CMO3", new { cMO_UploadResult.id });
            if (cMO_UploadResult.dateComplited.Value.Year < 2000 || cMO_UploadResult.dateComplited == null)
                return RedirectToAction("UploadDataCompany", "CMO3", new { cMO_UploadResult.id });
            try
            {
                CMO_UploadResult upCMO_UploadResult = db.CMO_UploadResult.Find(cMO_UploadResult.id);
                upCMO_UploadResult.cost = cMO_UploadResult.cost;
                upCMO_UploadResult.dateTimeUpload = DateTime.Now;
                upCMO_UploadResult.dateComplited = cMO_UploadResult.dateComplited;
                upCMO_UploadResult.day = GetBusinessDays(DateTime.Now, cMO_UploadResult.dateComplited.Value);
                db.Entry(upCMO_UploadResult).State = EntityState.Modified;
                db.SaveChanges();
                CMO_Tender cMO_Tender = db.CMO_Tender.First(d => d.id == upCMO_UploadResult.id_CMO_Tender);
                cMO_Tender.close = true;
                db.Entry(cMO_Tender).State = EntityState.Modified;
                db.SaveChanges();
                CMO_Tender tenderWin = db.CMO_Tender.First(d => d.id_CMO_TypeTask == 3 && d.id_CMO_Order == cMO_Tender.id_CMO_Order);
                tenderWin.finishPlanClose = cMO_UploadResult.dateComplited.Value;
                tenderWin.close = true;
                db.Entry(tenderWin).State = EntityState.Modified;
                db.SaveChanges();
                CMO_UploadResult winResult = db.CMO_UploadResult.First(d => d.id_CMO_Tender == tenderWin.id);
                winResult.day = GetBusinessDays(DateTime.Now, cMO_UploadResult.dateComplited.Value);
                winResult.cost = cMO_UploadResult.cost;
                winResult.dateTimeUpload = DateTime.Now;
                db.Entry(winResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewStartMenuOS", "CMO3");
            }
            catch
            {
                return RedirectToAction("Error", "CMO3");
            }
        }

        int GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =
                1 + ((endD - startD).TotalDays * 5 -
                (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return (int)calcBusinessDays;
        }
    }
}
