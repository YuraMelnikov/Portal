using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Wiki.Areas.Deb.Controllers
{
    public class UploadController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        [Authorize(Roles = "Admin, OPTP, OP, Fin director")]
        public ActionResult Index()
        {
            ViewBag.PZ = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            return View();
        }

        public JsonResult List()
        {
            var query = db.Debit_WorkBit
                .Where(d => d.id_TaskForPZ == 15 || d.id_TaskForPZ == 38).ToList();
            var data = query.Select(dataList => new
            {
                status = GetStatusName(dataList),
                edit =  "<a href =" + '\u0022' + "http://pserver/Deb/Upload/NewPlus/" + dataList.id + '\u0022' + " class=" + '\u0022' + "btn-xs btn-primary" + '\u0022' + "role =" + '\u0022' + "button" + '\u0022' + ">Внести</a>",
                dataList.PZ_PlanZakaz.PlanZakaz,
                dataList.PZ_PlanZakaz.Name,
                Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", ""),
                DateSupply = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.DateSupply, settings).Replace(@"""", "")
            });

            return Json(new { data });
        }

        string GetStatusName(Debit_WorkBit debit_WorkBit)
        {
            string statusName = "Не оплачен";
            if (debit_WorkBit.close == true)
                statusName = "Оплачен";
            if (debit_WorkBit.id_TaskForPZ == 38)
                statusName = "Внести предоплату";

            return statusName;
        }

        [Authorize(Roles = "Admin, Fin director")]
        public ActionResult NewPlus(int id)
        {
            double getCost = 0;
            Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(id);
            ViewBag.PlanZakaz = db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz).PlanZakaz.ToString();
            ViewBag.idPlanZakaz = db.PZ_PlanZakaz.Find(debit_WorkBit.id_PlanZakaz).Id;
            System.Globalization.NumberFormatInfo numForInf = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            ViewBag.Cost = db.PZ_TEO.Where(d => d.Id_PlanZakaz == debit_WorkBit.id_PlanZakaz).First().OtpuskChena.ToString("N", numForInf);
            ViewBag.Curency = db.PZ_TEO.Where(d => d.Id_PlanZakaz == debit_WorkBit.id_PlanZakaz).First().PZ_Currency.Name.ToString();
            var listCost = db.Debit_CostUpdate.Where(d => d.id_PZ_PlanZakaz == debit_WorkBit.id_PlanZakaz).ToList();
            foreach (var data in listCost)
            {
                getCost += data.cost;
            }
            ViewBag.GetCost = getCost.ToString("N", numForInf);
            PZ_TEO pZ_TEO = db.PZ_TEO.Where(d => d.Id_PlanZakaz == debit_WorkBit.id_PlanZakaz).First();
            double nds = 0;
            double ndsCost = pZ_TEO.OtpuskChena;
            if (pZ_TEO.NDS != null)
            {
                ndsCost += (double)pZ_TEO.NDS;
                nds += (double)pZ_TEO.NDS;
            }
            ViewBag.NDS = nds.ToString("N", numForInf);
            ViewBag.CostNDS = ndsCost.ToString("N", numForInf);
            ViewBag.PostCost = (ndsCost - getCost).ToString("N", numForInf);
            ViewBag.listCost = listCost.ToList();
            PZ_Setup pZ_Setup = db.PZ_Setup.First(d => d.id_PZ_PlanZakaz == debit_WorkBit.id_PlanZakaz);
            ViewBag.UslovieOplat = pZ_Setup.UslovieOplatyText;
            ViewBag.UslovieOplat = pZ_Setup.PunktDogovoraOSrokahPriemki;

            Debit_CostUpdate dc = new Debit_CostUpdate();
            dc.id_PZ_PlanZakaz = debit_WorkBit.id_PlanZakaz;
            return View(dc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPlus(Debit_CostUpdate debit_CostUpdate)
        {
            Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz & d.id_TaskForPZ == 15).First();
            if (debit_CostUpdate.dateGetMoney.Year < 2010)
                return RedirectToAction("NewPlus", "Upload", new { debit_WorkBit.id, area = "Deb" });
            if (debit_CostUpdate.cost == 0)
                return RedirectToAction("NewPlus", "Upload", new { debit_WorkBit.id, area = "Deb" });
            PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(debit_CostUpdate.id_PZ_PlanZakaz);
            debit_CostUpdate.id_PZ_PlanZakaz = pZ_PlanZakaz.Id;
            debit_CostUpdate.dateCreate = DateTime.Now;
            db.Debit_CostUpdate.Add(debit_CostUpdate);
            db.SaveChanges();
            PZ_TEO pZ_TEO = db.PZ_TEO.Where(d => d.Id_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz).First();
            double nds = 0;
            try
            {
                nds = (double)pZ_TEO.NDS;
            }
            catch
            {

            }
            double costCorrect = nds + (double)pZ_TEO.OtpuskChena;
            double costNow = 0;
            foreach (var data in db.Debit_CostUpdate.Where(d => d.id_PZ_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz))
            {
                costNow += data.cost;
            }
            if (costCorrect - costNow == 0)
            {
                debit_WorkBit.close = true;
                debit_WorkBit.dateClose = DateTime.Now;
                db.Entry(debit_WorkBit).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Upload", new { area = "Deb" });
        }

        [Authorize(Roles = "Admin, Fin director")]
        public ActionResult EditPartial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debit_CostUpdate debit_CostUpdate = db.Debit_CostUpdate.Find(id);
            if (debit_CostUpdate == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return PartialView(debit_CostUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartial(Debit_CostUpdate debit_CostUpdate)
        {
            if (ModelState.IsValid)
            {
                if (debit_CostUpdate.dateGetMoney == null)
                    return RedirectToAction("NewPlus", "Upload", new { db.Debit_WorkBit.First(d => d.id_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz && d.id_TaskForPZ == 15).id, area = "Deb" });
                db.Entry(debit_CostUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NewPlus", "Upload", new { db.Debit_WorkBit.First(d => d.id_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz && d.id_TaskForPZ == 15).id, area = "Deb" });
            }
            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return View(debit_CostUpdate);
        }
        
        public JsonResult СreateTask38С(int[] PZ)
        {
            foreach (int data in PZ)
            {
                int countPZ = db.Debit_WorkBit.Where(d => d.id_TaskForPZ == 28 || d.id_TaskForPZ == 38).Where(d => d.id_PlanZakaz == data).Count();
                if (countPZ == 0)
                {
                    Debit_WorkBit debit_WorkBit = new Debit_WorkBit();
                    debit_WorkBit.id_PlanZakaz = data;
                    debit_WorkBit.close = false;
                    debit_WorkBit.dateCreate = DateTime.Now;
                    debit_WorkBit.datePlan = DateTime.Now;
                    debit_WorkBit.datePlanFirst = DateTime.Now;
                    debit_WorkBit.id_TaskForPZ = 38;
                    db.Debit_WorkBit.Add(debit_WorkBit);
                    db.SaveChanges();
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int Id)
        {
            var query = db.Debit_CostUpdate.Where(d => d.id == Id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.dateCreate,
                dateGetMoney = JsonConvert.SerializeObject(dataList.dateGetMoney, settings).Replace(@"""", ""),
                dataList.id_PZ_PlanZakaz,
                dataList.cost
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(Debit_CostUpdate debit_CostUpdate, float mcost, DateTime mdateGetMoney)
        {
            Debit_CostUpdate newCost = db.Debit_CostUpdate.Find(debit_CostUpdate.id);
            Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == newCost.id_PZ_PlanZakaz & d.id_TaskForPZ == 15).First();
            if (mdateGetMoney.Year < 2010)
                return Json(1, JsonRequestBehavior.AllowGet);
            if (mcost == 0)
                return Json(1, JsonRequestBehavior.AllowGet);

            newCost.cost = mcost;
            newCost.dateGetMoney = mdateGetMoney;
            db.Entry(newCost).State = EntityState.Modified;
            db.SaveChanges();
            PZ_TEO pZ_TEO = db.PZ_TEO.Where(d => d.Id_PlanZakaz == newCost.id_PZ_PlanZakaz).First();
            double nds = 0;
            try
            {
                nds = (double)pZ_TEO.NDS;
            }
            catch
            {

            }
            double costCorrect = nds + (double)pZ_TEO.OtpuskChena;
            double costNow = 0;
            foreach (var data in db.Debit_CostUpdate.Where(d => d.id_PZ_PlanZakaz == newCost.id_PZ_PlanZakaz))
            {
                costNow += data.cost;
            }
            if (costCorrect - costNow == 0)
            {
                debit_WorkBit.close = true;
                debit_WorkBit.dateClose = DateTime.Now;
                db.Entry(debit_WorkBit).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}