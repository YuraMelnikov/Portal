using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Areas.CMO.Models;
using Wiki.Models;

namespace Wiki.Areas.CMO.Controllers
{
    public class CMOAreaController : Controller
    {
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly JsonSerializerSettings longSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy HH:mm" };

        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            int devisionUser = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_CMO_TypeProduct = new SelectList(db.CMO_TypeProduct.Where(d => d.active == true), "id", "name");
            if (devisionUser == 7)
                ViewBag.userGroupId = 1;
            else if (login == "nrf@katek.by")
                ViewBag.userGroupId = 2;
            else if (devisionUser == 13)
                ViewBag.userGroupId = 3;
            else if (devisionUser == 18 || devisionUser == 15)
                ViewBag.userGroupId = 4;
            else
                ViewBag.userGroupId = 5;
            ViewBag.id_CMO_Company = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");

            return View();
        }

        [HttpPost]
        public JsonResult ReportTable()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMO2_Order
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                dataList.CMO_Company.name,
                day = new WorkingDays().GetWorkingDays(dataList.manufDate.Value, dataList.finDate.Value),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, shortSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.finCost,
                dataList.id,
                dataList.folder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult ToWork()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMO2_Order
                .Where(d => d.workIn == false && d.reOrder == false)
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                dataList.CMO_Company.name,
                day = new WorkingDays().GetWorkingDays(dataList.manufDate.Value, dataList.finDate.Value),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, shortSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.finCost,
                dataList.id,
                dataList.folder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult ToManuf()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMO2_Order
                .Where(d => d.workIn == true && d.reOrder == false && d.manufIn == false)
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                dataList.CMO_Company.name,
                day = new WorkingDays().GetWorkingDays(dataList.manufDate.Value, dataList.finDate.Value),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, shortSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.finCost,
                dataList.id,
                dataList.folder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult ToClose()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMO2_Order
                .Where(d => d.workIn == true && d.reOrder == false && d.manufIn == true)
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                dataList.CMO_Company.name,
                day = new WorkingDays().GetWorkingDays(dataList.manufDate.Value, dataList.finDate.Value),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, shortSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.finCost,
                dataList.id,
                dataList.folder
            });
            return Json(new { data });
        }

        public JsonResult AddOrder(int[] id_PlanZakaz, int[] id_CMO_TypeProduct, HttpPostedFileBase[] file1)
        {
            string login = HttpContext.User.Identity.Name;
            new CMOOrederValid().CreateOrder(id_PlanZakaz, id_CMO_TypeProduct, login, file1);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddReOrder(int[] id_PlanZakaz, int id_CMO_Company, HttpPostedFileBase[] file1)
        {
            string login = HttpContext.User.Identity.Name;
            new CMOOrederValid().CreateReOrder(id_PlanZakaz, id_CMO_Company, login, file1);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            var query = db.CMO2_Order.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                dataList.id_AspNetUsers_Create,
                dataList.dateTimeCreate,
                dataList.workIn,
                dataList.workDateTime,
                dataList.workComplitet,
                dataList.workCost,
                dataList.manufIn,
                dataList.manufDate,
                dataList.manufComplited,
                dataList.manufCost,
                dataList.finIn,
                dataList.finDate,
                dataList.finComplited,
                dataList.finCost,
                dataList.folder,
                dataList.reOrder,
                dataList.id_CMO_Company
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        //Update(id)
        string GetPositionName(List<CMO2_Position> positionsList)
        {
            string positions = "";
            foreach (var data in positionsList.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz))
            {
                positions += data.PZ_PlanZakaz.PlanZakaz + " - " + data.CMO_TypeProduct.name + ";" + "</br>";
            }
            return positions;
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
    }
}