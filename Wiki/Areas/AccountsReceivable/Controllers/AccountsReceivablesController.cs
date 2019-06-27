using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wiki.Areas.AccountsReceivable.Controllers
{
    public class AccountsReceivablesController : Controller
    {
        public ActionResult Index()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                ViewBag.userTP = new SelectList(db.AspNetUsers.Where(d => d.Devision == 4).OrderBy(d => d.CiliricalName), "Id", "CiliricalName");
                ViewBag.currency = new SelectList(db.PZ_Currency.OrderBy(d => d.Name), "id", "Name");
                ViewBag.orders = new SelectList(db.PZ_PlanZakaz.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
                return View();
            }
        }

        public JsonResult TEOList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var query = db.PZ_TEO
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz)
                    .Include(d => d.PZ_Currency)
                    .OrderByDescending(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getTEO('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    dataList.Rate,
                    dataList.SSM,
                    dataList.SSR,
                    dataList.IzdKom,
                    dataList.IzdPPKredit,
                    dataList.PI,
                    dataList.NOP,
                    dataList.KI_S,
                    dataList.KI_prochee,
                    dataList.OtpuskChena,
                    Currency = dataList.PZ_Currency.Name,
                    dataList.NDS
                });
                return Json(new { data });
            }
        }

        public JsonResult TasksList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var query = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.TaskForPZ.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Where(d => d.close == false)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getTask('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    dataList.TaskForPZ.taskName,
                    client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    user = dataList.TaskForPZ.AspNetUsers.CiliricalName,
                    planDate = dataList.datePlan.ToShortDateString()
                });
                return Json(new { data });
            }
        }

        public JsonResult TasksCloseList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var query = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.TaskForPZ.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Where(d => d.close == true)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    dataList.TaskForPZ.taskName,
                    client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    user = dataList.TaskForPZ.AspNetUsers.CiliricalName,
                    planDate = dataList.datePlan.ToShortDateString()
                });
                return Json(new { data });
            }
        }

        public JsonResult MyTasksList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var query = db.Debit_WorkBit
                    .AsNoTracking()
                    .Include(d => d.TaskForPZ.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Where(d => d.close == false)
                    .OrderBy(d => d.PZ_PlanZakaz.PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getTask('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                    viewLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getTask('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list-alt" + '\u0022' + "></span></a></td>",
                    order = dataList.PZ_PlanZakaz.PlanZakaz,
                    dataList.TaskForPZ.taskName,
                    client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    user = dataList.TaskForPZ.AspNetUsers.CiliricalName,
                    planDate = dataList.datePlan.ToShortDateString()
                });
                return Json(new { data });
            }
        }

        public JsonResult ContractList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var query = db.PZ_Setup
                    .AsNoTracking()
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                    .Where(d => d.PZ_PlanZakaz.DateCreate.Year > 2017)
                    .Where(d => d.PZ_PlanZakaz.Client != 39)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    dataList.PZ_PlanZakaz.PlanZakaz,
                    Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                    Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    dataList.KolVoDneyNaPrijemku,
                    dataList.PunktDogovoraOSrokahPriemki,
                    dataList.UslovieOplatyText
                });
                return Json(new { data });
            }
        }

        public string RenderUserMenu()
        {
            string login = "Войти";
            try
            {
                if (HttpContext.User.Identity.Name != "")
                    login = HttpContext.User.Identity.Name;
            }
            catch
            {
                login = "Войти";
            }
            return login;
        }

        public JsonResult DebitList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .Include(d => d.PZ_PlanZakaz.AspNetUsers)
                    .Include(d => d.PZ_PlanZakaz.PZ_Client)
                    .Include(d => d.PZ_PlanZakaz.PZ_Setup)
                    .Where(d => d.id_TaskForPZ == 15 || d.id_TaskForPZ == 38)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    edit = "<a href =" + '\u0022' + "http://pserver/Deb/Upload/NewPlus/" + dataList.id + '\u0022' + " class=" + '\u0022' + "btn-xs btn-primary" + '\u0022' + "role =" + '\u0022' + "button" + '\u0022' + ">Внести</a>",
                    dataList.PZ_PlanZakaz.PlanZakaz,
                    Manager = dataList.PZ_PlanZakaz.AspNetUsers.CiliricalName,
                    Client = dataList.PZ_PlanZakaz.PZ_Client.NameSort,
                    dataList.PZ_PlanZakaz.PZ_Setup.First().KolVoDneyNaPrijemku,
                    dataList.PZ_PlanZakaz.PZ_Setup.First().PunktDogovoraOSrokahPriemki,
                    dataList.PZ_PlanZakaz.PZ_Setup.First().UslovieOplatyText,
                    status = GetStatusName(dataList)
                });
                return Json(new { data });
            }
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

        //public JsonResult GetDefault(int id)//GetDefault(int id) - returt taskNmae + id
        //{
        //    using (PortalKATEKEntities db = new PortalKATEKEntities())
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        db.Configuration.LazyLoadingEnabled = false;
        //        var query = db.ServiceRemarks
        //            .Include(d => d.ServiceRemarksPlanZakazs)
        //            .Include(d => d.ServiceRemarksTypes)
        //            .Include(d => d.ServiceRemarksCauses)
        //            .Include(d => d.AspNetUsers)
        //            .Where(d => d.id == id).ToList();
        //        var data = query.Select(dataList => new
        //        {
        //            numberReclamation = "Рекламация №: " + dataList.id,
        //            dataList.id,
        //            pZ_PlanZakaz = GetPlanZakazArray(dataList.ServiceRemarksPlanZakazs.ToList()),
        //            id_Reclamation_Type = GetTypesArray(dataList.ServiceRemarksTypes.ToList()),
        //            id_ServiceRemarksCause = GetCausesArray(dataList.ServiceRemarksCauses.ToList()),
        //            dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, settings).Replace(@"""", ""),
        //            userCreate = dataList.AspNetUsers.CiliricalName,
        //            datePutToService = JsonConvert.SerializeObject(dataList.datePutToService, shortSettingLeftRight).Replace(@"""", ""),
        //            dateClose = JsonConvert.SerializeObject(dataList.dateClose, shortSettingLeftRight).Replace(@"""", ""),
        //            dataList.folder,
        //            dataList.text,
        //            dataList.description,
        //            answerHistiryText = GetHistoryText(dataList.id)
        //        });
        //        return Json(data.First(), JsonRequestBehavior.AllowGet);
        //    }
        //}

        
        //UpdateDefault(int id, bool checkedDefault)

        //GetTEO(int id)
        //UpdateTEO(int PZ_TEO)

        //GetSetup
        //UpdateSetup

        //GetLetter
        //UpdateLetter

        //GetTN
        //UpdateTN

        //GetCostSh
        //UpdateCostSh
    }
}