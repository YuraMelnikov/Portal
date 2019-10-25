using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System;

namespace Wiki.Areas.CMKO.Controllers
{
    public class CMKController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        public ActionResult Index()
        {
            ViewBag.period = new SelectList(db.CMKO_PeriodResult
                .Where(d => d.close == false)
                .OrderByDescending(x => x.period), "period", "period");
            ViewBag.autor = new SelectList(db.AspNetUsers
                .Where(d => d.LockoutEnabled == true)
                .Where(d => d.Devision == 3 || d.Devision == 15 || d.Devision == 16)
                .OrderBy(x => x.CiliricalName), "Id", "CiliricalName");
            return View();
        }

        [HttpPost]
        public JsonResult GetOptimizationList()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Optimization
                .AsNoTracking()
                .Include(d => d.AspNetUsers)
                .Include(d => d.CMKO_PeriodResult)
                .Include(d => d.AspNetUsers2)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = GetEditLinkOptimization(login, dataList.id),
                dateCreate = dataList.datetimeCreate.ToShortDateString() + " " + dataList.datetimeCreate.ToShortTimeString(),
                userCreate = dataList.AspNetUsers.CiliricalName,
                autor = dataList.AspNetUsers2.CiliricalName,
                textData = dataList.description,
                dataList.CMKO_PeriodResult.period
            });
            return Json(new { data });
        }

        string GetEditLinkOptimization(string login, int id)
        {
            if (login == "myi@katek.by" || login == "Kuchynski@katek.by" || login == "nrf@katek.by" || login == "fvs@katek.by")
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOptimization('" + id.ToString() + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "<td></td>";
        }

        public JsonResult AddOptimization(CMKO_Optimization optimization)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            optimization.id_AspNetUsersCreate = db.AspNetUsers.First(d => d.Email == login).Id;
            optimization.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            optimization.datetimeCreate = DateTime.Now;
            optimization.dateTimeUpdate = DateTime.Now;
            optimization.histiryEdit = "";
            db.CMKO_Optimization.Add(optimization);
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOptimization(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMKO_Optimization
                .AsNoTracking()
                .Where(d => d.id == id)
                .ToList();
            var data = query.Select(dataList => new
            {
                idOptimization = dataList.id,
                dateCreate = dataList.datetimeCreate.ToShortDateString() + " " + dataList.datetimeCreate.ToShortTimeString(),
                userCreate = dataList.AspNetUsers.CiliricalName,
                autor = dataList.AspNetUsers2.CiliricalName,
                textData = dataList.description,
                dataList.CMKO_PeriodResult.period
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateOptimization(CMKO_Optimization optimization)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Optimization updateOptimization = db.CMKO_Optimization.First(d => d.id == optimization.id);
            updateOptimization.id_AspNetUsersUpdate = db.AspNetUsers.First(d => d.Email == login).Id;
            updateOptimization.dateTimeUpdate = DateTime.Now;
            if (updateOptimization.description != optimization.description)
                updateOptimization.description = optimization.description;
            if (updateOptimization.id_AspNetUsersIdea != optimization.id_AspNetUsersIdea)
                updateOptimization.id_AspNetUsersIdea = optimization.id_AspNetUsersIdea;
            if (updateOptimization.id_CMKO_PeriodResult != optimization.id_CMKO_PeriodResult)
                updateOptimization.id_CMKO_PeriodResult = optimization.id_CMKO_PeriodResult;
            db.Entry(updateOptimization).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveOptimization(CMKO_Optimization optimization)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            CMKO_Optimization updateOptimization = db.CMKO_Optimization.First(d => d.id == optimization.id);
            db.Entry(updateOptimization).State = EntityState.Deleted;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}