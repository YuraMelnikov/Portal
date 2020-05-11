using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Areas.CMO.Models;
using Wiki.Models;

namespace Wiki.Areas.CMO.Controllers
{
    public class CMOAreaController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        PortalKATEKEntities db = new PortalKATEKEntities();
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings longUsSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd HH:mm" };
        readonly JsonSerializerSettings longSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy HH:mm" };

        public ActionResult Index()
        {
            string login = HttpContext.User.Identity.Name;
            int devisionUser = 0;
            try
            {
                devisionUser = db.AspNetUsers.First(d => d.Email == login).Devision.Value;
            }
            catch
            {

            }
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");

            ViewBag.id_PlanZakazStickerOrder = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now && d.StickersPreOrder.Count(s => s.id_PZ_PlanZakaz == d.Id) == 0).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_PlanZakazReStickersOrder = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now && d.StickersPreOrder.Count(s => s.id_PZ_PlanZakaz == d.Id) != 0).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_CMO_TypeProduct = new SelectList(db.CMO_TypeProduct.Where(d => d.active == true), "id", "name");
            if (devisionUser == 7)
                ViewBag.userGroupId = 1;
            else if (login == "nrf@katek.by")
                ViewBag.userGroupId = 2;
            else if (login == "vi@katek.by")
                ViewBag.userGroupId = 2;
            else if (login == "ovp@katek.by" || login == "bav@katek.by")
                ViewBag.userGroupId = 6;
            else if (devisionUser == 13)
                ViewBag.userGroupId = 3;
            else if (devisionUser == 18 || devisionUser == 15)
                ViewBag.userGroupId = 4;
            else if (devisionUser == 16)
            {
                ViewBag.userGroupId = 7;
            }
            else
                ViewBag.userGroupId = 5;
            ViewBag.id_CMO_Company = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_SandwichPanelCustomer = new SelectList(db.SandwichPanelCustomer.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            logger.Debug("CMO: " + login);
            return View();
        }

        [HttpPost]
        public JsonResult ReportTable()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMO2_Order
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position.Select(p => p.CMO_TypeProduct)))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                name = GetCompanyName(dataList.CMO_Company),
                day = GetDay(dataList.workDateTime, dataList.manufDate),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, shortSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.reOrder,
                dataList.finCost,
                dataList.id,
                folder = @"<a href =" + dataList.folder + "> Папка </a>",
                status = GetStatuName(dataList),
                removeLink = GetFeRemoveLink(dataList.id)
            });
            return Json(new { data });
        }

        private string GetFeRemoveLink(int id)
        {
            string login = HttpContext.User.Identity.Name;
            int devision = GetDevision(login);
            if (devision == 7)
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveFe('" + id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>";
            else
                return "";
        }

        public JsonResult RemoveFe(int id)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var orders = db.CMO2_Position.Where(a => a.id_CMO2 == id).ToList();
            foreach(var order in orders)
            {
                db.CMO2_Position.Remove(order);
                db.SaveChanges();
            }

            CMO2_Order cMO2_Order = db.CMO2_Order.Find(id);
            new EmailCMO(cMO2_Order, login, 6);
            db.CMO2_Order.Remove(cMO2_Order);
            db.SaveChanges();
            return Json(6, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ToWork()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMO2_Order
                .Where(d => d.workIn == false && d.reOrder == false)
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position.Select(p => p.CMO_TypeProduct)))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                name = GetCompanyName(dataList.CMO_Company),
                day = GetDay(dataList.workDateTime, dataList.manufDate),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, longSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.finCost,
                dataList.id,
                folder = @"<a href =" + dataList.folder + "> Папка </a>"
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
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position.Select(p => p.CMO_TypeProduct)))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                name = GetCompanyName(dataList.CMO_Company),
                day = GetDay(dataList.workDateTime, dataList.manufDate),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, longSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.finCost,
                dataList.id,
                folder = @"<a href =" + dataList.folder + "> Папка </a>"
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult ToClose()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMO2_Order
                .Where(d => d.workIn == true && d.reOrder == false && d.manufIn == true && d.finComplited == false)
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position.Select(p => p.CMO_TypeProduct)))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return get('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                name = GetCompanyName(dataList.CMO_Company),
                day = GetDay(dataList.workDateTime, dataList.manufDate),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, shortSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.finCost,
                dataList.id,
                folder = @"<a href =" + dataList.folder + "> Папка </a>"
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult ToReOrder()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.CMO2_Order
                .Where(d => d.manufIn == false && d.reOrder == true)
                .Include(d => d.CMO2_Position.Select(s => s.PZ_PlanZakaz).Select(s => s.CMO2_Position.Select(p => p.CMO_TypeProduct)))
                .Include(d => d.CMO_Company)
                .OrderByDescending(d => d.dateTimeCreate)
                .ToList();
            var data = query.Select(dataList => new
            {
                editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getRe('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                position = GetPositionName(dataList.CMO2_Position.ToList()),
                name = GetCompanyName(dataList.CMO_Company),
                day = GetDay(dataList.workDateTime, dataList.manufDate),
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, longSetting).Replace(@"""", ""),
                dataList.workCost,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                dataList.manufCost,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                dataList.finCost,
                dataList.id,
                folder = @"<a href =" + dataList.folder + "> Папка </a>"
            });
            return Json(new { data });
        }

        [HttpPost]
        public ActionResult AddOrder(int[] oid_PlanZakaz, int[] oid_CMO_TypeProduct, HttpPostedFileBase[] ofile1)
        {
            if (ofile1[0] != null && oid_PlanZakaz != null && oid_CMO_TypeProduct != null)
            {
                string login = HttpContext.User.Identity.Name;
                new CMOOrederValid().CreateOrder(oid_PlanZakaz, oid_CMO_TypeProduct, login, ofile1);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddReOrder(int[] roid_PlanZakaz, int roid_CMO_Company, HttpPostedFileBase[] rofile1)
        {
            if (rofile1[0] != null && roid_PlanZakaz != null && roid_CMO_Company != 0)
            {
                string login = HttpContext.User.Identity.Name;
                new CMOOrederValid().CreateReOrder(roid_PlanZakaz, roid_CMO_Company, login, rofile1);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddSandwichPanelOrder(int[] spid_PlanZakaz, HttpPostedFileBase[] spfile1)
        {
            if (spfile1[0] != null && spid_PlanZakaz != null)
            {
                string login = HttpContext.User.Identity.Name;
                new SandwichPanelCRUD().CreateOrder(spid_PlanZakaz, login, spfile1);
            }
            return RedirectToAction("Index");
        }

        public JsonResult Get(int id)
        {
            var query = db.CMO2_Order.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                id_AspNetUsers_Create = dataList.AspNetUsers.CiliricalName,
                dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortDefaultSetting).Replace(@"""", ""),
                dataList.workIn,
                workDateTime = JsonConvert.SerializeObject(dataList.workDateTime, shortDefaultSetting).Replace(@"""", ""),
                dataList.workComplitet,
                dataList.workCost,
                dataList.manufIn,
                manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortDefaultSetting).Replace(@"""", ""),
                dataList.manufComplited,
                dataList.manufCost,
                dataList.finIn,
                finDate = JsonConvert.SerializeObject(dataList.finDate, shortDefaultSetting).Replace(@"""", ""),
                dataList.finComplited,
                dataList.finCost,
                dataList.folder,
                dataList.reOrder,
                id_PlanZakaz = GetPlanZakazArray(dataList.CMO2_Position.ToList()),
                id_CMO_TypeProduct = GetTypesArray(dataList.CMO2_Position.ToList()),
                dataList.id_CMO_Company
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        string[] GetPlanZakazArray(List<CMO2_Position> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_PZ_PlanZakaz.ToString();
            }
            return pZ_PlanZakaz;
        }

        string GetSPPlanZakazArray(List<SandwichPanel_PZ> reclamation_PZs)
        {
            string pZ_PlanZakaz = "";
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz += reclamation_PZs[i].PZ_PlanZakaz.PlanZakaz.ToString() + "; ";
            }
            return pZ_PlanZakaz;
        }

        string[] GetTypesArray(List<CMO2_Position> reclamation_PZs)
        {
            string[] pZ_PlanZakaz = new string[reclamation_PZs.Count];
            for (int i = 0; i < reclamation_PZs.Count; i++)
            {
                pZ_PlanZakaz[i] = reclamation_PZs[i].id_CMO_TypeProduct.ToString();
            }
            return pZ_PlanZakaz;
        }

        public JsonResult Update(CMO2_Order cMO2_Order)
        {
            string login = HttpContext.User.Identity.Name;
            new CMOOrederValid().UpdateOrder(cMO2_Order, login);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateReOrder(int id)
        {
            CMO2_Order order = db.CMO2_Order.Find(id);
            order.manufIn = true;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        string GetPositionName(List<CMO2_Position> positionsList)
        {
            string positions = "";
            foreach (var data in positionsList.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz))
            {
                positions += data.PZ_PlanZakaz.PlanZakaz + " - " + data.CMO_TypeProduct.name + ";" + "</br>";
            }
            return positions;
        }

        string GetPZsName(List<SandwichPanel_PZ> positionsList)
        {
            string positions = "";
            foreach (var data in positionsList.OrderBy(d => d.PZ_PlanZakaz.PlanZakaz))
            {
                positions += data.PZ_PlanZakaz.PlanZakaz + ";" + "</br>";
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

        int GetDay(DateTime? from, DateTime? to)
        {
            if (from == null || to == null)
                return 0;
            else
                return new WorkingDays().GetWorkingDays((DateTime)from, (DateTime)to);
        }

        string GetCompanyName(CMO_Company company)
        {
            if (company != null)
                return company.name;
            else
                return "";
        }

        string GetStatuName(CMO2_Order oreder)
        {
            string name = "";
            if (oreder.reOrder == true)
            {
                name = "Дозаказ";
            }
            else if (oreder.workDateTime == null)
            {
                name = "Не отправлен";
            }
            else if (oreder.manufDate == null)
            {
                name = "Ожидание сроков";
            }
            else if (oreder.finDate == null)
            {
                name = "Производится";
            }
            else
            {
                name = "Оприходован";
            }
            return name;
        }

        [HttpPost]
        public JsonResult SandwichPanelReport()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.SandwichPanel
                .Include(d => d.SandwichPanel_PZ.Select(p => p.PZ_PlanZakaz))
                .Include(d => d.SandwichPanelCustomer)
                .ToList();
            var data = query.Select(dataList => new
            {
                order = dataList.id,
                pz = GetPZsName(dataList.SandwichPanel_PZ.ToList()),
                dateCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longUsSetting).Replace(@"""", ""),
                dateApprove = JsonConvert.SerializeObject(dataList.datetimeUploadNewVersion, longUsSetting).Replace(@"""", ""),
                dateToCustomer = JsonConvert.SerializeObject(dataList.datetimeToCustomer, longUsSetting).Replace(@"""", ""),
                datePlanComplited = JsonConvert.SerializeObject(dataList.datetimePlanComplited, longUsSetting).Replace(@"""", ""),
                dateComplited = JsonConvert.SerializeObject(dataList.datetimeComplited, longUsSetting).Replace(@"""", ""),
                state = GetState(dataList),
                customerName = GetCustomerName(dataList.SandwichPanelCustomer),
                folder = @"<a href =" + dataList.folder + "> Папка </a>",
                dataList.numberOrder
            });
            return Json(new { data });
        }

        public string GetState(SandwichPanel sandwichPanel)
        {
            string state;
            if (sandwichPanel.remove == true)
                state = "Отменен";
            else if (sandwichPanel.onApprove == true)
                state = "На проверке";
            else if (sandwichPanel.onCorrection == true)
                state = "На исправлении";
            else if (sandwichPanel.onCustomer == true)
                state = "Ожидание отправки";
            else if (sandwichPanel.onGetDateComplited == true)
                state = "Ожидание сроков";
            else if (sandwichPanel.onComplited == true)
                state = "Изготавливается";
            else
                state = "Оприходован";
            return state;
        }

        [HttpPost]
        public JsonResult OnApproveTable()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.SandwichPanel
                .Where(d => d.onApprove == true && d.remove != true)
                .Include(d => d.SandwichPanel_PZ.Select(p => p.PZ_PlanZakaz))
                .Include(d => d.SandwichPanelCustomer)
                .ToList();
            var data = query.Select(dataList => new
            {
                edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getSandwichPanel('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                order = dataList.id,
                pz = GetPZsName(dataList.SandwichPanel_PZ.ToList()),
                dateCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longUsSetting).Replace(@"""", ""),
                dateApprove = JsonConvert.SerializeObject(dataList.datetimeUploadNewVersion, longUsSetting).Replace(@"""", ""),
                dateToCustomer = JsonConvert.SerializeObject(dataList.datetimeToCustomer, longUsSetting).Replace(@"""", ""),
                datePlanComplited = JsonConvert.SerializeObject(dataList.datetimePlanComplited, longUsSetting).Replace(@"""", ""),
                dateComplited = JsonConvert.SerializeObject(dataList.datetimeComplited, longUsSetting).Replace(@"""", ""),
                state = GetState(dataList),
                customerName = GetCustomerName(dataList.SandwichPanelCustomer),
                folder = @"<a href =" + dataList.folder + "> Папка </a>",
                dataList.numberOrder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OnCorrectionTable()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.SandwichPanel
                .Where(d => d.onCorrection == true && d.remove != true)
                .Include(d => d.SandwichPanel_PZ.Select(p => p.PZ_PlanZakaz))
                .Include(d => d.SandwichPanelCustomer)
                .ToList();
            var data = query.Select(dataList => new
            {
                edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getSandwichPanel('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                order = dataList.id,
                pz = GetPZsName(dataList.SandwichPanel_PZ.ToList()),
                dateCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longUsSetting).Replace(@"""", ""),
                dateApprove = JsonConvert.SerializeObject(dataList.datetimeUploadNewVersion, longUsSetting).Replace(@"""", ""),
                dateToCustomer = JsonConvert.SerializeObject(dataList.datetimeToCustomer, longUsSetting).Replace(@"""", ""),
                datePlanComplited = JsonConvert.SerializeObject(dataList.datetimePlanComplited, longUsSetting).Replace(@"""", ""),
                dateComplited = JsonConvert.SerializeObject(dataList.datetimeComplited, longUsSetting).Replace(@"""", ""),
                state = GetState(dataList),
                customerName = GetCustomerName(dataList.SandwichPanelCustomer),
                folder = @"<a href =" + dataList.folder + "> Папка </a>",
                dataList.numberOrder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OnCustomerTable()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.SandwichPanel
                .Where(d => d.onCustomer == true && d.remove != true)
                .Include(d => d.SandwichPanel_PZ.Select(p => p.PZ_PlanZakaz))
                .Include(d => d.SandwichPanelCustomer)
                .ToList();
            var data = query.Select(dataList => new
            {
                edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getSandwichPanel('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                order = dataList.id,
                pz = GetPZsName(dataList.SandwichPanel_PZ.ToList()),
                dateCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longUsSetting).Replace(@"""", ""),
                dateApprove = JsonConvert.SerializeObject(dataList.datetimeUploadNewVersion, longUsSetting).Replace(@"""", ""),
                dateToCustomer = JsonConvert.SerializeObject(dataList.datetimeToCustomer, longUsSetting).Replace(@"""", ""),
                datePlanComplited = JsonConvert.SerializeObject(dataList.datetimePlanComplited, longUsSetting).Replace(@"""", ""),
                dateComplited = JsonConvert.SerializeObject(dataList.datetimeComplited, longUsSetting).Replace(@"""", ""),
                state = GetState(dataList),
                customerName = GetCustomerName(dataList.SandwichPanelCustomer),
                folder = @"<a href =" + dataList.folder + "> Папка </a>",
                dataList.numberOrder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OnGetDateComplited()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.SandwichPanel
                .Where(d => d.onGetDateComplited == true && d.remove != true)
                .Include(d => d.SandwichPanel_PZ.Select(p => p.PZ_PlanZakaz))
                .Include(d => d.SandwichPanelCustomer)
                .ToList();
            var data = query.Select(dataList => new
            {
                edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getSandwichPanel('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                order = dataList.id,
                pz = GetPZsName(dataList.SandwichPanel_PZ.ToList()),
                dateCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longUsSetting).Replace(@"""", ""),
                dateApprove = JsonConvert.SerializeObject(dataList.datetimeUploadNewVersion, longUsSetting).Replace(@"""", ""),
                dateToCustomer = JsonConvert.SerializeObject(dataList.datetimeToCustomer, longUsSetting).Replace(@"""", ""),
                datePlanComplited = JsonConvert.SerializeObject(dataList.datetimePlanComplited, longUsSetting).Replace(@"""", ""),
                dateComplited = JsonConvert.SerializeObject(dataList.datetimeComplited, longUsSetting).Replace(@"""", ""),
                state = GetState(dataList),
                customerName = GetCustomerName(dataList.SandwichPanelCustomer),
                folder = @"<a href =" + dataList.folder + "> Папка </a>",
                dataList.numberOrder
            });
            return Json(new { data });
        }

        [HttpPost]
        public JsonResult OnComplitedTable()
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.SandwichPanel
                .Where(d => d.onComplited == true && d.remove != true)
                .Include(d => d.SandwichPanel_PZ.Select(p => p.PZ_PlanZakaz))
                .Include(d => d.SandwichPanelCustomer)
                .ToList();
            var data = query.Select(dataList => new
            {
                edit = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return getSandwichPanel('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                order = dataList.id,
                pz = GetPZsName(dataList.SandwichPanel_PZ.ToList()),
                dateCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longUsSetting).Replace(@"""", ""),
                dateApprove = JsonConvert.SerializeObject(dataList.datetimeUploadNewVersion, longUsSetting).Replace(@"""", ""),
                dateToCustomer = JsonConvert.SerializeObject(dataList.datetimeToCustomer, longUsSetting).Replace(@"""", ""),
                datePlanComplited = JsonConvert.SerializeObject(dataList.datetimePlanComplited, longUsSetting).Replace(@"""", ""),
                dateComplited = JsonConvert.SerializeObject(dataList.datetimeComplited, longUsSetting).Replace(@"""", ""),
                state = GetState(dataList),
                customerName = GetCustomerName(dataList.SandwichPanelCustomer),
                folder = @"<a href =" + dataList.folder + "> Папка </a>",
                dataList.numberOrder
            });
            return Json(new { data });
        }

        public JsonResult GetSandwichPanel(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.SandwichPanel
                .Where(d => d.id == id)
                .Include(d => d.SandwichPanel_PZ.Select(s => s.PZ_PlanZakaz))
                .Include(d => d.SandwichPanelCustomer)
                .Include(d => d.AspNetUsers)
                .ToList();
            var data = query.Select(dataList => new
            {
                spid = dataList.id,
                spid_PlanZakaz2 = GetSPPlanZakazArray(dataList.SandwichPanel_PZ.ToList()),
                id_SandwichPanelCustomer = GetCustomerId(dataList.SandwichPanelCustomer),
                spdateTimeCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longSetting).Replace(@"""", ""),
                spid_AspNetUsers_Create = dataList.AspNetUsers.CiliricalName,
                dataList.onApprove,
                datetimeToCorrection = JsonConvert.SerializeObject(dataList.datetimeToCorrection, shortDefaultSetting).Replace(@"""", ""),
                dataList.onCorrection,
                datetimeUploadNewVersion = JsonConvert.SerializeObject(dataList.datetimeUploadNewVersion, shortDefaultSetting).Replace(@"""", ""),
                dataList.onCustomer,
                datetimeToCustomer = JsonConvert.SerializeObject(dataList.datetimeToCustomer, shortDefaultSetting).Replace(@"""", ""),
                dataList.onGetDateComplited,
                datetimePlanComplited = JsonConvert.SerializeObject(dataList.datetimePlanComplited, shortDefaultSetting).Replace(@"""", ""),
                dataList.onComplited,
                datetimeComplited = JsonConvert.SerializeObject(dataList.datetimeComplited, shortDefaultSetting).Replace(@"""", ""),
                dataList.numberOrder
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        int GetCustomerId(SandwichPanelCustomer name)
        {
            if (name == null)
                return 0;
            else
                return name.id;
        }

        string GetCustomerName(SandwichPanelCustomer name)
        {
            if (name == null)
                return "";
            else
                return name.name;
        }

        public JsonResult PostPanelToKO(int spid)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            SandwichPanel sandwichPanel = db.SandwichPanel.Find(spid);
            sandwichPanel.onApprove = false;
            sandwichPanel.onCorrection = true;
            sandwichPanel.datetimeToCorrection = DateTime.Now;
            db.Entry(sandwichPanel).State = EntityState.Modified;
            db.SaveChanges();
            try
            {
                new EmailSandwichPanel(sandwichPanel, login, 3);
                logger.Debug("CMOAreaController / PostPanelToKO: " + sandwichPanel.id);
            }
            catch (Exception ex)
            {
                logger.Error("CMOAreaController / PostPanelToKO: " + sandwichPanel.id + " | " + ex);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostPanelToWork(int spid)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            SandwichPanel sandwichPanel = db.SandwichPanel.Find(spid);
            sandwichPanel.onApprove = false;
            sandwichPanel.onCustomer = true;
            sandwichPanel.datetimeUploadNewVersion = DateTime.Now;
            db.Entry(sandwichPanel).State = EntityState.Modified;
            db.SaveChanges();
            try
            {
                new EmailSandwichPanel(sandwichPanel, login, 2);
                logger.Debug("CMOAreaController / PostPanelToWork: " + sandwichPanel.id);
            }
            catch (Exception ex)
            {
                logger.Error("CMOAreaController / PostPanelToWork: " + sandwichPanel.id + " | " + ex);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostPanelToManufacturing(int spid)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            SandwichPanel sandwichPanel = db.SandwichPanel.Find(spid);
            sandwichPanel.onApprove = false;
            sandwichPanel.onCorrection = false;
            sandwichPanel.remove = true;
            sandwichPanel.datetimeUploadNewVersion = DateTime.Now;
            db.Entry(sandwichPanel).State = EntityState.Modified;
            db.SaveChanges();
            try
            {
                logger.Debug("CMOAreaController / PostPanelToManufacturing: " + sandwichPanel.id);
            }
            catch (Exception ex)
            {
                logger.Error("CMOAreaController / PostPanelToManufacturing: " + sandwichPanel.id + " | " + ex);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostPanelToCustomer(int spid, int id_SandwichPanelCustomer)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            SandwichPanel sandwichPanel = db.SandwichPanel.Find(spid);
            sandwichPanel.onCustomer = false;
            sandwichPanel.id_SandwichPanelCustomer = id_SandwichPanelCustomer;
            sandwichPanel.onGetDateComplited = true;
            sandwichPanel.datetimeToCustomer = DateTime.Now;
            db.Entry(sandwichPanel).State = EntityState.Modified;
            db.SaveChanges();
            try
            {
                new EmailSandwichPanel(sandwichPanel, login, 4);
                logger.Debug("CMOAreaController / PostPanelToCustomer: " + sandwichPanel.id);
            }
            catch (Exception ex)
            {
                logger.Error("CMOAreaController / PostPanelToCustomer: " + sandwichPanel.id + " | " + ex);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostPanelToPlanComplited(int spid, int id_SandwichPanelCustomer, DateTime datetimePlanComplited, string numberOrder)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            SandwichPanel sandwichPanel = db.SandwichPanel.Find(spid);
            sandwichPanel.onGetDateComplited = false;
            sandwichPanel.onComplited = true;
            sandwichPanel.datetimePlanComplited = datetimePlanComplited;
            sandwichPanel.id_SandwichPanelCustomer = id_SandwichPanelCustomer;
            sandwichPanel.numberOrder = numberOrder;
            db.Entry(sandwichPanel).State = EntityState.Modified;
            db.SaveChanges();
            try
            {
                new EmailSandwichPanel(sandwichPanel, login, 5);
                logger.Debug("CMOAreaController / PostPanelToPlanComplited: " + sandwichPanel.id);
            }
            catch (Exception ex)
            {
                logger.Error("CMOAreaController / PostPanelToPlanComplited: " + sandwichPanel.id + " | " + ex);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostPanelToComplited(int spid, DateTime datetimeComplited, string numberOrder)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            SandwichPanel sandwichPanel = db.SandwichPanel.Find(spid);
            sandwichPanel.onComplited = false;
            sandwichPanel.numberOrder = numberOrder;
            sandwichPanel.datetimeComplited = datetimeComplited;
            db.Entry(sandwichPanel).State = EntityState.Modified;
            db.SaveChanges();
            try
            {
                new EmailSandwichPanel(sandwichPanel, login, 6);
                logger.Debug("CMOAreaController / PostPanelToComplited: " + sandwichPanel.id);
            }
            catch (Exception ex)
            {
                logger.Error("CMOAreaController / PostPanelToComplited: " + sandwichPanel.id + " | " + ex);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStickersPanel()
        {
            string login = HttpContext.User.Identity.Name;
            int devision = GetDevision(login);
            DateTime controlDate = DateTime.Now.AddDays(-90);
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.StickersPreOrder
                .Include(d => d.AspNetUsers)
                .Where(a => a.datetimeCreate > controlDate)
                .ToList();
            var data = query.Select(dataList => new
            {
                dataList.id,
                editLink = GetEditStickerLink(dataList, devision),
                removeLink = GetRemoveLink(dataList, devision),
                closeOrderLink = GetCloseOrderLink(dataList, devision),
                order = dataList.orderNumString,
                user = dataList.AspNetUsers.CiliricalName,
                dateCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longUsSetting).Replace(@"""", ""),
                deadline1 = JsonConvert.SerializeObject(dataList.deadline, shortSetting).Replace(@"""", ""),
                deadline2 = JsonConvert.SerializeObject(dataList.datePlanUpload, shortSetting).Replace(@"""", ""),
                dataList.description,
                state = GetStateStickersOrder(dataList)
            });
            return Json(new { data });
        }

        private string GetRemoveLink(StickersPreOrder order, int devision)
        {
            bool inWork = GetInWork(devision);
            if (order.datetimePush == null && inWork == true)
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveStickersOrder('" + order.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>";
            else
                return "";
        }

        private string GetEditStickerLink(StickersPreOrder order, int devision)
        {
            bool inWork = GetInWorkOS(devision);
            if (order.datetimePush == null && inWork == true)
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetStickersOrder('" + order.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>";
            else
                return "";
        }

        private string GetCloseOrderLink(StickersPreOrder order, int devision)
        {
            bool inWork = GetInWorkOS(devision);
            if (order.datetimeClose == null && inWork == true && order.datetimePush != null)
                return "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return CloseStickersOrder('" + order.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-ok" + '\u0022' + "></span></a></td>";
            else
                return "";
        }

        private bool GetInWork(int devision)
        {
            if (devision == 7 || devision == 15 || devision == 16)
                return true;
            else
                return false;
        }

        private bool GetInWorkOS(int devision)
        {
            if (devision == 7)
                return true;
            else
                return false;
        }

        private int GetDevision(string login)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                return db.AspNetUsers.First(a => a.Email == login).Devision.Value;
            }
            catch
            {
                return 0;
            }
        }

        private string GetStateStickersOrder(StickersPreOrder order)
        {
            if (order.datetimePush == null)
                return "Ожидает отправки";
            else if (order.datetimeClose == null)
                return "Ожидается поставка";
            else
                return "Поступили на склад";
        }

        public JsonResult RemoveStickersOrder(int id)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            StickersPreOrder order = db.StickersPreOrder.Find(id);
            try
            {
                db.StickersPreOrder.Remove(order);
                db.SaveChanges();
                new EmailStickers(order, login, 7);
                logger.Debug("CMOAreaController / RemoveStickersOrder: " + order.id + " | " + login);
            }
            catch (Exception ex)
            {
                logger.Error("CMOAreaController / RemoveStickersOrder: " + order.id + " | " + ex + " | " + login);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddStickersOrder(int? idStickersOrder, HttpPostedFileBase[] spfileStickers, 
            DateTime? dateStickersOrder, string descStickersOrder)
        {
            string login = HttpContext.User.Identity.Name;
            if (dateStickersOrder == null)
                dateStickersOrder = DateTime.Now.AddDays(7);
            StickersPreOrder order = new StickersPreOrder
            {
                datetimeCreate = DateTime.Now,
                deadline = dateStickersOrder.Value,
                description = descStickersOrder,
                id_PZ_PlanZakaz = idStickersOrder,
                id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                datetimePush = null,
                datetimeClose = null,
                datePlanUpload = dateStickersOrder.Value,
                orderNumString = db.PZ_PlanZakaz.Find(idStickersOrder).PlanZakaz.ToString()
            };
            db.StickersPreOrder.Add(order);
            db.SaveChanges();
            string directory = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Stickers\" + order.orderNumString + order.id + @"\";
            Directory.CreateDirectory(directory);
            foreach (var file in spfileStickers)
            {
                try
                {
                    string fileReplace = Path.GetFileName(file.FileName);
                    fileReplace = ToSafeFileName(fileReplace);
                    var fileName = string.Format("{0}\\{1}", directory, fileReplace);
                    file.SaveAs(fileName);
                }
                catch
                {

                }
            }
            new EmailStickers(order, login, 1);
            return RedirectToAction("Index");
        }

        private string ToSafeFileName(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }

        [HttpPost]
        public ActionResult AddReStickersOrder(int? idStickersReOrder, HttpPostedFileBase[] spfileReStickers,
                                                 DateTime? dateStickersReOrder, string descReOrder)
        {
            string login = HttpContext.User.Identity.Name;
            if (dateStickersReOrder == null)
                dateStickersReOrder = DateTime.Now.AddDays(7);
            StickersPreOrder order = new StickersPreOrder
            {
                datetimeCreate = DateTime.Now,
                deadline = dateStickersReOrder.Value,
                description = descReOrder,
                id_PZ_PlanZakaz = idStickersReOrder,
                id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                datetimePush = null,
                datetimeClose = null,
                datePlanUpload = dateStickersReOrder.Value,
                orderNumString = db.PZ_PlanZakaz.Find(idStickersReOrder).PlanZakaz.ToString() + GetDZName(idStickersReOrder.Value)
            };
            db.StickersPreOrder.Add(order);
            db.SaveChanges();
            string directory = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Stickers\" + order.orderNumString + order.id + @"\";
            Directory.CreateDirectory(directory);
            foreach (var file in spfileReStickers)
            {
                try
                {
                    string fileReplace = Path.GetFileName(file.FileName);
                    fileReplace = ToSafeFileName(fileReplace);
                    var fileName = string.Format("{0}\\{1}", directory, fileReplace);
                    file.SaveAs(fileName);
                }
                catch
                {

                }
            }
            new EmailStickers(order, login, 2);
            return RedirectToAction("Index");
        }

        private string GetDZName(int id_Order)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            return " - Д" + db.StickersPreOrder.Count(d => d.id_PZ_PlanZakaz == id_Order);
        }

        [HttpPost]
        public ActionResult AddSimpleStickersOrder(HttpPostedFileBase[] spfileSimpleStickers,
                                         DateTime? dateStickersSimpleOrder, string descStickersSimpleOrder)
        {
            string login = HttpContext.User.Identity.Name;
            if (dateStickersSimpleOrder == null)
                dateStickersSimpleOrder = DateTime.Now.AddDays(7);
            StickersPreOrder order = new StickersPreOrder
            {
                datetimeCreate = DateTime.Now,
                deadline = dateStickersSimpleOrder.Value,
                description = descStickersSimpleOrder,
                id_PZ_PlanZakaz = null,
                id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                datetimePush = null,
                datetimeClose = null,
                datePlanUpload = dateStickersSimpleOrder.Value,
                orderNumString = ""
            };
            db.StickersPreOrder.Add(order);
            db.SaveChanges();
            order.orderNumString = "О - " + order.id;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            string directory = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Stickers\" + order.orderNumString + @"\";
            Directory.CreateDirectory(directory);
            foreach (var file in spfileSimpleStickers)
            {
                try
                {
                    string fileReplace = Path.GetFileName(file.FileName);
                    fileReplace = ToSafeFileName(fileReplace);
                    var fileName = string.Format("{0}\\{1}", directory, fileReplace);
                    file.SaveAs(fileName);
                }
                catch
                {

                }
            }
            new EmailStickers(order, login, 3);
            return RedirectToAction("Index");
        }

        public JsonResult PushStickersOrders()
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var preorderList = db.StickersPreOrder.Where(a => a.datetimePush == null).OrderBy(a => a.id_PZ_PlanZakaz).ToList();
            if(preorderList.Count == 0)
                return Json(0, JsonRequestBehavior.AllowGet);
            try
            {
                foreach (var preorder in preorderList)
                {
                    new EmailStickers(preorder, login, 4);
                    preorder.datetimePush = DateTime.Now;
                    db.Entry(preorder).State = EntityState.Modified;
                    db.SaveChanges();
                }
                logger.Debug("CMOAreaController / PushStickersOrders: " + DateTime.Now.ToString() + " | " + login);
            }
            catch (Exception ex)
            {
                logger.Error("CMOAreaController / PushStickersOrders: " + DateTime.Now.ToString() + " | " + ex + " | " + login);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStickersOrder(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            var query = db.StickersPreOrder.Where(d => d.id == id).ToList();
            var data = query.Select(dataList => new
            {
                updateStickerId = dataList.id,
                updateStickerNum = dataList.orderNumString,
                updateStickerDeadline = dataList.deadline.ToShortDateString()
            });
            return Json(data.First(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditStickers(int updateStickerId, DateTime updateStickerNewDate)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            StickersPreOrder order = db.StickersPreOrder.Find(updateStickerId);
            order.datePlanUpload = updateStickerNewDate;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            new EmailStickers(order, login, 5);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CloseStickersOrder(int id)
        {
            string login = HttpContext.User.Identity.Name;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            StickersPreOrder order = db.StickersPreOrder.Find(id);
            order.datetimeClose = DateTime.Now;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            new EmailStickers(order, login, 6);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}