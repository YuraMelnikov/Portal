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
            //VB - Currency to Index
            //VB - user TP
            //VB - orders null?

            return View();
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

        //TasksCloseList
        //TEOList
        //ContractList
        //DebitList

        //GetDefault(int id) - returt taskNmae + id
        //UpdateDefault(int id, bool checkedDefault)

        //GetTEO(int id)
        //UpdateTEO(int PZ_TEO)

        //GetSetup
        //UpdateSetup

        //GetLetter
        //UpdateLetter

        //GetTN
        //UpdateTN
    }
}