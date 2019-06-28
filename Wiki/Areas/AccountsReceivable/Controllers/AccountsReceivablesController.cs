using System;
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

        public JsonResult GetDefault(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .Where(d => d.id == id)
                    .Include(d => d.TaskForPZ)
                    .Include(d => d.PZ_PlanZakaz)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    defaultId = dataList.id,
                    defaultTaskName = dataList.TaskForPZ.taskName + " по заказу: " + dataList.PZ_PlanZakaz.PlanZakaz.ToString()
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateDefault(int id, bool checkedDefault)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(id);
                if(checkedDefault == true)
                {
                    debit_WorkBit.close = true;
                    debit_WorkBit.dateClose = DateTime.Now;
                    db.Entry(debit_WorkBit).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTEO(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_TEO.Where(d => d.id == id).ToList();
                var data = query.Select(dataList => new
                {
                    idTEO = dataList.id,
                    dataList.Currency,
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
                    dataList.KursValuti,
                    dataList.NDS
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTEO(PZ_TEO pZ_TEO)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Entry(pZ_TEO).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSetup(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.PZ_Setup.Where(d => d.id == id).ToList();
                var data = query.Select(dataList => new
                {
                    idSetup = dataList.id,
                    dataList.KolVoDneyNaPrijemku,
                    dataList.PunktDogovoraOSrokahPriemki,
                    dataList.UslovieOplatyText,
                    dataList.UslovieOplatyInt,
                    dataList.TimeNaRKD,
                    dataList.RassmotrenieRKD,
                    dataList.SrokZamechanieRKD,
                    dataList.userTP
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateSetup(PZ_Setup pZ_Setup)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Entry(pZ_Setup).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLetter(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_WorkBit
                    .Where(d => d.id == id)
                    .Include(d => d.TaskForPZ)
                    .ToList();
                var data = query.Select(dataList => new
                {
                    letterId = dataList.id
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateLetter(int id, HttpPostedFileBase[] ofile1)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Debit_WorkBit debit_WorkBit = db.Debit_WorkBit.Find(id);
                debit_WorkBit.close = true;
                debit_WorkBit.dateClose = DateTime.Now;
                db.Entry(debit_WorkBit).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTN(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_TN.Where(d => d.@int == id).ToList();
                var data = query.Select(dataList => new
                {
                    tnId = dataList.@int,
                    dataList.numberTN,
                    dataList.dateTN,
                    dataList.numberSF,
                    dataList.dateSF,
                    dataList.Summa
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTN(Debit_TN debit)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Entry(debit).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCostSh(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Debit_IstPost.Where(d => d.id == id).ToList();
                var data = query.Select(dataList => new
                {
                    costShId = dataList.id,
                    dataList.transportSum,
                    dataList.numberOrder,
                    dataList.ndsSum,
                    dataList.currency
                });
                return Json(data.First(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateCostSh(Debit_IstPost debit)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                db.Entry(debit).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}