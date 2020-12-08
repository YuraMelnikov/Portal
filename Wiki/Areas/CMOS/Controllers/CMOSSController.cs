using Newtonsoft.Json;
using NLog;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using Wiki.Areas.CMOS.Models;
using Wiki.Areas.CMOS.Struct;

namespace Wiki.Areas.CMOS.Controllers
{
    public class CMOSSController : Controller
    {
        private readonly double rate = 3.55;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings longUsSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd HH:mm" };
        readonly JsonSerializerSettings longSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy HH:mm" };

        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            string login = HttpContext.User.Identity.Name;
            int devisionUser = GetDevision(login);
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_CMO_TypeProduct = new SelectList(db.CMO_TypeProduct.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            var preordersList = db.CMOSPreOrder
                .Include(d => d.PZ_PlanZakaz)
                .Include(d => d.CMO_TypeProduct)
                .Where(d => d.CMOSOrderPreOrder.Count == 0 && d.remove == false)
                .ToList();
            List<object> newList = new List<object>();
            foreach (var member in preordersList)
                newList.Add(new
                {
                    Id = member.id,
                    Name = member.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + member.CMO_TypeProduct.name
                });
            ViewBag.id_CMOSPreorder = new SelectList(newList, "Id", "Name");
            if (devisionUser == 7)
                ViewBag.userGroupId = 1;
            else if (login == "myi@katek.by" || login == "koag@katek.by" || login == "naa@katek.by")
                ViewBag.userGroupId = 5;
            else if (login == "nrf@katek.by" || login == "vi@katek.by" || login == "kaav@katek.by" || login == "goa@katek.by" || login == "cherskov@katek.by" || login == "cyv@katek.by")
                ViewBag.userGroupId = 2;
            else if (login == "bav@katek.by" || login == "laa@katek.by")
                ViewBag.userGroupId = 4;
            else
                ViewBag.userGroupId = 3;
            ViewBag.id_CMO_Company = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.correctingListArmis = new SelectList(db.CMOSOrder
                .Where(d => d.remove == false && d.cMO_CompanyId == 1)
                .OrderByDescending(d => d.id), "id", "id");
            ViewBag.correctingListGratius = new SelectList(db.CMOSOrder
                .Where(d => d.remove == false && d.finDate == null && d.cMO_CompanyId == 2)
                .OrderBy(d => d.id), "id", "id");
            ViewBag.correctingListEcowood = new SelectList(db.CMOSOrder
                .Where(d => d.remove == false && d.finDate == null && d.cMO_CompanyId == 3)
                .OrderBy(d => d.id), "id", "id");
            return View();
        }

        public JsonResult GetTableOrders()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    DateTime controlDate = DateTime.Now.AddDays(-85);
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSOrder
                        .AsNoTracking()
                        .Include(a => a.CMO_Company)
                        .Include(a => a.CMOSPositionOrder)
                        .Where(a => a.dateTimeCreate > controlDate && a.remove == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.id,
                        percentComplited = GetPercentComplited(dataList.id),
                        positions = GetPositionsNamePreOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        dateGetMail = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                        state = GetStateOrder(dataList.id),
                        startDate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        finishDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                        dataList.factCost,
                        dataList.rate,
                        folder = @"<a href =" + dataList.folder + "> Папка </a>",
                        tnNumber = dataList.numberTN,
                        posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        curency = GetCurrencyForReport(dataList.finDate),
                        summaryWeight = Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2),
                        cost = Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2) * dataList.rate * GetCurrencyForReport(dataList.finDate),
                        deviation = dataList.factCost - Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2) * dataList.rate * GetCurrencyForReport(dataList.finDate)
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetTableOrders: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private double GetCurrencyForReport(DateTime? date)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                if (date == null)
                {
                    date = db.CurencyBYN.Max(a => a.date);
                    return db.CurencyBYN.First(a => a.date == date).USD;
                }
                else
                {
                    date = date.Value.AddDays(1);
                    try
                    {
                        db.Configuration.ProxyCreationEnabled = false;
                        db.Configuration.LazyLoadingEnabled = false;
                        try
                        {
                            return db.CurencyBYN.First(a => a.date == date).USD;
                        }
                        catch
                        {
                            date = db.CurencyBYN.Max(a => a.date);
                            return db.CurencyBYN.First(a => a.date == date).USD;
                        }
                    }
                    catch
                    {
                        return 0.0;
                    }
                }
            }
        }

        private double GetDeviation(double cost, double deviationCost)
        {
            if (deviationCost == 0.0)
                return deviationCost;
            else
                return deviationCost - cost;
        }

        public JsonResult GetTableNoPlaningPreOrder()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSPreOrder
                        .AsNoTracking()
                        .Include(a => a.CMO_TypeProduct)
                        .Include(a => a.PZ_PlanZakaz)
                        .Include(a => a.AspNetUsers)
                        .Include(a => a.CMOSPositionPreOrder)
                        .Where(a => a.CMOSOrderPreOrder.Count == 0 && a.remove != true)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.id,
                        positionName = dataList.CMO_TypeProduct.name,
                        summaryWeight = Math.Round(dataList.CMOSPositionPreOrder.Sum(a => a.summaryWeight), 2),
                        order = dataList.PZ_PlanZakaz.PlanZakaz,
                        dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        userCreate = dataList.AspNetUsers.CiliricalName,
                        positionsList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsPreorder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemovePreOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        folder = @"<a href =" + dataList.folder + "> Папка </a>"
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetTableNoPlaningPreOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTableNoPlaningOrder()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSOrder
                        .AsNoTracking()
                        .Include(a => a.AspNetUsers)
                        .Include(a => a.CMO_Company)
                        .Include(a => a.CMOSPositionOrder)
                        .Where(a => a.manufDate == null && a.remove == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.id,
                        userCreate = dataList.AspNetUsers.CiliricalName,
                        dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        positionName = GetPositionsNamePreOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        dateGetMail = JsonConvert.SerializeObject(dataList.workDate, longSetting).Replace(@"""", ""),
                        folder = @"<a href =" + dataList.folder + "> Папка </a>",
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        summaryWeight = Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2),
                        posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        planingCost = dataList.cost,
                        dataList.rate,
                        curency = dataList.curency
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetTableNoPlaningOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTableTNOrder()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSOrder
                        .AsNoTracking()
                        .Include(a => a.CMO_Company)
                        .Include(a => a.CMOSPositionOrder)
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.numberTN == null && a.manufDate != null && a.remove == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.id,
                        userCreate = dataList.AspNetUsers.CiliricalName,
                        dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        positionName = GetPositionsNameOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        dateGetMail = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                        folder = @"<a href =" + dataList.folder + "> Папка </a>",
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        summaryWeight = Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2),
                        posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        planingCost = dataList.cost,
                        dataList.rate,
                        curency = dataList.curency
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetTableTNOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTableNoClothingOrder()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSOrder
                        .AsNoTracking()
                        .Include(a => a.CMO_Company)
                        .Include(a => a.AspNetUsers)
                        .Include(a => a.CMOSPositionOrder)
                        .Where(a => a.finDate == null && a.manufDate != null && a.remove == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        bujetList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetBujetList('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-warning-sign" + '\u0022' + "></span></a></td>",
                        dataList.id,
                        userCreate = dataList.AspNetUsers.CiliricalName,
                        dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        positionName = GetPositionsNameOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        dateGetMail = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                        folder = @"<a href =" + dataList.folder + "> Папка </a>",
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        summaryWeight = Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2),
                        posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        planingCost = dataList.cost,
                        dataList.rate,
                        percentComplited = GetPercentComplited(dataList.id),
                        curency = dataList.curency,
                        weight = dataList.weight,
                        cost = dataList.factCost,
                        dataList.numberTN
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetTableNoClothingOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTableNoClothingOrderApi()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSOrder
                        .AsNoTracking()
                        .Include(a => a.CMO_Company)
                        .Where(a => a.finDate == null && a.manufDate != null && a.remove == false && a.numberTN != null)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.id,
                        positionName = GetPositionsNameOrderReparce(dataList.id),
                        customer = dataList.CMO_Company.name,
                        percentComplited = GetPercentComplited(dataList.id),
                        dataList.numberTN
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetTableNoClothingOrderApi: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private double GetRFactCost(double rate, double curency, double weight)
        {
            if (weight == 0)
                return 0.00;
            else
                return rate * curency * weight;
        }

        public JsonResult RemovePreOrder(int id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var ord = db.CMOSPreOrder.Find(id);
                    if (ord.CMOSOrderPreOrder.Count == 0)
                    {
                        ord.remove = true;
                        db.Entry(ord).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / RemovePreOrder: " + " | " + ex + " | " + id.ToString() + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RemoveOrder(int id)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var ord = db.CMOSOrder.Find(id);
                    ord.remove = true;
                    db.Entry(ord).State = EntityState.Modified;
                    db.SaveChanges();
                    new EmailCMOS(ord, login, 6);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / RemoveOrder: " + " | " + ex + " | " + id.ToString() + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddPreOrder()
        {
            string login = HttpContext.User.Identity.Name;
            int returnId = 0;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    HttpPostedFileBase[] files = new HttpPostedFileBase[Request.Files.Count];
                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i] = Request.Files[i];
                    }
                    int[] ord = GetOrdersArray(Request.Form.ToString());
                    int typeMaterials = GetTypeMaterials(Request.Form.ToString());
                    foreach (var p in ord)
                    {
                        var preorder = new CMOSPreOrder
                        {
                            id_PZ_PlanZakaz = p,
                            id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                            id_CMO_TypeProduct = typeMaterials,
                            dateTimeCreate = DateTime.Now,
                            reOrder = false,
                            remove = false,
                            folder = "",
                            note = ""
                        };
                        db.CMOSPreOrder.Add(preorder);
                        db.SaveChanges();
                        preorder.folder = CreateFolderAndFileForPreOrder(preorder.id, files);
                        CreatingPositionsPreorder(preorder.id, preorder.folder);
                        db.Entry(preorder).State = EntityState.Modified;
                        db.SaveChanges();
                        if (returnId == 0)
                            returnId = preorder.id;
                        new EmailCMOS(preorder, login, 0);
                    }
                    return Json(returnId, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / AddPreOrder: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddBackorder()
        {
            string login = HttpContext.User.Identity.Name;
            int result = 0;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    HttpPostedFileBase[] files = new HttpPostedFileBase[Request.Files.Count];
                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i] = Request.Files[i];
                    }
                    int[] ord = GetOrdersArray(Request.Form.ToString());
                    int customerId = GetTypeMaterials(Request.Form.ToString());
                    var preorder = new CMOSPreOrder
                    {
                        id_PZ_PlanZakaz = 3606,
                        id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                        id_CMO_TypeProduct = 10,
                        dateTimeCreate = DateTime.Now,
                        reOrder = true,
                        remove = false,
                        folder = "",
                        note = GetNoteBackorder(ord)
                    };
                    db.CMOSPreOrder.Add(preorder);
                    db.SaveChanges();
                    result = preorder.id;
                    preorder.folder = CreateFolderAndFileForBackorderrder(preorder.id, files);
                    CreatingPositionsPreorder(preorder.id, preorder.folder);
                    db.Entry(preorder).State = EntityState.Modified;
                    db.SaveChanges();
                    DateTime maxDateCurency = db.CurencyBYN.Max(a => a.date);
                    DateTime curencyDate = db.CurencyBYN.Max(a => a.date);
                    double curency = db.CurencyBYN.Find(curencyDate).USD;
                    var order = new CMOSOrder
                    {
                        aspNetUsersCreateId = preorder.id_AspNetUsersCreate,
                        dateTimeCreate = DateTime.Now,
                        workDate = DateTime.Now.AddHours(6),
                        folder = preorder.folder,
                        cMO_CompanyId = customerId,
                        cost = 0,
                        remove = false,
                        rate = 3.8,
                        weight = 0.0,
                        curency = curency
                    };
                    db.CMOSOrder.Add(order);
                    db.SaveChanges();
                    var relations = new CMOSOrderPreOrder
                    {
                        id_CMOSOrder = order.id,
                        id_CMOSPreOrder = preorder.id
                    };
                    db.CMOSOrderPreOrder.Add(relations);
                    db.SaveChanges();
                    CreatingPositionsBackorder(order.id, order.folder);
                    double summaryWeight = db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == order.id).Sum(a => a.summaryWeight);
                    order.cost = Math.Round(order.rate * curency * summaryWeight, 2);
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    new EmailCMOS(order, login, 4);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / AddBackorder: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddOrder(int[] preordersList, int customerOrderId, DateTime workDate, DateTime? datePlanningGetMaterials)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    DateTime curencyDate = db.CurencyBYN.Max(a => a.date);
                    double curency = db.CurencyBYN.Find(curencyDate).USD;
                    var order = new CMOSOrder
                    {
                        aspNetUsersCreateId = db.AspNetUsers.First(a => a.Email == login).Id,
                        dateTimeCreate = DateTime.Now,
                        workDate = workDate,
                        folder = "",
                        cMO_CompanyId = customerOrderId,
                        cost = 0,
                        remove = false,
                        rate = rate,
                        weight = 0.0,
                        curency = curency
                    };
                    db.CMOSOrder.Add(order);
                    db.SaveChanges();
                    order.folder = CreateFolderAndFileForOrder(order.id);
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    foreach (var preord in preordersList)
                    {
                        var relations = new CMOSOrderPreOrder
                        {
                            id_CMOSOrder = order.id,
                            id_CMOSPreOrder = preord
                        };
                        db.CMOSOrderPreOrder.Add(relations);
                        db.SaveChanges();
                    }
                    CreatingPositionsOrder(order.id);
                    double summaryWeight = db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == order.id).Sum(a => a.summaryWeight);
                    order.cost = Math.Round(order.rate * curency * summaryWeight, 2);
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    CreatingFileOrder(order.id);
                    CreatingStockFileOrder(order.id);
                    new EmailCMOS(order, login, 2, datePlanningGetMaterials);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / AddOrder: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPositionsPreorder(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSPositionPreOrder
                        .AsNoTracking()
                        .Where(a => a.CMOSPreOrderId == id)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.positionNum,
                        dataList.CMOSPreOrderId,
                        dataList.designation,
                        dataList.name,
                        dataList.index,
                        dataList.weight,
                        dataList.quantity,
                        quantity8 = dataList.flow,
                        dataList.summaryWeight,
                        dataList.color,
                        dataList.coating,
                        dataList.note
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetPositionsPreorder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPositionsOrder(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSPositionOrder
                        .AsNoTracking()
                        .Where(a => a.id_CMOSOrder == id)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.positionNum,
                        CMOSPreOrderId = dataList.id_CMOSOrder,
                        dataList.designation,
                        dataList.name,
                        dataList.index,
                        dataList.weight,
                        dataList.quantity,
                        quantity8 = GetFlowPreorders(dataList.designation, dataList.index, dataList.id_CMOSOrder),
                        dataList.summaryWeight,
                        dataList.color,
                        dataList.coating,
                        dataList.note,
                        sku = GetSKU(dataList.name, dataList.index, dataList.designation)
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetPositionsOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private double GetFlowPreorders(string designation, string index, int orderId)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                double res = 0.0;
                try
                {
                    var list = db.CMOSOrderPreOrder.AsNoTracking().Where(a => a.id_CMOSOrder == orderId).ToList();
                    foreach (var p in list)
                    {
                        res += db.CMOSPositionPreOrder.AsNoTracking().Where(a => a.CMOSPreOrderId == p.id_CMOSPreOrder && a.designation == designation && a.index == index).Sum(a => a.flow);
                    }
                }
                catch
                {

                }
                return res;
            }
        }

        public JsonResult GetOrder(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSOrder
                        .AsNoTracking()
                        .Include(a => a.CMOSOrderPreOrder)
                        .Include(a => a.AspNetUsers)
                        .Include(a => a.CMO_Company)
                        .Where(a => a.id == id)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        preordersList = GetPreorderArray(dataList.id),
                        idOrder = dataList.id,
                        aspNetUsersCreateId = dataList.AspNetUsers.CiliricalName,
                        dateTimeCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, longSetting).Replace(@"""", ""),
                        workDate = JsonConvert.SerializeObject(dataList.workDate, shortDefaultSetting).Replace(@"""", ""),
                        manufDate = JsonConvert.SerializeObject(dataList.manufDate, shortDefaultSetting).Replace(@"""", ""),
                        finDate = JsonConvert.SerializeObject(dataList.finDate, shortDefaultSetting).Replace(@"""", ""),
                        customerOrderId = dataList.cMO_CompanyId,
                        numberTN = dataList.numberTN,
                        cost = dataList.cost,
                        factCost = dataList.factCost,
                        planWeight = GetWeigthtOrder(dataList.id),
                        factWeightTN = dataList.weight,
                        curency = dataList.curency,
                        rate = dataList.rate
                    });
                    return Json(data.First(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPositionsPreorderApi(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSOrderPreOrder
                        .AsNoTracking()
                        .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                        .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                        .Include(a => a.CMOSPreOrder.CMOSPositionPreOrder)
                        .Where(a => a.id_CMOSOrder == id)
                        .ToList();
                    List<CMOSPositionPreOrder> listPrd = new List<CMOSPositionPreOrder>();
                    foreach (var prds in query)
                    {
                        var posList = prds.CMOSPreOrder.CMOSPositionPreOrder.Where(a => a.note != "Входит в сб.").OrderBy(a => a.CMOSPreOrderId).ThenBy(a => a.name).ToList();
                        foreach (var prd in posList)
                        {
                            listPrd.Add(prd);
                        }
                    }
                    var data = listPrd.Select(dataList => new
                    {
                        name = dataList.designation + "<" + dataList.index + ">" + dataList.name,
                        code = GetSKUName(dataList.sku.Value),
                        weight = GetWeightSKU(dataList.designation, dataList.index, dataList.newWeight),
                        shortName = dataList.name,
                        norm = dataList.quantity,
                        rate = dataList.flow,
                        loading = dataList.quantity - dataList.flow,
                        order = (dataList.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + dataList.CMOSPreOrder.CMO_TypeProduct.name).Replace("\r\n", ""),
                        color = "RAL" + dataList.color,
                        id = dataList.id,
                        IsWeight = GetIsWeight(dataList.newWeight)
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetPositionsPreorderApi: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private bool GetIsWeight(double? d)
        {
            if (d > 0)
                return true;
            else
                return false;
        }

        private double GetWeightSKU(string designation, string index, Nullable<double> newWeight)
        {
            try
            {
                if (newWeight > 0)
                    return newWeight.Value;
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    return db.SKU.First(a => a.designation == designation && a.indexMaterial == index).weight;
                }
            }
            catch
            {
                return 0.0;
            }
        }

        public string PostPositionsPreorderApi()
        {
            string link = Request.RawUrl.Replace("/CMOS/CMOSS/PostPositionsPreorderApi/", "").Replace(@"\", "");
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    double contrilWeight = 0.0;
                    int id = Convert.ToInt32(link.Substring(0, link.IndexOf("a")));
                    link = link.Substring(link.IndexOf("a") + 1);
                    int rate = Convert.ToInt32(link.Substring(0, link.IndexOf("a")));
                    link = link.Substring(link.IndexOf("a") + 1);
                    double weight = Convert.ToDouble(link);
                    CMOSPositionPreOrder pos = db.CMOSPositionPreOrder.Find(id);
                    pos.flow = rate;
                    pos.weight = weight;
                    try
                    {
                        contrilWeight = db.SKU.First(a => a.designation == pos.designation && a.indexMaterial == pos.index).weight;
                    }
                    catch
                    {
                    }
                    if (contrilWeight != weight)
                        pos.newWeight = weight;
                    db.Entry(pos).State = EntityState.Modified;
                    db.SaveChanges();
                    return "1";
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / PostPositionsPreorderApi: " + " | " + ex + " | " + link);
                return "0";
            }
        }

        private string GetSKUName(int sku)
        {
            if (sku.ToString().Length == 5)
                return sku.ToString();
            else if (sku.ToString().Length == 4)
                return "0" + sku.ToString();
            else if (sku.ToString().Length == 3)
                return "00" + sku.ToString();
            else if (sku.ToString().Length == 2)
                return "000" + sku.ToString();
            else
                return "0000" + sku.ToString();
        }

        [HttpPost]
        public JsonResult UpdateOrder(int idOrder, int customerOrderId, DateTime? manufDate,
            DateTime? finDate, string numberTN, double rate)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    CMOSOrder order = db.CMOSOrder.Find(idOrder);
                    DateTime curencyDate = new DateTime();
                    if (order.manufDate == null)
                    {
                        curencyDate = db.CurencyBYN.Max(a => a.date);
                    }
                    else
                    {
                        curencyDate = db.CurencyBYN.Max(a => a.date);
                    }
                    double curency = db.CurencyBYN.Find(curencyDate).USD;
                    double summaryWeight = db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == order.id).Sum(a => a.summaryWeight);
                    if (rate != order.rate)
                    {
                        order.rate = rate;
                        order.cost = Math.Round(order.rate * curency * summaryWeight, 2);
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (customerOrderId != order.cMO_CompanyId)
                    {
                        order.manufDate = null;
                        order.finDate = null;
                        order.cMO_CompanyId = customerOrderId;
                        order.curency = curency;
                        order.cost = Math.Round(order.rate * curency * summaryWeight, 2);
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        new EmailCMOS(order, login, 2);
                    }
                    else if (numberTN == "" && finDate == null)
                    {
                        if (order.manufDate != manufDate)
                        {
                            order.manufDate = manufDate;
                            order.curency = curency;
                            order.cost = Math.Round(order.rate * curency * summaryWeight, 2);
                            db.Entry(order).State = EntityState.Modified;
                            db.SaveChanges();
                            var prd = db.CMOSOrderPreOrder
                                .Include(a => a.CMOSPreOrder)
                                .First(a => a.id_CMOSOrder == order.id);
                            if (prd.CMOSPreOrder.reOrder == false)
                                new EmailCMOS(order, login, 3);
                        }
                    }
                    else if (finDate == null)
                    {
                        if (order.numberTN == "")
                            order.cost = Math.Round(order.rate * curency * summaryWeight, 2);
                        order.numberTN = numberTN;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();

                        if(order.cMO_CompanyId == 1)
                        {
                            string folder = CreateFolderForArmis(order.id);
                            CreateFileForArmis(folder, order);
                            new EmailArmis(order, login);
                        }
                        else
                        {
                            //try
                            //{
                            //    new EmailOS(order, login);
                            //}
                            //catch (Exception ex)
                            //{
                            //    logger.Error("CMOSSController / UpdateOrder: " + " | " + ex + " | " + login);
                            //}
                            new EmailCMOS(order, login, 7);
                        }
                    }
                    else if (finDate != null)
                    {
                        order.numberTN = numberTN;
                        order.finDate = finDate;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / UpdateOrder: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdatePreordersList()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var sucursalList = db.CMOSPreOrder
                    .Include(d => d.PZ_PlanZakaz)
                    .Include(d => d.CMO_TypeProduct)
                    .Where(d => d.CMOSOrderPreOrder.Count == 0 && d.remove == false)
                    .ToList();
                var data = sucursalList.Select(m => new SelectListItem()
                {
                    Text = m.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + m.CMO_TypeProduct.name,
                    Value = m.id.ToString()
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadingMaterialsC()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    HttpPostedFileBase fiel = Request.Files[0];
                    string fileReplace = Path.GetFileName(fiel.FileName);
                    var fileName = Path.Combine(Server.MapPath("~/Areas/CMOS/ToSKU"), fileReplace);
                    fiel.SaveAs(fileName);
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        FileStream inputStream = fiel.InputStream as FileStream;
                        IApplication application = excelEngine.Excel;
                        IWorkbook workbook = application.Workbooks.OpenReadOnly(fileName);
                        IWorksheet worksheet = workbook.Worksheets[0];
                        int rowsQuantity = worksheet.Rows.Length;
                        SKUStruct[] skuUpList = new SKUStruct[rowsQuantity];
                        var list = worksheet.Rows.ToArray();
                        int i = 0;
                        foreach (var t in list)
                        {
                            SKUStruct skuUp = new SKUStruct
                            {
                                sku1 = (int)t.Cells[0].Number,
                                name = t.Cells[1].Value,
                                weight = Convert.ToDouble(t.Cells[2].Number),
                                indexMaterial = t.Cells[3].Value,
                                designation = t.Cells[4].Value,
                                weightR = Convert.ToDouble(t.Cells[5].Number)
                            };
                            try
                            {
                                skuUp.weight = (double)Convert.ToDecimal(skuUp.weight);
                            }
                            catch
                            {
                                skuUp.weight = 0.0;
                            }
                            try
                            {
                                skuUp.weightR = (double)Convert.ToDecimal(skuUp.weightR);
                            }
                            catch
                            {
                                skuUp.weightR = 0.0;
                            }
                            skuUpList[i] = skuUp;
                            i++;
                        }
                        var dbres = db.SKU.AsNoTracking().ToArray();
                        SKU skuIn = new SKU();
                        foreach (var sku in skuUpList)
                        {
                            if (dbres.Count(a => a.sku1 == sku.sku1) == 0)
                            {
                                SKU skuAdd = new SKU
                                {
                                    designation = sku.designation,
                                    name = sku.name,
                                    indexMaterial = sku.indexMaterial,
                                    sku1 = sku.sku1,
                                    weight = sku.weight,
                                    WeightR = sku.weightR,
                                    WeightArmis = 0.0
                                };
                                db.SKU.Add(skuAdd);
                                db.SaveChanges();
                            }
                            else
                            {
                                skuIn = dbres.First(a => a.sku1 == sku.sku1);
                                skuIn.designation = sku.designation;
                                skuIn.name = sku.name;
                                skuIn.indexMaterial = sku.indexMaterial;
                                skuIn.weight = (double)sku.weight;
                                skuIn.WeightR = (double)sku.weightR;
                                db.Entry(skuIn).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / LoadingMaterialsC: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LoadingInput()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    HttpPostedFileBase fiel = Request.Files[0];
                    string fileReplace = Path.GetFileName(fiel.FileName);
                    var fileName = Path.Combine(Server.MapPath("~/Areas/CMOS/ToInput"), fileReplace);
                    fiel.SaveAs(fileName);
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        FileStream inputStream = fiel.InputStream as FileStream;
                        IApplication application = excelEngine.Excel;
                        IWorkbook workbook = application.Workbooks.OpenReadOnly(fileName);
                        IWorksheet worksheet = workbook.Worksheets[0];
                        var list = worksheet.Rows.ToArray();
                        int i = 0;
                        foreach (var t in list)
                        {
                            if (t.Cells[1].Value != "" && i > 1)
                            {
                                string num = t.Cells[0].Value.Replace(" ", "");
                                if (db.CMOSOrder.Count(a => a.numberTN == num) > 0)
                                {
                                    CMOSOrder order = db.CMOSOrder.First(a => a.numberTN == num);
                                    order.finDate = Convert.ToDateTime(t.Cells[1].Value);
                                    order.factCost = Convert.ToDouble(t.Cells[8].Number) - (Convert.ToDouble(t.Cells[8].Number) / 1.2 * 0.2);
                                    db.Entry(order).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            i++;
                        }
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / LoadingInput: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetActiveOrders()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.CMOSOrder
                    .AsNoTracking()
                    .Include(a => a.CMO_Company)
                    .Include(a => a.AspNetUsers)
                    //.Where(a => a.finDate == null && a.numberTN != null && a.manufDate != null && a.remove == false)
                    .ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBujetList(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var order = db.CMOSOrder.Find(id);
                    var positionsList = db.CMOSPositionOrder
                        .AsNoTracking()
                        .Where(a => a.id_CMOSOrder == id)
                        .ToList();
                    List<PositionBujet> list = new List<PositionBujet>();
                    for (int i = 0; i < 4; i++)
                    {
                        list.Add(new PositionBujet());
                    }
                    list[0].name = "Ставка";
                    list[1].name = "Курс";
                    list[2].name = "Вес";
                    list[3].name = "_Итого";
                    list[0].plan = order.rate.ToString();
                    list[1].plan = order.curency.ToString();
                    list[2].plan = Math.Round(positionsList.Sum(a => a.summaryWeight), 2).ToString();
                    list[3].plan = order.cost.ToString();
                    list[0].factAuto = order.rate.ToString();
                    list[1].factAuto = order.curency.ToString();
                    list[2].factAuto = order.weight.ToString();
                    list[3].factAuto = Math.Round((order.rate * order.curency * order.weight), 2).ToString();
                    list[0].factDoc = order.rate.ToString();
                    list[1].factDoc = order.curency.ToString();
                    list[2].factDoc = order.weight.ToString();
                    list[3].factDoc = order.factCost.ToString();
                    var data = list.Select(dataList => new
                    {
                        dataList.name,
                        dataList.plan,
                        dataList.factAuto,
                        dataList.factDoc
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (id != 0)
                    logger.Error("GetBujetList / GetPositionsOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult LoadingFileArmis()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                HttpPostedFileBase[] files = new HttpPostedFileBase[Request.Files.Count];
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Request.Files[i];
                }
                int ord = GetTypeMaterials(Request.Form.ToString());
                var positions = db.CMOSPositionOrder
                    .AsNoTracking()
                    .Where(a => a.id_CMOSOrder == ord && a.note != "Входит в сб.")
                    .ToList();
                foreach (var p in positions)
                {
                    p.weight = 0.0;
                }
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.Open(files[0].InputStream);
                    workbook.Version = ExcelVersion.Excel97to2003;
                    string fullPath = Path.Combine(Server.MapPath("~/temp"), files[0].FileName);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    bool read = false;
                    int rows = worksheet.Rows.Length;
                    worksheet.Rows[0].Cells[0].Text = ord.ToString() + ": " + GetPositionsNameOrder(ord).Replace("\r\n\n", "; ");
                    for (int i = 0; i < rows; i++)
                    {
                        if (read == false)
                        {
                            if (worksheet.Rows[i].Cells[0].Value == "Наименование товара")
                            {
                                read = true;
                                i += 3;
                            }
                        }
                        else
                        {
                            if (worksheet.Rows[i].Cells[0].Value == "ИТОГО")
                            {
                                worksheet.Rows[1].Cells[0].Text = "Кол-во строк: " + (i + 2).ToString();
                                i = rows;
                            }
                            else
                            {
                                string postName = worksheet.Rows[i].Cells[0].Value.Replace(@",  ЗАО ""АрмисИнвестГрупп""", "");
                                string name = worksheet.Rows[i].Cells[0].Value.Replace(@",  ЗАО ""АрмисИнвестГрупп""", "");
                                string[] split = postName.Split();
                                if (split[0] == "")
                                {
                                    name = name.Substring(1, name.Length - 1);
                                    postName = postName.Substring(1, postName.Length - 1);
                                }
                                var indexPos = name.IndexOf(">");
                                name = name.Substring(0, indexPos);
                                postName = postName.Substring(indexPos);
                                if (double.IsNaN(worksheet.Rows[i].Cells[20].Number))
                                    worksheet.Rows[i].Cells[20].Number = 0.0;
                                if (double.IsNaN(worksheet.Rows[i].Cells[25].Number))
                                    worksheet.Rows[i].Cells[25].Number = 0.0;
                                if (double.IsNaN(worksheet.Rows[i].Cells[28].Number))
                                    worksheet.Rows[i].Cells[28].Number = 0.0;
                                if (double.IsNaN(worksheet.Rows[i].Cells[33].Number))
                                    worksheet.Rows[i].Cells[33].Number = 0.0;
                                worksheet.Rows[i].Cells[0].Text = name + postName;
                                double quentity = worksheet.Rows[i].Cells[13].Number;
                                try
                                {
                                    var findPosition = positions.First(a => a.designation + " <" + a.index == name);
                                    findPosition.weight += quentity;
                                    if (findPosition.quantity != quentity)
                                    {
                                        worksheet.Rows[i].Cells[38].Text = "Неверное кол-во, заложено: " + Math.Round(findPosition.quantity, 2).ToString();
                                        worksheet.Rows[i].Cells[38].CellStyle.ColorIndex = ExcelKnownColors.Light_yellow;
                                    }
                                }
                                catch
                                {
                                    worksheet.Rows[i].Cells[38].Text = "Позиция не найдена";
                                    worksheet.Rows[i].Cells[38].CellStyle.ColorIndex = ExcelKnownColors.Red2;
                                }
                            }
                        }
                    }
                    var notFoundPositions = positions.Where(a => a.weight == 0.0).ToList();
                    int j = 0;
                    foreach (var p in notFoundPositions)
                    {
                        worksheet.Rows[j].Cells[46].Value = p.designation + "<" + p.index + ">" + p.name + ", в количестве: " + Math.Round(p.quantity, 2);
                        j++;
                    }
                    workbook.SaveAs(@"\\192.168.1.16\public$\Финансовый отдел\Армис для загрузки\" + ord.ToString() + ".xls");
                }
                return Json(1);
            }
        }

        [HttpPost]
        public ActionResult LoadingFileGratius()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                HttpPostedFileBase[] files = new HttpPostedFileBase[Request.Files.Count];
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Request.Files[i];
                }
                int ord = GetTypeMaterials(Request.Form.ToString());
                var positions = db.CMOSPositionOrder
                    .AsNoTracking()
                    .Where(a => a.id_CMOSOrder == ord && a.note != "Входит в сб.")
                    .ToList();
                foreach (var p in positions)
                {
                    p.weight = 0.0;
                }
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.Open(files[0].InputStream);
                    string fullPath = Path.Combine(Server.MapPath("~/temp"), files[0].FileName);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    bool read = false;
                    int rows = worksheet.Rows.Length;
                    for (int i = 0; i < rows; i++)
                    {
                        if (read == false)
                        {
                            if (worksheet.Rows[i].Cells[0].Value == "№ п/п")
                            {
                                read = true;
                            }
                        }
                        else
                        {
                            if (worksheet.Rows[i].Cells[0].Value == "")
                                i = rows;
                            else
                            {
                                string name = worksheet.Rows[i].Cells[1].Value.Replace(" ", "") + " <" + worksheet.Rows[i].Cells[3].Value.Replace(" ", "");
                                worksheet.Rows[i].Cells[1].Text = name + "> " + worksheet.Rows[i].Cells[2].Text;
                                double quentity = worksheet.Rows[i].Cells[4].Number;
                                try
                                {
                                    var findPosition = positions.First(a => a.designation + "<" + a.index == name);
                                    findPosition.weight += quentity;
                                    if (findPosition.quantity != quentity)
                                    {
                                        worksheet.Rows[i].Cells[9].Text = "Неверное кол-во, заложено: " + Math.Round(findPosition.quantity, 2).ToString();
                                        worksheet.Rows[i].Cells[9].CellStyle.ColorIndex = ExcelKnownColors.Yellow;
                                    }
                                }
                                catch
                                {
                                    worksheet.Rows[i].Cells[9].Text = "Позиция не найдена";
                                    worksheet.Rows[i].Cells[9].CellStyle.ColorIndex = ExcelKnownColors.Red2;
                                }
                            }
                        }
                    }
                    var notFoundPositions = positions.Where(a => a.weight == 0.0).ToList();
                    int j = 8;
                    foreach (var p in notFoundPositions)
                    {
                        worksheet.Rows[j].Cells[8].Value = p.designation + " <" + p.index + "> " + p.name + ", в количестве: " + Math.Round(p.quantity, 2);
                        worksheet.Rows[j].Cells[8].CellStyle.ColorIndex = ExcelKnownColors.Yellow;
                        j++;
                    }
                    workbook.SaveAs(@"\\192.168.1.16\public$\Финансовый отдел\Армис для загрузки\" + ord.ToString() + ".xls");
                }
                return Json(1);
            }
        }

        [HttpPost]
        public ActionResult LoadingFileEcowood()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                HttpPostedFileBase[] files = new HttpPostedFileBase[Request.Files.Count];
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Request.Files[i];
                }
                int ord = GetTypeMaterials(Request.Form.ToString());
                var positions = db.CMOSPositionOrder
                    .AsNoTracking()
                    .Where(a => a.id_CMOSOrder == ord && a.note != "Входит в сб.")
                    .ToList();
                foreach (var p in positions)
                {
                    p.weight = 0.0;
                }
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.Open(files[0].InputStream);
                    string fullPath = Path.Combine(Server.MapPath("~/temp"), files[0].FileName);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    bool read = false;
                    int rows = worksheet.Rows.Length;
                    string[] nameArray = new string[rows];
                    for (int i = 0; i < rows; i++)
                    {
                        if (read == false)
                        {
                            if (worksheet.Rows[i].Cells[0].Value == "№п/п")
                            {
                                read = true;
                                for (int k = i + 1; k < rows; k++)
                                {
                                    nameArray[k] = worksheet.Rows[k].Cells[2].Text;
                                }
                                worksheet.InsertColumn(3);
                                worksheet.InsertColumn(3);
                            }
                        }
                        else
                        {
                            if (worksheet.Rows[i].Cells[0].Value == "")
                                i = rows;
                            else
                            {
                                string name = worksheet.Rows[i].Cells[1].Value.Replace(" ", "") + " <" + worksheet.Rows[i].Cells[5].Value.Replace(" ", "");
                                double quentity = worksheet.Rows[i].Cells[6].Number;
                                worksheet.Rows[i].Cells[2].Text = nameArray[i];
                                worksheet.Rows[i].Cells[3].Text = name + "> " + nameArray[i];
                                worksheet.Rows[i].Cells[4].Text = "шт";
                                try
                                {
                                    var findPosition = positions.First(a => a.designation + " <" + a.index == name && a.quantity == quentity);
                                    findPosition.weight = quentity;
                                    if (findPosition.quantity != quentity)
                                    {
                                        worksheet.Rows[i].Cells[9].Text = "Неверное кол-во, заложено: " + Math.Round(findPosition.quantity, 2).ToString();
                                        worksheet.Rows[i].Cells[9].CellStyle.ColorIndex = ExcelKnownColors.Yellow;
                                    }
                                }
                                catch
                                {
                                    worksheet.Rows[i].Cells[9].Text = "Позиция не найдена";
                                    worksheet.Rows[i].Cells[9].CellStyle.ColorIndex = ExcelKnownColors.Red2;
                                }
                            }
                        }
                    }
                    var notFoundPositions = positions.Where(a => a.weight == 0.0).ToList();
                    int j = 0;
                    foreach (var p in notFoundPositions)
                    {
                        worksheet.Rows[j].Cells[8].Value = p.designation + " <" + p.index + "> " + p.name + ", в количестве: " + Math.Round(p.quantity, 2);
                        worksheet.Rows[j].Cells[8].CellStyle.ColorIndex = ExcelKnownColors.Yellow;
                        j++;
                    }
                    workbook.SaveAs(@"\\192.168.1.16\public$\Финансовый отдел\Армис для загрузки\" + ord.ToString() + ".xls");
                }
                return Json(1);
            }
        }

        [HttpGet]
        public ActionResult Download(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/temp"), fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }

        private double GetCurrency(DateTime date)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    try
                    {
                        return db.CurencyBYN.First(a => a.date == date).USD;
                    }
                    catch
                    {
                        date = db.CurencyBYN.Max(a => a.date);
                        return db.CurencyBYN.First(a => a.date == date).USD;
                    }
                }
            }
            catch
            {
                return 0.0;
            }
        }

        private string GetPercentComplited(int id)
        {
            double weightGet = 0;
            double summaryWeight = 0;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var prdList = db.CMOSOrderPreOrder.AsNoTracking().Where(a => a.id_CMOSOrder == id).ToList();
                foreach (var prdId in prdList)
                {
                    var pos = db.CMOSPositionPreOrder.AsNoTracking().Where(a => a.CMOSPreOrderId == prdId.id_CMOSPreOrder && a.note != "Входит в сб.").ToList();
                    summaryWeight += pos.Sum(a => a.quantity);
                    foreach (var p in pos)
                    {
                        weightGet += p.flow;
                    }
                }
                double returnPercent = (weightGet / summaryWeight * 100);
                if (returnPercent > 100.0)
                    return "100";

                return Math.Round(returnPercent, 2).ToString();
            }
        }

        private double GetWeigthtOrder(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    return Math.Round(db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == id).Sum(a => a.summaryWeight), 4);
                }
            }
            catch
            {
                return 0.0;
            }
        }

        private string GetPreorderArray(int idOrder)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string res = "";
                var list = db.CMOSOrderPreOrder
                    .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                    .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                    .Where(a => a.id_CMOSOrder == idOrder)
                    .ToList();
                foreach (var prd in list)
                {
                    res += prd.id + " - " + prd.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + ": " + prd.CMOSPreOrder.CMO_TypeProduct.name + "; ";
                }
                return res;
            }
        }

        private void CreatingFileOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string sizeTable = "A1:J" + (db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == id).Count() + 8).ToString();
                string sizeTableForGrig = "A4:J" + (db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == id).Count() + 8).ToString();
                var positionsList = db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == id && a.note != "Входит в сб.").ToList();
                string folder = db.CMOSOrder.Find(id).folder;
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    IStyle style = workbook.Styles.Add("FullStyle");
                    style.Font.Size = 8;
                    style.Font.FontName = "Arial";
                    style.Font.Bold = false;
                    style.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet[sizeTable].RowHeight = 12.0;
                    worksheet[sizeTable].CellStyle = style;
                    IRange range = worksheet.Range[sizeTableForGrig];
                    range.BorderInside(ExcelLineStyle.Thin);
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["A1"].ColumnWidth = 12.0;
                    worksheet["B1"].ColumnWidth = 50.0;
                    worksheet["C1"].ColumnWidth = 45.0;
                    worksheet["D1"].ColumnWidth = 8.5;
                    worksheet["E1"].ColumnWidth = 13.17;
                    worksheet["F1"].ColumnWidth = 14.3;
                    worksheet["G1"].ColumnWidth = 15.33;
                    worksheet["H1"].ColumnWidth = 9.67;
                    worksheet["I1"].ColumnWidth = 11.83;
                    worksheet["J1"].ColumnWidth = 30.5;
                    worksheet.Range["A2:J2"].Merge();
                    worksheet.Range["A2"].Text = "Реестр заказных позиций для изделия (изделий) №: " + id.ToString() + " от " + DateTime.Now.ToShortDateString() + " (" + GetPositionsNameOrder(id) + ")";
                    worksheet.Range["A2"].CellStyle.Font.Bold = true;
                    worksheet["A2"].RowHeight = 18.0;
                    worksheet.Range["A2"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range["A4:J4"].CellStyle.Font.Bold = true;
                    worksheet.Range["A4:J4"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["A4"].Text = "№ п/п";
                    worksheet["B4"].Text = "Обозначение";
                    worksheet["C4"].Text = "Наименование";
                    worksheet["D4"].Text = "Индекс";
                    worksheet["E4"].Text = "Вес, кг";
                    worksheet["F4"].Text = "Количество";
                    worksheet["G4"].Text = "Общий вес, кг";
                    worksheet["H4"].Text = "Цвет RAL";
                    worksheet["I4"].Text = "Покрытие";
                    worksheet["J4"].Text = "Примечание";
                    worksheet.Range["A5:J5"].Merge();
                    worksheet.Range["A5"].Text = "Заказные детали:";
                    worksheet.Range["A5"].CellStyle.Font.Bold = true;
                    int rowNum = 6;
                    foreach (var pos in positionsList)
                    {
                        worksheet.Range[rowNum, 1].Text = pos.positionNum;
                        worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 2].Text = pos.designation;
                        worksheet.Range[rowNum, 3].Text = pos.name;
                        worksheet.Range[rowNum, 4].Text = pos.index;
                        worksheet.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 5].NumberFormat = "0.0000";
                        worksheet.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 5].Number = pos.weight;
                        worksheet.Range[rowNum, 6].NumberFormat = "0.0000";
                        worksheet.Range[rowNum, 6].Number = pos.quantity;
                        worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 7].NumberFormat = "0.0000";
                        worksheet.Range[rowNum, 7].Number = pos.summaryWeight;
                        worksheet.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 8].Text = pos.color;
                        worksheet.Range[rowNum, 8].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 9].Text = pos.coating;
                        worksheet.Range[rowNum, 9].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 10].Text = pos.note;
                        rowNum++;
                    }
                    worksheet.Range[rowNum, 4].CellStyle.Font.Bold = true;
                    worksheet.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                    worksheet.Range[rowNum, 5].CellStyle.Font.Bold = true;
                    worksheet.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[rowNum, 6].CellStyle.Font.Bold = true;
                    worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[rowNum, 7].CellStyle.Font.Bold = true;
                    worksheet.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range["A" + rowNum.ToString() + ":D" + rowNum.ToString()].Merge();
                    worksheet.Range[rowNum, 1].Text = "Итого:";
                    worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                    worksheet.Range[rowNum, 1].CellStyle.Font.Bold = true;
                    worksheet.Range[rowNum, 5].Text = "-";
                    worksheet.Range[rowNum, 6].NumberFormat = "0.0000";
                    worksheet.Range[rowNum, 7].NumberFormat = "0.0000";
                    worksheet.Range[rowNum, 6].Number = positionsList.Sum(a => a.quantity);
                    worksheet.Range[rowNum, 7].Number = positionsList.Sum(a => a.summaryWeight);
                    worksheet.Range["A" + (rowNum + 1).ToString() + ":J" + (rowNum + 1).ToString()].Merge();
                    rowNum += 2;
                    worksheet.Range["A" + rowNum.ToString() + ":J" + rowNum.ToString()].Merge();
                    var materialsList = db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == id && a.note == "Входит в сб.").ToList();
                    worksheet.Range[rowNum, 1].Text = "Детали, входящие в сборки:";
                    worksheet.Range[rowNum, 1].CellStyle.Font.Bold = true;
                    rowNum++;
                    foreach (var pos in materialsList)
                    {
                        worksheet.Range[rowNum, 1].Text = pos.positionNum;
                        worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 2].Text = pos.designation;
                        worksheet.Range[rowNum, 3].Text = pos.name;
                        worksheet.Range[rowNum, 4].Text = pos.index;
                        worksheet.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 5].NumberFormat = "0.0000";
                        worksheet.Range[rowNum, 5].Number = pos.weight;
                        worksheet.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 6].NumberFormat = "0.0000";
                        worksheet.Range[rowNum, 6].Number = pos.quantity;
                        worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 7].NumberFormat = "0.0000";
                        worksheet.Range[rowNum, 7].Number = pos.summaryWeight;
                        worksheet.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 8].Text = pos.color;
                        worksheet.Range[rowNum, 8].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 9].Text = pos.coating;
                        worksheet.Range[rowNum, 9].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 10].Text = pos.note;
                        rowNum++;
                    }
                    workbook.SaveAs(folder + "Заказ изделий из ЛМ №_" + id.ToString() + ".xlsx");
                }
            }
        }

        private void CreatingStockFileOrder(int id)
        {
            string saveDirectory = CreateFolderForStock(id);
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var preordersList = db.CMOSOrderPreOrder
                    .Include(a => a.CMOSPreOrder.CMOSPositionPreOrder)
                    .Where(a => a.id_CMOSOrder == id)
                    .ToList();
                int lenghtGrid = 0;
                foreach (var preorder in preordersList)
                {
                    lenghtGrid += preorder.CMOSPreOrder.CMOSPositionPreOrder.Where(a => a.note != "Входит в сб.").Count();
                    lenghtGrid++;
                }
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    IStyle style = workbook.Styles.Add("FullStyle");
                    style.Font.Size = 8;
                    style.Font.FontName = "Arial";
                    style.Font.Bold = false;
                    style.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    string sizeTable = "A1:N" + (lenghtGrid + 6).ToString();
                    string sizeTableForGrig = "A4:N" + (lenghtGrid + 6).ToString();
                    worksheet[sizeTable].RowHeight = 12.0;
                    worksheet[sizeTable].CellStyle = style;
                    IRange range = worksheet.Range[sizeTableForGrig];
                    range.BorderInside(ExcelLineStyle.Thin);
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["A1"].ColumnWidth = 6.0;
                    worksheet["B1"].ColumnWidth = 8.9;
                    worksheet["C1"].ColumnWidth = 27.0;
                    worksheet["D1"].ColumnWidth = 27.0;
                    worksheet["E1"].ColumnWidth = 27.0;
                    worksheet["F1"].ColumnWidth = 7.7;
                    worksheet["G1"].ColumnWidth = 7.7;
                    worksheet["H1"].ColumnWidth = 10.0;
                    worksheet["I1"].ColumnWidth = 10.0;
                    worksheet["J1"].ColumnWidth = 11.4;
                    worksheet["K1"].ColumnWidth = 9.0;
                    worksheet["L1"].ColumnWidth = 11.5;
                    worksheet["M1"].ColumnWidth = 11.5;
                    worksheet["N1"].ColumnWidth = 27.0;
                    worksheet.Range["A2:N2"].Merge();
                    worksheet.Range["A2"].Text = "Реестр заказных позиций для изделия (изделий) №: " + id.ToString() + " от " + DateTime.Now.ToShortDateString() + " (" + GetPositionsNameOrder(id) + ")";
                    worksheet.Range["A2"].CellStyle.Font.Bold = true;
                    worksheet["A2"].RowHeight = 18.0;
                    worksheet.Range["A2"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range["A4:N4"].CellStyle.Font.Bold = true;
                    worksheet.Range["A4:N4"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["A4"].Text = "№ п/п";
                    worksheet["B4"].Text = "План-заказ";
                    worksheet["C4"].Text = "Полуфабрикат";
                    worksheet["D4"].Text = "Обозначение";
                    worksheet["E4"].Text = "Наименование";
                    worksheet["F4"].Text = "Индекс";
                    worksheet["G4"].Text = "Вес, кг";
                    worksheet["H4"].Text = "Количество";
                    worksheet["I4"].Text = "Поступило";
                    worksheet["J4"].Text = "Общий вес, кг";
                    worksheet["K4"].Text = "Цвет RAL";
                    worksheet["L4"].Text = "Покрытие";
                    worksheet["M4"].Text = "ИД. 1с7";
                    worksheet["N4"].Text = "Примечание";
                    worksheet.Range["A5:N5"].Merge();
                    worksheet.Range["A5"].Text = "Заказные детали:";
                    worksheet.Range["A5"].CellStyle.Font.Bold = true;
                    int rowNum = 6;
                    foreach (var prd in preordersList)
                    {
                        var preorder = db.CMOSPositionPreOrder
                            .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                            .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                            .Where(a => a.CMOSPreOrderId == prd.id_CMOSPreOrder && a.note != "Входит в сб.")
                            .ToList();
                        foreach (var pos in preorder)
                        {
                            worksheet.Range[rowNum, 1].Text = pos.positionNum;
                            worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            worksheet.Range[rowNum, 2].Text = pos.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString();
                            worksheet.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            worksheet.Range[rowNum, 3].Text = pos.CMOSPreOrder.CMO_TypeProduct.name;
                            worksheet.Range[rowNum, 4].Text = pos.designation;
                            worksheet.Range[rowNum, 5].Text = pos.name;
                            worksheet.Range[rowNum, 6].Text = pos.index;
                            worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            worksheet.Range[rowNum, 7].NumberFormat = "0.0000";
                            worksheet.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            worksheet.Range[rowNum, 7].Number = pos.weight;
                            worksheet.Range[rowNum, 8].NumberFormat = "0.0000";
                            worksheet.Range[rowNum, 8].Number = pos.quantity;
                            worksheet.Range[rowNum, 8].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            worksheet.Range[rowNum, 9].NumberFormat = "0.0000";
                            worksheet.Range[rowNum, 9].Number = pos.flow;
                            worksheet.Range[rowNum, 9].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            worksheet.Range[rowNum, 10].NumberFormat = "0.0000";
                            worksheet.Range[rowNum, 10].Number = pos.summaryWeight;
                            worksheet.Range[rowNum, 10].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheet.Range[rowNum, 11].Text = pos.color;
                            worksheet.Range[rowNum, 11].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            worksheet.Range[rowNum, 12].Text = pos.coating;
                            worksheet.Range[rowNum, 12].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                            worksheet.Range[rowNum, 13].Text = pos.sku.ToString();
                            worksheet.Range[rowNum, 14].Text = pos.note;
                            rowNum++;
                        }
                        worksheet.Range[rowNum, 4].CellStyle.Font.Bold = true;
                        worksheet.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 5].CellStyle.Font.Bold = true;
                        worksheet.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 6].CellStyle.Font.Bold = true;
                        worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 7].CellStyle.Font.Bold = true;
                        worksheet.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range["A" + rowNum.ToString() + ":D" + rowNum.ToString()].Merge();
                        worksheet.Range[rowNum, 1].Text = "Итого:";
                        worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 1].CellStyle.Font.Bold = true;
                        worksheet.Range[rowNum, 5].Text = "-";
                        worksheet.Range[rowNum, 6].NumberFormat = "0.0000";
                        worksheet.Range[rowNum, 7].NumberFormat = "0.0000";
                        worksheet.Range[rowNum, 6].Number = preorder.Sum(a => a.quantity);
                        worksheet.Range[rowNum, 7].Number = preorder.Sum(a => a.summaryWeight);
                        rowNum++;
                        rowNum++;
                    }
                    workbook.SaveAs(saveDirectory + "Заказ изделий из ЛМ №_" + id.ToString() + ".xlsx");
                }
            }
        }

        private void CreatingPositionsBackorder(int preorderId, string path)
        {
            List<string> fiels = GetFileArray(path);
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                foreach (var fiel in fiels)
                {
                    try
                    {
                        using (ExcelEngine excelEngine = new ExcelEngine())
                        {
                            IApplication application = excelEngine.Excel;
                            IWorkbook workbook = application.Workbooks.Open(fiel);
                            IWorksheet worksheet = workbook.Worksheets[0];
                            var tmp = worksheet.Rows[0].DisplayText.Substring(0, 6);
                            if (tmp == "Реестр")
                            {
                                int rows = worksheet.Rows.Length;
                                for (int i = 0; i < rows; i++)
                                {
                                    try
                                    {
                                        CMOSPositionOrder pos = new CMOSPositionOrder
                                        {
                                            id_CMOSOrder = preorderId,
                                            positionNum = worksheet.Rows[i].Cells[0].Value,
                                            designation = worksheet.Rows[i].Cells[1].Value,
                                            name = worksheet.Rows[i].Cells[2].Value,
                                            index = worksheet.Rows[i].Cells[3].Value,
                                            weight = Convert.ToDouble(worksheet.Rows[i].Cells[4].Value),
                                            quantity = Convert.ToDouble(worksheet.Rows[i].Cells[5].Value),
                                            summaryWeight = Convert.ToDouble(worksheet.Rows[i].Cells[6].Value),
                                            color = worksheet.Rows[i].Cells[7].Value,
                                            coating = worksheet.Rows[i].Cells[8].Value,
                                            note = worksheet.Rows[i].Cells[9].Value
                                        };
                                        db.CMOSPositionOrder.Add(pos);
                                        db.SaveChanges();
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void CreatingPositionsOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                int positionCounter = 1;
                List<CMOSPositionOrder> prepositionsList = new List<CMOSPositionOrder>();
                List<MaterialsOrdObj> materialsList = new List<MaterialsOrdObj>();
                List<CMOSPositionOrder> positionsList = new List<CMOSPositionOrder>();

                var preordersList = db.CMOSOrderPreOrder.Where(a => a.id_CMOSOrder == id).ToList();
                foreach (var prd in preordersList)
                {
                    var posList = db.CMOSPositionPreOrder.Where(a => a.CMOSPreOrderId == prd.id_CMOSPreOrder).ToList();
                    foreach (var data in posList)
                    {
                        CMOSPositionOrder pos = new CMOSPositionOrder
                        {
                            id_CMOSOrder = id,
                            positionNum = "",
                            designation = data.designation,
                            name = data.name,
                            index = data.index,
                            weight = data.weight,
                            quantity = data.quantity,
                            summaryWeight = data.summaryWeight,
                            color = data.color,
                            coating = data.coating,
                            note = data.note
                        };
                        prepositionsList.Add(pos);

                        int count = materialsList.Count(a => a.Designation == pos.designation && a.Name == pos.name && a.Index == pos.index && a.Color == pos.color && a.Coating == pos.coating);

                        if (count == 0)
                        {
                            MaterialsOrdObj material = new MaterialsOrdObj
                            {
                                Designation = pos.designation,
                                Name = pos.name,
                                Index = pos.index,
                                Quantity = 0.0,
                                Color = pos.color,
                                Coating = pos.coating,
                                Weigth = pos.weight,
                                Note = pos.note
                            };
                            materialsList.Add(material);
                        }
                    }
                }
                foreach (var mt in materialsList)
                {
                    CMOSPositionOrder pos = new CMOSPositionOrder
                    {
                        id_CMOSOrder = id,
                        positionNum = positionCounter.ToString(),
                        designation = mt.Designation,
                        name = mt.Name,
                        index = mt.Index,
                        weight = mt.Weigth,
                        quantity = prepositionsList.Where(a => a.designation == mt.Designation && a.name == mt.Name && a.index == mt.Index && a.color == mt.Color && a.coating == mt.Coating).Sum(a => a.quantity),
                        summaryWeight = prepositionsList.Where(a => a.designation == mt.Designation && a.name == mt.Name && a.index == mt.Index && a.color == mt.Color && a.coating == mt.Coating).Sum(a => a.quantity) * mt.Weigth,
                        color = mt.Color,
                        coating = mt.Coating,
                        note = mt.Note
                    };
                    db.CMOSPositionOrder.Add(pos);
                    db.SaveChanges();
                    positionCounter++;
                }
            }
        }

        private string GetNoteBackorder(int[] ordersArray)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string note = "";
                foreach (var ord in ordersArray)
                {
                    note += db.PZ_PlanZakaz.Find(ord).PlanZakaz.ToString() + "; ";
                }
                return note + " - дозаказ";
            }
        }

        private void CreatingPositionsPreorder(int preorderId, string path)
        {
            List<string> fiels = GetFileArray(path);
            List<MaterialsPreObj> materialsInStock = new List<MaterialsPreObj>();
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                foreach (var fiel in fiels)
                {
                    try
                    {
                        using (ExcelEngine excelEngine = new ExcelEngine())
                        {
                            IApplication application = excelEngine.Excel;
                            IWorkbook workbook = application.Workbooks.Open(fiel);
                            IWorksheet worksheet = workbook.Worksheets[0];
                            var tmp = worksheet.Rows[0].DisplayText.Substring(0, 6);
                            if (tmp == "Реестр")
                            {
                                int rows = worksheet.Rows.Length;
                                for (int i = 0; i < rows; i++)
                                {
                                    try
                                    {
                                        CMOSPositionPreOrder pos = new CMOSPositionPreOrder
                                        {
                                            CMOSPreOrderId = preorderId,
                                            positionNum = worksheet.Rows[i].Cells[0].Value,
                                            designation = worksheet.Rows[i].Cells[1].Value,
                                            name = worksheet.Rows[i].Cells[2].Value,
                                            index = worksheet.Rows[i].Cells[3].Value,
                                            weight = Convert.ToDouble(worksheet.Rows[i].Cells[4].Value),
                                            quantity = Convert.ToDouble(worksheet.Rows[i].Cells[5].Value),
                                            quantity8 = 0,
                                            summaryWeight = Convert.ToDouble(worksheet.Rows[i].Cells[6].Value),
                                            color = worksheet.Rows[i].Cells[7].Value,
                                            coating = worksheet.Rows[i].Cells[8].Value,
                                            note = worksheet.Rows[i].Cells[9].Value,
                                            flow = 0
                                        };
                                        db.CMOSPositionPreOrder.Add(pos);
                                        db.SaveChanges();
                                        try
                                        {
                                            var sku = db.SKU.First(a => a.designation == pos.designation && a.name == pos.name && a.indexMaterial == pos.index);
                                            pos.sku = sku.sku1;
                                            db.Entry(pos).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            else
                            {
                                int rows = worksheet.Rows.Length;
                                for (int i = 0; i < rows; i++)
                                {
                                    try
                                    {
                                        double res = Convert.ToDouble(worksheet.Rows[i].Cells[4].Value);
                                        if (res > 0)
                                            materialsInStock.Add(new MaterialsPreObj
                                            {
                                                Designation = worksheet.Rows[i].Cells[0].Value,
                                                Name = worksheet.Rows[i].Cells[1].Value,
                                                Index = worksheet.Rows[i].Cells[2].Value,
                                                Quantity = res
                                            });
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                foreach (var material in materialsInStock)
                {
                    try
                    {
                        CMOSPositionPreOrder pos = db.CMOSPositionPreOrder
                            .First(a => a.CMOSPreOrderId == preorderId &&
                            a.designation == material.Designation && a.name == material.Name &&
                            a.index == material.Index);
                        pos.quantity8 = material.Quantity;
                        db.Entry(pos).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }
            }
        }

        private int[] GetOrdersArray(string str)
        {
            str = str.Split('=')[0];
            var strList = str.Split(new string[] { "%2c" }, StringSplitOptions.RemoveEmptyEntries);
            int[] res = new int[strList.Length];
            for (int i = 0; i < strList.Length; i++)
            {
                res[i] = Convert.ToInt32(strList[i]);
            }
            return res;
        }

        private int GetTypeMaterials(string str)
        {
            str = str.Split('=').Last();
            return Convert.ToInt32(str);
        }

        private string CreateFolderAndFileForPreOrder(int id, HttpPostedFileBase[] fileUploadArray)
        {
            string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\CMOS\\Preorder\\" + id.ToString() + "\\";
            Directory.CreateDirectory(directory);
            for (int i = 0; i < fileUploadArray.Length; i++)
            {
                string fileReplace = Path.GetFileName(fileUploadArray[i].FileName);
                fileReplace = ToSafeFileName(fileReplace);
                var fileName = string.Format("{0}\\{1}", directory, fileReplace);
                fileUploadArray[i].SaveAs(fileName);
                if (fileReplace.Substring(fileReplace.Length - 1) == "s")
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook wbv = excel.Workbooks.Open(directory + fileReplace);
                    Microsoft.Office.Interop.Excel.Worksheet wx = excel.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                    wbv.SaveAs(directory + fileReplace + "x", Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
                    wbv.Close(true, Type.Missing, Type.Missing);
                    excel.Quit();
                    System.IO.File.Delete(directory + fileReplace);
                }
            }
            return directory;
        }

        private string CreateFolderAndFileForBackorderrder(int id, HttpPostedFileBase[] fileUploadArray)
        {
            string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\CMOS\\Backorder\\" + id.ToString() + "\\";
            Directory.CreateDirectory(directory);
            for (int i = 0; i < fileUploadArray.Length; i++)
            {
                string fileReplace = Path.GetFileName(fileUploadArray[i].FileName);
                fileReplace = ToSafeFileName(fileReplace);
                var fileName = string.Format("{0}\\{1}", directory, fileReplace);
                fileUploadArray[i].SaveAs(fileName);
                if (fileReplace.Substring(fileReplace.Length - 1) == "s")
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook wbv = excel.Workbooks.Open(directory + fileReplace);
                    Microsoft.Office.Interop.Excel.Worksheet wx = excel.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
                    wbv.SaveAs(directory + fileReplace + "x", Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
                    wbv.Close(true, Type.Missing, Type.Missing);
                    excel.Quit();
                    System.IO.File.Delete(directory + fileReplace);
                }
            }
            return directory;
        }

        private string CreateFolderForArmis(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\CMOS\\Armis\\" + id.ToString() + "\\";
                Directory.CreateDirectory(directory);
                return directory;
            }
        }

        private string CreateFolderAndFileForOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\CMOS\\Order\\" + id.ToString() + "\\";
                Directory.CreateDirectory(directory);
                return directory;
            }
        }

        private string CreateFolderForStock(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\CMOS\\Stock\\" + id.ToString() + "\\";
                Directory.CreateDirectory(directory);
                return directory;
            }
        }

        private List<string> GetFileArray(string folder)
        {
            var fileList = Directory.GetFiles(folder).ToList();
            return fileList;
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

        private int GetDevision(string login)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return db.AspNetUsers.First(d => d.Email == login).Devision.Value;
                }
                catch
                {
                    return 0;
                }
            }
        }

        private string GetPositionsNamePreOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                try
                {
                    var positionsList = db.CMOSOrderPreOrder
                        .AsNoTracking()
                        .Include(a => a.CMOSPreOrder.CMOSPositionPreOrder)
                        .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                        .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                        .Where(a => a.id_CMOSOrder == id)
                        .ToList();
                    int idPrd = positionsList[0].id_CMOSPreOrder;
                    bool reorder = db.CMOSPreOrder.AsNoTracking().First(a => a.id == idPrd).reOrder;
                    if (reorder == false)
                    {
                        foreach (var dataInList in positionsList)
                        {
                            data += dataInList.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + dataInList.CMOSPreOrder.CMO_TypeProduct.name + "\n";
                        }
                    }
                    else
                    {
                        int preOrderID = db.CMOSOrderPreOrder.First(a => a.id_CMOSOrder == id).id_CMOSPreOrder;
                        data += db.CMOSPreOrder.Find(preOrderID).note;
                    }
                    return data;
                }
                catch
                {
                    int preOrderID = db.CMOSOrderPreOrder.First(a => a.id_CMOSOrder == id).id_CMOSPreOrder;
                    return db.CMOSPreOrder.Find(preOrderID).note;
                }
            }
        }

        private string GetStateOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    var data = db.CMOSOrder.Find(id);
                    if (data.workDate == null)
                        return "Не размещен";
                    else if (data.manufDate == null)
                        return "Ожидание сроков";
                    else if (data.finDate == null)
                        return "Производится";
                    else
                        return "Оприходован";
                }
                catch
                {
                    return "";
                }
            }
        }

        private string GetPositionsNameOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                try
                {
                    var listPos = db.CMOSOrderPreOrder
                        .AsNoTracking()
                        .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                        .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                        .Where(a => a.id_CMOSOrder == id)
                        .ToList();
                    int idPrd = listPos[0].id_CMOSPreOrder;
                    bool reorder = db.CMOSPreOrder.AsNoTracking().First(a => a.id == idPrd).reOrder;
                    if (reorder == false)
                    {
                        foreach (var pos in listPos)
                        {
                            data += pos.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + pos.CMOSPreOrder.CMO_TypeProduct.name + "\n";
                        }
                    }
                    else
                    {
                        int preOrderID = db.CMOSOrderPreOrder.First(a => a.id_CMOSOrder == id).id_CMOSPreOrder;
                        data += db.CMOSPreOrder.Find(preOrderID).note;
                    }
                    return data;
                }
                catch
                {
                    return "";
                }
            }
        }

        private string GetPositionsNameOrderReparce(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                try
                {
                    var listPos = db.CMOSOrderPreOrder
                        .AsNoTracking()
                        .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                        .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                        .Where(a => a.id_CMOSOrder == id)
                        .ToList();
                    int idPrd = listPos[0].id_CMOSPreOrder;
                    bool reorder = db.CMOSPreOrder.AsNoTracking().First(a => a.id == idPrd).reOrder;
                    if (reorder == false)
                    {
                        foreach (var pos in listPos)
                        {
                            data += pos.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + pos.CMOSPreOrder.CMO_TypeProduct.name.Replace("\r\n\r\n", "").Replace("\r\n", "") + "; ";
                        }
                    }
                    else
                    {
                        int preOrderID = db.CMOSOrderPreOrder.First(a => a.id_CMOSOrder == id).id_CMOSPreOrder;
                        data += db.CMOSPreOrder.Find(preOrderID).note;
                    }
                    return data;
                }
                catch
                {
                    return "";
                }
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

        string GetPositionName(CMOSOrder order)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string positions = "";
                var list = db.CMOSPreOrder.AsNoTracking()
                    .Include(a => a.PZ_PlanZakaz)
                    .Include(a => a.CMO_TypeProduct)
                    .Include(a => a.CMOSOrderPreOrder)
                    .Where(a => a.CMOSOrderPreOrder.Count(b => b.id_CMOSOrder == order.id) > 0)
                    .ToList();
                foreach (var d in list)
                {
                    positions += d.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + d.CMO_TypeProduct.name + "; ";
                }
                positions = positions.Replace("\r\n", "");
                return positions;
            }
        }

        double GetOrdersRKOSummaryWeight(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                double res = 0.0;
                var list = db.CMOSPositionPreOrder.AsNoTracking()
                    .Include(a => a.CMOSPreOrder.CMOSOrderPreOrder)
                    .Where(a => a.CMOSPreOrder.CMOSOrderPreOrder.Count(b => b.id_CMOSOrder == id) > 0 && a.note != "Входит в сб.")
                    .ToList();
                foreach (var d in list)
                {
                    try
                    {
                        double rWeight = db.SKU.First(a => a.sku1 == d.sku).WeightR;
                        double fWeight = db.SKU.First(a => a.sku1 == d.sku).weight;
                        if (rWeight > 0)
                        {
                            res += d.quantity * rWeight;
                        }
                        else if (fWeight > 0)
                        {
                            res += d.quantity * fWeight;
                        }
                        else
                        {
                            res += d.summaryWeight;
                        }
                    }
                    catch
                    {
                        res += d.summaryWeight;
                    }
                }
                return res;
            }
        }

        double GetOrdersFactSummaryWeight(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                double res = 0.0;
                var list = db.CMOSPositionPreOrder.AsNoTracking()
                    .Include(a => a.CMOSPreOrder.CMOSOrderPreOrder)
                    .Where(a => a.CMOSPreOrder.CMOSOrderPreOrder.Count(b => b.id_CMOSOrder == id) > 0 && a.note != "Входит в сб.")
                    .ToList();
                foreach (var d in list)
                {
                    try
                    {
                        double fWeight = db.SKU.First(a => a.sku1 == d.sku).weight;
                        if (fWeight > 0)
                        {
                            res += d.quantity * fWeight;
                        }
                        else
                        {
                            res += d.summaryWeight;
                        }
                    }
                    catch
                    {
                        res += d.summaryWeight;
                    }
                }
                return res;
            }
        }

        double GetPreordersSummaryWeight(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                double res = 0.0;
                var list = db.CMOSPositionPreOrder.AsNoTracking()
                    .Include(a => a.CMOSPreOrder.CMOSOrderPreOrder)
                    .Where(a => a.CMOSPreOrder.CMOSOrderPreOrder.Count(b => b.id_CMOSOrder == id) > 0)
                    .ToList();
                foreach (var d in list)
                {
                    res += d.summaryWeight;
                }

                return res;
            }
        }

        double GetPreordersSummaryFactWeight(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                double res = 0.0;
                var list = db.CMOSPositionPreOrder.AsNoTracking()
                    .Where(a => a.CMOSPreOrderId == id && a.note != "Входит в сб.")
                    .ToList();
                foreach (var d in list)
                {
                    double add = db.SKU.First(a => a.sku1 == d.sku).weight * d.quantity;
                    res += add;
                }

                return res;
            }
        }

        double GetOrdersSummaryWeight(CMOSOrder order)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                double res = 0.0;
                var list = db.CMOSPositionOrder.AsNoTracking()
                    .Where(a => a.note != "Входит в сб." && a.id_CMOSOrder == order.id)
                    .ToList();
                foreach (var d in list)
                {
                    res += d.summaryWeight;
                }

                return res;
            }
        }

        public ActionResult GerOrderForArmis()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var ordersList = db.CMOSOrder
                    .AsNoTracking()
                    .Where(a => a.remove == false && a.cMO_CompanyId == 1 && a.finDate != null)
                    .OrderByDescending(a => a.finDate)
                    .ToList();
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];

                    worksheet["A1"].Text = "Данные по состоянию на: " + DateTime.Now.ToLongDateString() + " на " + DateTime.Now.ToShortTimeString();

                    worksheet["A1"].ColumnWidth = 10.0;
                    worksheet["B1"].ColumnWidth = 40.0;
                    worksheet["C1"].ColumnWidth = 10.0;
                    worksheet["D1"].ColumnWidth = 10.0;
                    worksheet["E1"].ColumnWidth = 10.0;
                    worksheet["F1"].ColumnWidth = 10.0;
                    worksheet["G1"].ColumnWidth = 10.0;
                    worksheet["H1"].ColumnWidth = 15.0;
                    worksheet["I1"].ColumnWidth = 15.0;
                    worksheet["J1"].ColumnWidth = 15.0;

                    worksheet["A3"].Text = "Ид. заказа";
                    worksheet["A3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["A3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["A3"].WrapText = true;

                    worksheet["B3"].Text = "Позиции";
                    worksheet["B3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["B3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["B3"].WrapText = true;

                    worksheet["C3"].Text = "№ док. Поступление ТМЦ";
                    worksheet["C3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["C3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["C3"].WrapText = true;

                    worksheet["D3"].Text = "Дата поставки";
                    worksheet["D3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["D3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["D3"].WrapText = true;

                    worksheet["E3"].Text = "Расчетный вес, кг.";
                    worksheet["E3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["E3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["E3"].WrapText = true;

                    worksheet["F3"].Text = "Фактический вес, кг.";
                    worksheet["F3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["F3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["F3"].WrapText = true;

                    worksheet["G3"].Text = "Ставка";
                    worksheet["G3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["G3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["G3"].WrapText = true;

                    worksheet["H3"].Text = "Расчетная стоимость, USD";
                    worksheet["H3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["H3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["H3"].WrapText = true;

                    worksheet["I3"].Text = "Фактическая стоимость, USD";
                    worksheet["I3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["I3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["I3"].WrapText = true;

                    worksheet["J3"].Text = "Стоимость по накладной, бНДС BYN";
                    worksheet["J3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["J3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["J3"].WrapText = true;

                    int rowNum = 4;

                    foreach (var order in ordersList)
                    {
                        double rWeight = 0.0;
                        double fWeight = 0.0;

                        worksheet.Range[rowNum, 1].Text = order.id.ToString();
                        worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 1].WrapText = true;

                        worksheet.Range[rowNum, 2].Text = GetPositionName(order);
                        worksheet.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 2].WrapText = true;

                        worksheet.Range[rowNum, 3].Text = order.numberTN;
                        worksheet.Range[rowNum, 3].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 3].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 3].WrapText = true;

                        worksheet.Range[rowNum, 4].Text = order.finDate.Value.ToShortDateString();
                        worksheet.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 4].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 4].WrapText = true;

                        rWeight = Math.Round(GetPreordersSummaryWeight(order.id), 2);
                        worksheet.Range[rowNum, 5].Text = rWeight.ToString();
                        worksheet.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 5].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 5].WrapText = true;

                        fWeight = Math.Round(GetOrdersFactSummaryWeight(order.id), 2);
                        worksheet.Range[rowNum, 6].Text = fWeight.ToString();
                        worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 6].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 6].WrapText = true;

                        worksheet.Range[rowNum, 7].Text = order.rate.ToString();
                        worksheet.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 7].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 7].WrapText = true;

                        worksheet.Range[rowNum, 8].Text = Math.Round((rWeight * order.rate), 2).ToString();
                        worksheet.Range[rowNum, 8].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 8].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 8].WrapText = true;

                        worksheet.Range[rowNum, 9].Text = Math.Round((fWeight * order.rate), 2).ToString();
                        worksheet.Range[rowNum, 9].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 9].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 9].WrapText = true;

                        worksheet.Range[rowNum, 10].Text = Math.Round(order.factCost.Value, 2).ToString();
                        worksheet.Range[rowNum, 10].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 10].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 10].WrapText = true;

                        rowNum++;
                    }
                    HttpResponse response = HttpContext.ApplicationInstance.Response;
                    try
                    {
                        workbook.SaveAs(DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + "Отчет.xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.Open);
                    }
                    catch
                    {
                    }
                }
            }
            return View();
        }

        private string GetPeriodMonth(int month)
        {
            if (month < 10)
                return "0" + month.ToString();
            else
                return month.ToString();
        }

        public ActionResult ControlReport()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var customerList = db.CMO_Company.AsNoTracking().Where(a => a.active == true).ToList();
                var typeList = db.CMO_TypeProduct.AsNoTracking().Where(a => a.active == true || a.name == "Прочие\r\n").ToList();
                List<MonthResult> monthResults = new List<MonthResult>();
                List<MonthResultType> monthResultTypes = new List<MonthResultType>();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                DateTime controlDate = new DateTime(2020, 10, 1);
                var orders = db.CMOSOrder
                    .AsNoTracking()
                    .Include(a => a.CMO_Company)
                    .Where(a => a.numberTN != null && a.remove == false && a.finDate != null && a.finDate >= controlDate)
                    .ToList();
                var posList1 = db.CMOSPositionPreOrder
                    .AsNoTracking()
                    .Include(a => a.CMOSPreOrder)
                    .Include(a => a.CMOSPreOrder.CMOSOrderPreOrder.Select(b => b.CMOSOrder))
                    .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                    .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                    .Where(a => a.note != "Входит в сб." && a.CMOSPreOrder.CMOSOrderPreOrder.Count() > 0)
                    .ToList();
                var posList = posList1
                    .Where(a => a.CMOSPreOrder.CMOSOrderPreOrder.First().CMOSOrder.numberTN != null && a.CMOSPreOrder.CMOSOrderPreOrder.First().CMOSOrder.finDate != null && a.CMOSPreOrder.CMOSOrderPreOrder.First().CMOSOrder.finDate >= controlDate)
                    .ToList();
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    workbook.Worksheets.Create();
                    workbook.Worksheets.Create();
                    workbook.Worksheets.Create();
                    IRange range;


                    IWorksheet worksheet = workbook.Worksheets[1];
                    worksheet.Name = "Заказы";
                    worksheet["A1"].Text = "Данные по состоянию на: " + DateTime.Now.ToLongDateString() + " на " + DateTime.Now.ToShortTimeString();
                    worksheet["A1"].ColumnWidth = 10.0;
                    worksheet["B1"].ColumnWidth = 40.0;
                    worksheet["C1"].ColumnWidth = 15.0;
                    worksheet["D1"].ColumnWidth = 10.0;
                    worksheet["E1"].ColumnWidth = 10.0;
                    worksheet["F1"].ColumnWidth = 10.0;
                    worksheet["G1"].ColumnWidth = 10.0;
                    worksheet["H1"].ColumnWidth = 10.0;
                    worksheet["I1"].ColumnWidth = 10.0;
                    worksheet["J1"].ColumnWidth = 10.0;
                    worksheet["K1"].ColumnWidth = 15.0;
                    worksheet["L1"].ColumnWidth = 15.0;
                    worksheet["M1"].ColumnWidth = 15.0;
                    worksheet["N1"].ColumnWidth = 15.0;
                    worksheet["O1"].ColumnWidth = 15.0;
                    worksheet["P1"].ColumnWidth = 23.0;
                    worksheet["A3"].Text = "Ид. заказа";
                    worksheet["A3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["A3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["A3"].WrapText = true;
                    range = worksheet.Range["A3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["B3"].Text = "Позиции";
                    worksheet["B3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["B3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["B3"].WrapText = true;
                    range = worksheet.Range["B3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["C3"].Text = "Поставщик";
                    worksheet["C3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["C3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["C3"].WrapText = true;
                    range = worksheet.Range["C3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["D3"].Text = "№ док. Поступление ТМЦ";
                    worksheet["D3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["D3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["D3"].WrapText = true;
                    range = worksheet.Range["D3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["E3"].Text = "Дата поставки";
                    worksheet["E3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["E3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["E3"].WrapText = true;
                    range = worksheet.Range["E3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["F3"].Text = "Вес файла заказа, кг.";
                    worksheet["F3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["F3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["F3"].WrapText = true;
                    range = worksheet.Range["F3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["G3"].Text = "Расчетный вес, кг.";
                    worksheet["G3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["G3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["G3"].WrapText = true;
                    range = worksheet.Range["G3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["H3"].Text = "Фактический вес, кг.";
                    worksheet["H3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["H3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["H3"].WrapText = true;
                    range = worksheet.Range["H3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["I3"].Text = "Ставка";
                    worksheet["I3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["I3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["I3"].WrapText = true;
                    range = worksheet.Range["I3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["J3"].Text = "Курс на дату поставки, USD/BYN";
                    worksheet["J3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["J3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["J3"].WrapText = true;
                    range = worksheet.Range["J3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["K3"].Text = "Стоимость по файлу, USD";
                    worksheet["K3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["K3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["K3"].WrapText = true;
                    range = worksheet.Range["K3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["L3"].Text = "Расчетная стоимость, USD";
                    worksheet["L3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["L3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["L3"].WrapText = true;
                    range = worksheet.Range["L3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["M3"].Text = "Фактическая стоимость, USD";
                    worksheet["M3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["M3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["M3"].WrapText = true;
                    range = worksheet.Range["M3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["N3"].Text = "Стоимость по накладной, бНДС BYN";
                    worksheet["N3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["N3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["N3"].WrapText = true;
                    range = worksheet.Range["N3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["O3"].Text = "Стоимость по накладной, бНДС USD";
                    worksheet["O3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["O3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["O3"].WrapText = true;
                    range = worksheet.Range["O3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheet["P3"].Text = "[Стоимость по файлу] - [Стоимость по накладной], бНДС USD";
                    worksheet["P3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["P3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["P3"].WrapText = true;
                    range = worksheet.Range["P3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    int rowNum = 4;
                    foreach (var order in orders)
                    {
                        string period = order.finDate.Value.Year.ToString() + "." + GetPeriodMonth(order.finDate.Value.Month);
                        if (monthResults.Count(a => a.Period == period) == 0)
                        {
                            foreach (var t in customerList)
                            {
                                monthResults.Add(new MonthResult(period, t.name));
                                foreach (var tp in typeList)
                                {
                                    monthResultTypes.Add(new MonthResultType(period, t.name, tp.name));
                                }
                            }
                        }
                        double fileWeight = Math.Round(GetOrdersSummaryWeight(order), 2);
                        double rWeight = Math.Round(GetOrdersRKOSummaryWeight(order.id), 2);
                        double fWeight = Math.Round(GetOrdersFactSummaryWeight(order.id), 2);
                        double curency = GetCurrency(order.finDate.Value);
                        monthResults.First(a => a.Period == period && a.Customer == order.CMO_Company.name).Weight += fWeight;
                        monthResults.First(a => a.Period == period && a.Customer == order.CMO_Company.name).Cost += order.factCost.Value / curency;
                        var listPreorder = db.CMOSPreOrder
                            .AsNoTracking()
                            .Include(a => a.CMO_TypeProduct)
                            .Where(a => a.CMOSOrderPreOrder.Count(b => b.id_CMOSOrder == order.id) > 0)
                            .ToList();
                        foreach (var t in listPreorder)
                        {
                            var ws = GetPreordersSummaryFactWeight(t.id);
                            double percent = ws / fWeight;
                            double costPreorder = (order.factCost.Value / curency) * percent;
                            monthResultTypes.First(a => a.Period == period && a.Customer == order.CMO_Company.name && a.Type == t.CMO_TypeProduct.name).Weight += ws;
                            monthResultTypes.First(a => a.Period == period && a.Customer == order.CMO_Company.name && a.Type == t.CMO_TypeProduct.name).Cost += costPreorder;
                        }
                        worksheet.Range[rowNum, 1].Text = order.id.ToString();
                        worksheet.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 1].WrapText = true;
                        range = worksheet.Range[rowNum, 1];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 2].Text = GetPositionName(order);
                        worksheet.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 2].WrapText = true;
                        range = worksheet.Range[rowNum, 2];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 3].Text = order.CMO_Company.name.Replace("\r\n", "");
                        worksheet.Range[rowNum, 3].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheet.Range[rowNum, 3].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 3].WrapText = true;
                        range = worksheet.Range[rowNum, 3];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 4].Text = order.numberTN;
                        worksheet.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 4].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 4].WrapText = true;
                        range = worksheet.Range[rowNum, 4];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 5].Text = order.finDate.Value.ToShortDateString();
                        worksheet.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheet.Range[rowNum, 5].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 5].WrapText = true;
                        range = worksheet.Range[rowNum, 5];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 6].Number = fileWeight;
                        worksheet.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 6].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 6].WrapText = true;
                        range = worksheet.Range[rowNum, 6];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 7].Number = rWeight;
                        worksheet.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 7].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 7].WrapText = true;
                        range = worksheet.Range[rowNum, 7];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 8].Number = fWeight;
                        worksheet.Range[rowNum, 8].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 8].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 8].WrapText = true;
                        range = worksheet.Range[rowNum, 8];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 9].Number = order.rate;
                        worksheet.Range[rowNum, 9].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 9].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 9].WrapText = true;
                        range = worksheet.Range[rowNum, 9];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 10].Number = curency;
                        worksheet.Range[rowNum, 10].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 10].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 10].WrapText = true;
                        range = worksheet.Range[rowNum, 10];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 11].Number = Math.Round((fileWeight * order.rate), 2);
                        worksheet.Range[rowNum, 11].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 11].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 11].WrapText = true;
                        range = worksheet.Range[rowNum, 11];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 12].Number = Math.Round((rWeight * order.rate), 2);
                        worksheet.Range[rowNum, 12].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 12].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 12].WrapText = true;
                        range = worksheet.Range[rowNum, 12];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 13].Number = Math.Round((fWeight * order.rate), 2);
                        worksheet.Range[rowNum, 13].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 13].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 13].WrapText = true;
                        range = worksheet.Range[rowNum, 13];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 14].Number = Math.Round(order.factCost.Value, 2);
                        worksheet.Range[rowNum, 14].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 14].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 14].WrapText = true;
                        range = worksheet.Range[rowNum, 14];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 15].Number = Math.Round(order.factCost.Value / curency, 2);
                        worksheet.Range[rowNum, 15].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 15].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 15].WrapText = true;
                        range = worksheet.Range[rowNum, 15];
                        range.BorderAround(ExcelLineStyle.Thin);
                        worksheet.Range[rowNum, 16].Number = Math.Round((fileWeight * order.rate), 2) - Math.Round(order.factCost.Value / curency, 2);
                        worksheet.Range[rowNum, 16].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheet.Range[rowNum, 16].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheet.Range[rowNum, 16].WrapText = true;
                        range = worksheet.Range[rowNum, 16];
                        range.BorderAround(ExcelLineStyle.Thin);
                        rowNum++;
                    }


                    IWorksheet worksheetSummary = workbook.Worksheets[0];
                    worksheetSummary.Name = "Суммарно";
                    worksheetSummary["A1"].Text = "Данные по состоянию на: " + DateTime.Now.ToLongDateString() + " на " + DateTime.Now.ToShortTimeString();
                    worksheetSummary["A1"].ColumnWidth = 25.0;
                    worksheetSummary["B1"].ColumnWidth = 15.0;
                    worksheetSummary["C1"].ColumnWidth = 15.0;
                    worksheetSummary["D1"].ColumnWidth = 15.0;
                    worksheetSummary["A3"].Text = "Итого";
                    worksheetSummary["A3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetSummary["A3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetSummary["A3"].WrapText = true;
                    range = worksheetSummary.Range["A3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheetSummary["B3"].Text = "Масса";
                    worksheetSummary["B3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetSummary["B3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetSummary["B3"].WrapText = true;
                    range = worksheetSummary.Range["B3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheetSummary["C3"].Text = "Стоимость";
                    worksheetSummary["C3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetSummary["C3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetSummary["C3"].WrapText = true;
                    range = worksheetSummary.Range["C3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    worksheetSummary["D3"].Text = "Цена за кг";
                    worksheetSummary["D3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetSummary["D3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetSummary["D3"].WrapText = true;
                    range = worksheetSummary.Range["D3"];
                    range.BorderAround(ExcelLineStyle.Thin);
                    string thisPeriod = "";
                    rowNum = 4;
                    foreach (var p in monthResults.OrderBy(a => a.Period))
                    {
                        if (p.Period != thisPeriod)
                        {
                            worksheetSummary.Range[rowNum, 1].Text = p.Period;
                            worksheetSummary.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            worksheetSummary.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 1].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 1];
                            range.BorderAround(ExcelLineStyle.Thin);
                            thisPeriod = p.Period;
                            rowNum++;
                            worksheetSummary.Range[rowNum, 1].Text = "Итого цена за кг " + p.Customer.Replace("\r\n", "") + ":";
                            worksheetSummary.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            worksheetSummary.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 1].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 1];
                            range.BorderAround(ExcelLineStyle.Thin);
                            worksheetSummary.Range[rowNum, 2].Number = Math.Round(p.Weight, 2);
                            worksheetSummary.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetSummary.Range[rowNum, 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 2].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 2];
                            range.BorderAround(ExcelLineStyle.Thin);
                            worksheetSummary.Range[rowNum, 3].Number = Math.Round(p.Cost, 2);
                            worksheetSummary.Range[rowNum, 3].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetSummary.Range[rowNum, 3].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 3].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 3];
                            range.BorderAround(ExcelLineStyle.Thin);
                            double cw = 0.0;
                            try
                            {
                                cw = p.Cost / p.Weight;
                            }
                            catch
                            {
                            }
                            worksheetSummary.Range[rowNum, 4].Number = Math.Round(cw, 2);
                            worksheetSummary.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetSummary.Range[rowNum, 4].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 4].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 4];
                            range.BorderAround(ExcelLineStyle.Thin);
                            rowNum++;
                        }
                        else
                        {
                            worksheetSummary.Range[rowNum, 1].Text = "Итого цена за кг " + p.Customer.Replace("\r\n", "") + ":";
                            worksheetSummary.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            worksheetSummary.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 1].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 1];
                            range.BorderAround(ExcelLineStyle.Thin);
                            worksheetSummary.Range[rowNum, 2].Number = Math.Round(p.Weight, 2);
                            worksheetSummary.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetSummary.Range[rowNum, 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 2].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 2];
                            range.BorderAround(ExcelLineStyle.Thin);
                            worksheetSummary.Range[rowNum, 3].Number = Math.Round(p.Cost, 2);
                            worksheetSummary.Range[rowNum, 3].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetSummary.Range[rowNum, 3].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 3].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 3];
                            range.BorderAround(ExcelLineStyle.Thin);
                            double cw = 0.0;
                            if (p.Weight != 0)
                                cw = p.Cost / p.Weight;
                            worksheetSummary.Range[rowNum, 4].Number = Math.Round(cw, 2);
                            worksheetSummary.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetSummary.Range[rowNum, 4].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetSummary.Range[rowNum, 4].WrapText = true;
                            range = worksheetSummary.Range[rowNum, 4];
                            range.BorderAround(ExcelLineStyle.Thin);
                            rowNum++;
                        }
                    }
                    rowNum += 4;
                    worksheetSummary.Range[rowNum, 1].Text = "[Масса] - масса на дату формирования отчета по 1с7 (поле [Вес]), [Стоимость] - фактическая стоимость заказа приведенная к USD на дату проведения документа 'Поступление ТМЦ' в 1с7";
                    worksheetSummary.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                    worksheetSummary.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetSummary.Range[rowNum, 1].WrapText = false;


                    IWorksheet worksheetPF = workbook.Worksheets[2];
                    worksheetPF.Name = "Группы изделий";
                    worksheetPF["A1"].Text = "Данные по состоянию на: " + DateTime.Now.ToLongDateString() + " на " + DateTime.Now.ToShortTimeString();
                    thisPeriod = "";
                    rowNum = 4;
                    worksheetPF.Range[rowNum, 1].Text = "Поставщик";
                    rowNum++;
                    int block = 0;
                    foreach (var type in typeList)
                    {
                        block += 4;
                        worksheetPF.Range[rowNum - 1, block - 1].Text = type.name;
                        worksheetPF.Range[rowNum, block - 2].Text = "Стоимость";
                        worksheetPF.Range[rowNum, block - 1].Text = "Цена за кг";
                        worksheetPF.Range[rowNum, block].Text = "Масса";
                    }
                    rowNum++;
                    int col = 0;
                    int controlRow = 0;
                    foreach (var p in monthResultTypes.OrderBy(a => a.Period))
                    {
                        if (p.Period != thisPeriod)
                        {
                            worksheetPF.Range[rowNum, 1].Text = p.Period;
                            worksheetPF.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                            worksheetPF.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetPF.Range[rowNum, 1].WrapText = true;
                            int l = 1;
                            foreach (var data in customerList)
                            {
                                worksheetPF.Range[rowNum + l, 1].Text = data.name;
                                worksheetPF.Range[rowNum + l, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                                worksheetPF.Range[rowNum + l, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                                worksheetPF.Range[rowNum + l, 1].WrapText = true;
                                l++;
                            }
                            rowNum++;
                            thisPeriod = p.Period;
                            col = 2;
                            controlRow = 0;
                        }
                        else
                        {
                            worksheetPF.Range[rowNum, col].Number = Math.Round(p.Weight);
                            worksheetPF.Range[rowNum, col].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetPF.Range[rowNum, col].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetPF.Range[rowNum, col].WrapText = true;
                            worksheetPF.Range[rowNum, col + 1].Number = Math.Round(p.Cost, 2);
                            worksheetPF.Range[rowNum, col + 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetPF.Range[rowNum, col + 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetPF.Range[rowNum, col + 1].WrapText = true;
                            double cw = 0.0;
                            if (p.Weight != 0)
                                cw = p.Cost / p.Weight;
                            worksheetPF.Range[rowNum, col + 2].Number = Math.Round(cw, 2);
                            worksheetPF.Range[rowNum, col + 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                            worksheetPF.Range[rowNum, col + 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                            worksheetPF.Range[rowNum, col + 2].WrapText = true;
                            rowNum++;
                            controlRow++;
                            if (controlRow >= customerList.Count())
                            {
                                rowNum -= customerList.Count();
                                col += 4;
                                controlRow = 0;
                            }
                        }
                    }


                    IWorksheet worksheetFullPos = workbook.Worksheets[3];
                    worksheetFullPos.Name = "Развернутый";
                    worksheetFullPos["A1"].Text = "Данные по состоянию на: " + DateTime.Now.ToLongDateString() + " на " + DateTime.Now.ToShortTimeString();
                    rowNum = 4;
                    worksheetFullPos["A1"].ColumnWidth = 10.0;
                    worksheetFullPos["B1"].ColumnWidth = 10.0;
                    worksheetFullPos["C1"].ColumnWidth = 40.0;
                    worksheetFullPos["D1"].ColumnWidth = 10.0;
                    worksheetFullPos["E1"].ColumnWidth = 40.0;
                    worksheetFullPos["F1"].ColumnWidth = 10.0;
                    worksheetFullPos["G1"].ColumnWidth = 10.0;
                    worksheetFullPos["H1"].ColumnWidth = 10.0;
                    worksheetFullPos["I1"].ColumnWidth = 10.0;
                    worksheetFullPos["J1"].ColumnWidth = 10.0;
                    worksheetFullPos["K1"].ColumnWidth = 10.0;
                    worksheetFullPos["L1"].ColumnWidth = 10.0;
                    worksheetFullPos["A3"].Text = "№ заказа";
                    worksheetFullPos["A3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["A3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["A3"].WrapText = true;
                    worksheetFullPos["B3"].Text = "№ план-заказа";
                    worksheetFullPos["B3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["B3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["B3"].WrapText = true;
                    worksheetFullPos["C3"].Text = "Полуфабрикат";
                    worksheetFullPos["C3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["C3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["C3"].WrapText = true;
                    worksheetFullPos["D3"].Text = "№ позиции";
                    worksheetFullPos["D3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["D3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["D3"].WrapText = true;
                    worksheetFullPos["E3"].Text = "Имя позиции";
                    worksheetFullPos["E3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["E3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["E3"].WrapText = true;
                    worksheetFullPos["F3"].Text = "№ поступления";
                    worksheetFullPos["F3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["F3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["F3"].WrapText = true;
                    worksheetFullPos["G3"].Text = "Дата поступления";
                    worksheetFullPos["G3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["G3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["G3"].WrapText = true;
                    worksheetFullPos["H3"].Text = "Вес по файлу";
                    worksheetFullPos["H3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["H3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["H3"].WrapText = true;
                    worksheetFullPos["I3"].Text = "Вес по 1с7";
                    worksheetFullPos["I3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["I3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["I3"].WrapText = true;
                    worksheetFullPos["J3"].Text = "Кол-во";
                    worksheetFullPos["J3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["J3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["J3"].WrapText = true;
                    worksheetFullPos["K3"].Text = "Суммарный вес по файлу";
                    worksheetFullPos["K3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["K3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["K3"].WrapText = true;
                    worksheetFullPos["L3"].Text = "Суммарный вес по 1с7";
                    worksheetFullPos["L3"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheetFullPos["L3"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheetFullPos["L3"].WrapText = true;
                    rowNum = 4;
                    foreach (var p in posList.OrderBy(a => a.id))
                    {
                        worksheetFullPos.Range[rowNum, 1].Text = p.CMOSPreOrder.CMOSOrderPreOrder.First().id_CMOSOrder.ToString();
                        worksheetFullPos.Range[rowNum, 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheetFullPos.Range[rowNum, 1].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 1].WrapText = true;
                        worksheetFullPos.Range[rowNum, 2].Text = p.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString();
                        worksheetFullPos.Range[rowNum, 2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheetFullPos.Range[rowNum, 2].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 2].WrapText = true;
                        worksheetFullPos.Range[rowNum, 3].Text = p.CMOSPreOrder.CMO_TypeProduct.name;
                        worksheetFullPos.Range[rowNum, 3].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheetFullPos.Range[rowNum, 3].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 3].WrapText = true;
                        worksheetFullPos.Range[rowNum, 4].Text = p.positionNum.ToString();
                        worksheetFullPos.Range[rowNum, 4].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheetFullPos.Range[rowNum, 4].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 4].WrapText = true;
                        worksheetFullPos.Range[rowNum, 5].Text = p.designation + " <" + p.index + "> " + p.name;
                        worksheetFullPos.Range[rowNum, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                        worksheetFullPos.Range[rowNum, 5].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 5].WrapText = true;
                        worksheetFullPos.Range[rowNum, 6].Text = p.CMOSPreOrder.CMOSOrderPreOrder.First().CMOSOrder.numberTN;
                        worksheetFullPos.Range[rowNum, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheetFullPos.Range[rowNum, 6].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 6].WrapText = true;
                        worksheetFullPos.Range[rowNum, 7].Text = p.CMOSPreOrder.CMOSOrderPreOrder.First().CMOSOrder.finDate.ToString().Substring(0, 10);
                        worksheetFullPos.Range[rowNum, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        worksheetFullPos.Range[rowNum, 7].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 7].WrapText = true;
                        worksheetFullPos.Range[rowNum, 8].Number = p.weight;
                        worksheetFullPos.Range[rowNum, 8].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheetFullPos.Range[rowNum, 8].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 8].WrapText = true;
                        worksheetFullPos.Range[rowNum, 9].Number = db.SKU.First(a => a.sku1 == p.sku).weight;
                        worksheetFullPos.Range[rowNum, 9].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheetFullPos.Range[rowNum, 9].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 9].WrapText = true;
                        worksheetFullPos.Range[rowNum, 10].Number = p.quantity;
                        worksheetFullPos.Range[rowNum, 10].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheetFullPos.Range[rowNum, 10].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 10].WrapText = true;
                        worksheetFullPos.Range[rowNum, 11].Number = p.weight * p.quantity;
                        worksheetFullPos.Range[rowNum, 11].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheetFullPos.Range[rowNum, 11].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 11].WrapText = true;
                        worksheetFullPos.Range[rowNum, 12].Number = db.SKU.First(a => a.sku1 == p.sku).weight * p.quantity;
                        worksheetFullPos.Range[rowNum, 12].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;
                        worksheetFullPos.Range[rowNum, 12].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                        worksheetFullPos.Range[rowNum, 12].WrapText = true;
                        rowNum++;
                    }


                    worksheetPF.Remove();
                    HttpResponse response = HttpContext.ApplicationInstance.Response;
                    try
                    {
                        workbook.SaveAs(DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + "Отчет.xlsx", HttpContext.ApplicationInstance.Response, ExcelDownloadType.Open);
                    }
                    catch
                    {
                    }
                }
            }
            return View();
        }

        public JsonResult GetControlWeightPreorder(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSPositionPreOrder
                        .AsNoTracking()
                        .Where(a => a.CMOSPreOrderId == id && a.note != "Входит в сб.")
                        .ToList();
                    double fileWeight = 0;
                    double rWeight = 0;
                    double sWeight = 0;
                    foreach (var pos in query)
                    {
                        fileWeight += pos.summaryWeight;
                        try
                        {
                            double skuR = db.SKU.First(a => a.indexMaterial == pos.index && a.designation == pos.designation).WeightR * pos.quantity;
                            double skuS = db.SKU.First(a => a.indexMaterial == pos.index && a.designation == pos.designation).weight * pos.quantity;
                            if (skuR == 0)
                                rWeight += pos.summaryWeight;
                            else
                                rWeight += skuR;

                            if (skuS == 0)
                                sWeight += pos.summaryWeight;
                            else
                                sWeight += skuS;
                        }
                        catch
                        {
                            rWeight += pos.summaryWeight;
                            sWeight += pos.summaryWeight;
                        }
                    }
                    List<WeightTable> listData = new List<WeightTable>();
                    listData.Add(new WeightTable { fileWeight = Math.Round(fileWeight, 2), rWeight = Math.Round(rWeight, 2), sWeight = Math.Round(sWeight, 2) });
                    var data = listData.Select(dataList => new
                    {
                        fileWeight,
                        rWeight,
                        sWeight
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetControlWeightPreorder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetControlWeightBackorder(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.CMOSPositionPreOrder
                        .AsNoTracking()
                        .Where(a => a.CMOSPreOrderId == id && a.note != "Входит в сб.")
                        .ToList();
                    double fileWeight = 0;
                    double rWeight = 0;
                    double sWeight = 0;
                    foreach (var pos in query)
                    {
                        fileWeight += pos.summaryWeight;
                        try
                        {
                            double skuR = db.SKU.First(a => a.indexMaterial == pos.index && a.designation == pos.designation).WeightR * pos.quantity;
                            double skuS = db.SKU.First(a => a.indexMaterial == pos.index && a.designation == pos.designation).weight * pos.quantity;
                            if (skuR == 0)
                                rWeight += pos.summaryWeight;
                            else
                                rWeight += skuR;

                            if (skuS == 0)
                                sWeight += pos.summaryWeight;
                            else
                                sWeight += skuS;
                        }
                        catch
                        {
                            rWeight += pos.summaryWeight;
                            sWeight += pos.summaryWeight;
                        }
                    }
                    List<WeightTable> listData = new List<WeightTable>();
                    listData.Add(new WeightTable { fileWeight = Math.Round(fileWeight, 2), rWeight = Math.Round(rWeight, 2), sWeight = Math.Round(sWeight, 2) });
                    var data = listData.Select(dataList => new
                    {
                        fileWeight,
                        rWeight,
                        sWeight
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetControlWeightBackorder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetCode(int code)
        {
            if (code < 10)
                return "0000" + code.ToString();
            else if (code < 100)
                return "000" + code.ToString();
            else if (code < 1000)
                return "00" + code.ToString();
            else if (code < 10000)
                return "0" + code.ToString();
            else
                return code.ToString();
        }

        private string GetCode(string code)
        {
            if (code.Length < 2)
                return "000" + code;
            else if (code.Length < 3)
                return "00" + code;
            else if (code.Length < 4)
                return "0" + code;
            else
                return code;
        }

        private int GetSKU(string designation, string index)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                try
                {
                    return db.SKU.First(a => a.designation == designation && a.indexMaterial == index).sku1;
                }
                catch
                {
                    return 0;
                }
            }
        }

        private string GetSKU(string name, string index, string designation)
        {
            return "";
        }

        private void CreateFileForArmis(string folder, CMOSOrder order)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var list = db.CMOSOrderPreOrder.AsNoTracking().Where(a => a.id_CMOSOrder == order.id).ToList();
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2013;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    worksheet["A1"].ColumnWidth = 45.0;
                    worksheet["B1"].ColumnWidth = 15.0;
                    worksheet["C1"].ColumnWidth = 15.0;
                    worksheet["D1"].ColumnWidth = 25.0;
                    worksheet["E1"].ColumnWidth = 15.0;
                    worksheet["F1"].ColumnWidth = 10.0;
                    worksheet["G1"].ColumnWidth = 10.0;
                    worksheet["H1"].ColumnWidth = 10.0;
                    worksheet["I1"].ColumnWidth = 10.0;
                    worksheet["A1"].Text = "Наименование ТМЦ";
                    worksheet["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["A1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["A1"].WrapText = true;
                    worksheet["B1"].Text = "Номер партии";
                    worksheet["B1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["B1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["B1"].WrapText = true;
                    worksheet["C1"].Text = "Прим.";
                    worksheet["C1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["C1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["C1"].WrapText = true;
                    worksheet["D1"].Text = "Склад";
                    worksheet["D1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["D1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["D1"].WrapText = true;
                    worksheet["E1"].Text = "Код EAN13";
                    worksheet["E1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["E1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["E1"].WrapText = true;
                    worksheet["F1"].Text = "Кол-во";
                    worksheet["F1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["F1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["F1"].WrapText = true;
                    worksheet["G1"].Text = "Цвет";
                    worksheet["G1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["G1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["G1"].WrapText = true;
                    worksheet["H1"].Text = "Обозначение";
                    worksheet["H1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["H1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["H1"].WrapText = true;
                    worksheet["I1"].Text = "Индекс";
                    worksheet["I1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet["I1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    worksheet["I1"].WrapText = true;
                    int rowNum = 2;
                    foreach (var rel in list)
                    {
                        var posList = db.CMOSPositionPreOrder.AsNoTracking().Where(a => a.CMOSPreOrderId == rel.id_CMOSPreOrder).ToList();
                        foreach (var pos in posList)
                        {
                            int code = GetSKU(pos.designation, pos.index);
                            worksheet.Range[rowNum, 1].Text = pos.designation + " <" + pos.index + "> " + pos.name;
                            worksheet.Range[rowNum, 2].Text = "парт:" + order.numberTN;
                            worksheet.Range[rowNum, 3].Text = "Заказ №: " + order.id.ToString();
                            worksheet.Range[rowNum, 4].Text = "Адр: (Склад №1 Пром9)";
                            worksheet.Range[rowNum, 5].Text = "0100" + GetCode(order.numberTN) + GetCode(code);
                            worksheet.Range[rowNum, 6].Text = pos.quantity.ToString();
                            worksheet.Range[rowNum, 7].Text = pos.color;
                            worksheet.Range[rowNum, 8].Text = pos.designation;
                            worksheet.Range[rowNum, 9].Text = pos.index;
                            rowNum++;
                        }
                    }
                    HttpResponse response = HttpContext.ApplicationInstance.Response;
                    try
                    {
                        workbook.SaveAs(folder + "Этикетки.xlsx");
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}