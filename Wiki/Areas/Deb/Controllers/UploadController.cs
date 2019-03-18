﻿using Newtonsoft.Json;
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

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var query = db.Debit_WorkBit.Where(d => d.close == false).Where(d => d.id_TaskForPZ == 15).ToList();
            var data = query.Select(dataList => new
            {
                edit =  "<a href =" + '\u0022' + "http://localhost:57314/Deb/Upload/NewPlus/" + dataList.id + '\u0022' + " class=" + '\u0022' + "btn-xs btn-primary" + '\u0022' + "role =" + '\u0022' + "button" + '\u0022' + ">Внести</a>",
                dataList.PZ_PlanZakaz.PlanZakaz,
                dataList.PZ_PlanZakaz.Name,
                Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", ""),
                DateSupply = JsonConvert.SerializeObject(dataList.PZ_PlanZakaz.DateSupply, settings).Replace(@"""", "")
            });

            return Json(new { data });
        }

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
            Debit_CostUpdate dc = new Debit_CostUpdate();
            dc.id_PZ_PlanZakaz = debit_WorkBit.id_PlanZakaz;
            return View(dc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPlus(Debit_CostUpdate debit_CostUpdate)
        {
            if (ModelState.IsValid)
            {
                PZ_PlanZakaz pZ_PlanZakaz = db.PZ_PlanZakaz.Find(debit_CostUpdate.id_PZ_PlanZakaz);
                debit_CostUpdate.id_PZ_PlanZakaz = pZ_PlanZakaz.Id;
                debit_CostUpdate.dateCreate = DateTime.Now;
                if(debit_CostUpdate.dateGetMoney == null)
                    return RedirectToAction("NewPlus", "Upload", new { db.Debit_WorkBit.First(d => d.id_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz && d.id_TaskForPZ == 15).id, area = "Deb" });
                db.Debit_CostUpdate.Add(debit_CostUpdate);
                db.SaveChanges();
                PZ_TEO pZ_TEO = db.PZ_TEO.Where(d => d.Id_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz).First();
                double costCorrect = (double)pZ_TEO.NDS + (double)pZ_TEO.OtpuskChena;
                double costNow = 0;
                foreach (var data in db.Debit_CostUpdate.Where(d => d.id_PZ_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz))
                {
                    costNow += data.cost;
                }

                if (costCorrect - costNow == 0)
                {
                    Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_PlanZakaz == debit_CostUpdate.id_PZ_PlanZakaz & d.id_TaskForPZ == 15).First();
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Upload", new { area = "Deb" });
            }

            ViewBag.id_PZ_PlanZakaz = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", debit_CostUpdate.id_PZ_PlanZakaz);
            return View(debit_CostUpdate);
        }

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
    }
}