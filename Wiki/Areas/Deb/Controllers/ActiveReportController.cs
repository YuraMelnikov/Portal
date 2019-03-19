﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Wiki.Areas.Deb.Controllers
{
    public class ActiveReportController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings settings = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly string firstPartLinkEditOP = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyID('";
        readonly string firstPartLinkEditKO = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getbyKOID('";
        readonly string lastPartEdit = "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";

        [Authorize(Roles = "Admin, OPTP, OP, Fin director")]
        public ActionResult Index()
        {
            Debit_PeriodReportOprih debit_PeriodReportOprih = db.Debit_PeriodReportOprih.Find(db.Debit_PeriodReportOprih.Max(d => d.id));
            int closePeriod = 1;
            string login = HttpContext.User.Identity.Name;
            if (login == "mvv@katek.by" || login == "myi@katek.by")
            {
                closePeriod = 0;
                if (debit_PeriodReportOprih.close == true)
                    closePeriod = 1;
            }
            ViewBag.ClosePeriod = closePeriod;
            return View();
        }

        public JsonResult List()
        {
            Debit_PeriodReportOprih debit_PeriodReportOprih = db.Debit_PeriodReportOprih.Find(db.Debit_PeriodReportOprih.Max(d => d.id));
            var query = db.Debit_DataReportOprih.Where(d => d.id_Debit_PeriodReportOprih == debit_PeriodReportOprih.id).ToList();
            string login = HttpContext.User.Identity.Name;
            int devision = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            string linkPartOne = "";
            string linkPartTwo = "";
            if (devision == 5 || login == "myi@katek.by" && debit_PeriodReportOprih.close != true)
            {
                linkPartOne = firstPartLinkEditOP;
                linkPartTwo = lastPartEdit;
            }

            var data = query.Select(dataList => new
            {
                id = linkPartOne + dataList.id + linkPartTwo,
                dataList.Debit_WorkBit.PZ_PlanZakaz.PlanZakaz,
                dataList.Debit_WorkBit.PZ_PlanZakaz.Name,
                Manager = dataList.Debit_WorkBit.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                Client = dataList.Debit_WorkBit.PZ_PlanZakaz.PZ_Client.NameSort,
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.Debit_WorkBit.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", ""),
                DateSupply = JsonConvert.SerializeObject(dataList.Debit_WorkBit.PZ_PlanZakaz.DateSupply, settings).Replace(@"""", ""),

                dataList.numberSF,
                dataList.reclamation,
                openReclamation = JsonConvert.SerializeObject(dataList.reclamationOpen, settings).Replace(@"""", ""),
                closeReclamation = JsonConvert.SerializeObject(dataList.reclamationClose, settings).Replace(@"""", ""),

                dataList.description,
                dataList.oprihClose,
                dateOprihPlanFact = JsonConvert.SerializeObject(dataList.dateOprihPlanFact, settings).Replace(@"""", "")
            });

            return Json(new { data });
        }

        [Authorize(Roles = "Admin, OPTP, OP")]
        public JsonResult GetId(int id)
        {
            var query = db.Debit_DataReportOprih.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.description,
                dataList.oprihClose,
                dateOprihPlanFact = JsonConvert.SerializeObject(dataList.dateOprihPlanFact, settings).Replace(@"""", ""),

                dataList.Debit_WorkBit.PZ_PlanZakaz.PlanZakaz,
                dataList.Debit_WorkBit.PZ_PlanZakaz.Name,
                Manager = dataList.Debit_WorkBit.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                Client = dataList.Debit_WorkBit.PZ_PlanZakaz.PZ_Client.NameSort,
                dataOtgruzkiBP = JsonConvert.SerializeObject(dataList.Debit_WorkBit.PZ_PlanZakaz.dataOtgruzkiBP, settings).Replace(@"""", ""),
                DateSupply = JsonConvert.SerializeObject(dataList.Debit_WorkBit.PZ_PlanZakaz.DateSupply, settings).Replace(@"""", ""),

                dataList.numberSF,
                dataList.reclamation,
                openReclamation = JsonConvert.SerializeObject(dataList.reclamationOpen, settings).Replace(@"""", ""),
                closeReclamation = JsonConvert.SerializeObject(dataList.reclamationClose, settings).Replace(@"""", "")
            });

            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, OPTP, OP")]
        public JsonResult Update(Debit_DataReportOprih work, bool oprihClose)
        {
            Debit_DataReportOprih debit_DataReportOprih = db.Debit_DataReportOprih.Find(work.id);
            debit_DataReportOprih.dateOprihPlanFact = work.dateOprihPlanFact;
            if (work.description != null)
                debit_DataReportOprih.description = work.description;
            debit_DataReportOprih.oprihClose = oprihClose;
            db.Entry(debit_DataReportOprih).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ClosePeriod()
        {
            Debit_PeriodReportOprih debit_PeriodReportOprih = db.Debit_PeriodReportOprih.Find(db.Debit_PeriodReportOprih.Max(d => d.id));
            debit_PeriodReportOprih.close = true;
            debit_PeriodReportOprih.dateTimeClose = DateTime.Now;
            db.Entry(debit_PeriodReportOprih).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var listDate = db.Debit_DataReportOprih.Where(d => d.id_Debit_PeriodReportOprih == debit_PeriodReportOprih.id).ToList();
            foreach(var data in listDate)
            {
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.First(d => d.id == data.id_DebitWork);
                debit_WorkBit.close = data.oprihClose;
                debit_WorkBit.dateClose = data.dateOprihPlanFact;
                db.Entry(debit_WorkBit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                if (debit_WorkBit.close == true)
                    CreateNewTasks(28, debit_WorkBit.id_PlanZakaz);
                ClosePredoplataTask(debit_WorkBit.id_PlanZakaz);
            }
            return RedirectToAction("Index", "ActiveReport", new { area = "Deb" });
        }

        void ClosePredoplataTask(int planZakaz)
        {
            try
            {
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Where(d => d.id_TaskForPZ == 38).Where(d => d.id_PlanZakaz == planZakaz).First();
                debit_WorkBit.close = true;
                db.Entry(debit_WorkBit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {

            }
        }
        
        void CreateNewTasks(int predecessors, int idOrder)
        {
            List<TaskForPZ> dateTaskWork = db.TaskForPZ.Where(w => w.Predecessors == predecessors).Where(z => z.id_TypeTaskForPZ == 1).ToList();
            foreach (var data in dateTaskWork)
            {
                Debit_WorkBit newDebit_WorkBit = new Debit_WorkBit();
                newDebit_WorkBit.dateCreate = DateTime.Now;
                newDebit_WorkBit.close = false;
                newDebit_WorkBit.id_PlanZakaz = idOrder;
                newDebit_WorkBit.id_TaskForPZ = data.id;
                newDebit_WorkBit.datePlanFirst = DateTime.Now.AddDays(data.time);
                newDebit_WorkBit.datePlan = DateTime.Now.AddDays(data.time);
                db.Debit_WorkBit.Add(new Debit_WorkBit()
                {
                    close = false,
                    dateClose = null,
                    dateCreate = DateTime.Now,
                    datePlan = newDebit_WorkBit.datePlan,
                    datePlanFirst = newDebit_WorkBit.datePlanFirst,
                    id_PlanZakaz = newDebit_WorkBit.id_PlanZakaz,
                    id_TaskForPZ = newDebit_WorkBit.id_TaskForPZ
                });
                db.SaveChanges();
            }
        }
    }
}