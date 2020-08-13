using Newtonsoft.Json;
using NLog;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
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
            //ViewBag.id_CMOSPreorder = new SelectList(db.CMOSPreOrder.Where(d => d.CMOSOrderPreOrder.Count == 0 && d.remove == false), "id", "id_PZ_PlanZakaz");
            if (devisionUser == 7 || login == "myi@katek.by" || login == "koag@katek.by")
                ViewBag.userGroupId = 1;
            else if (login == "nrf@katek.by" || login == "vi@katek.by" || login == "goa@katek.by")
                ViewBag.userGroupId = 2;
            else if (login == "bav@katek.by")
                ViewBag.userGroupId = 4;
            else
                ViewBag.userGroupId = 3;
            ViewBag.id_CMO_Company = new SelectList(db.CMO_Company.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            logger.Debug("CMOSSController/Index: " + login);
            return View();
        }

        public JsonResult GetTableOrders()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    DateTime controlDate = DateTime.Now.AddDays(-90);
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
                        dateGetMail = JsonConvert.SerializeObject(dataList.manufDate, shortDefaultSetting).Replace(@"""", ""),
                        positions = GetPositionsNamePreOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        state = GetStateOrder(dataList.id),
                        startDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                        finishDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                        dataList.cost,
                        dataList.factCost,
                        folder = @"<a href =" + dataList.folder + "> Папка </a>",
                        tnNumber = dataList.numberTN,
                        dateTN = dataList.dateTN,
                        summaryWeight = Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2),
                        posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        dataList.rate
                    });
                    logger.Debug("CMOSSController / GetTableOrders");
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetTableOrders: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
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
                    logger.Debug("CMOSSController / GetTableNoPlaningPreOrder");
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
                    logger.Debug("CMOSSController / GetTableNoPlaningOrder");
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
                        dateGetMail = JsonConvert.SerializeObject(dataList.manufDate, shortDefaultSetting).Replace(@"""", ""),
                        folder = @"<a href =" + dataList.folder + "> Папка </a>",
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        summaryWeight = Math.Round(dataList.CMOSPositionOrder.Sum(a => a.summaryWeight), 2),
                        posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>",
                        planingCost = dataList.cost,
                        dataList.rate,
                        curency = dataList.curency
                    });
                    logger.Debug("CMOSSController / GetTableTNOrder");
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
                        .Where(a => a.finDate == null && a.numberTN != null && a.manufDate != null && a.remove == false)
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
                        dateGetMail = JsonConvert.SerializeObject(dataList.manufDate, shortDefaultSetting).Replace(@"""", ""),
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
                    logger.Debug("CMOSSController / GetTableNoClothingOrder");
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetTableNoClothingOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
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
                    logger.Debug("CMOSSController / RemovePreOrder: " + id.ToString() + " | " + login);
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
                    logger.Debug("CMOSSController / RemoveOrder: " + id.ToString() + " | " + login);
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
                        new EmailCMOS(preorder, login, 0);
                    }
                    logger.Debug("CMOSSController / AddPreOrder: " + " | " + login);
                    return Json(1, JsonRequestBehavior.AllowGet);
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
                    logger.Debug("CMOSSController / AddBackorder: " + " | " + login);
                    return Json(1, JsonRequestBehavior.AllowGet);
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
                    logger.Debug("CMOSSController / AddOrder: " + " | " + login + " | " + order.id);
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
                        dataList.quantity8,
                        dataList.summaryWeight,
                        dataList.color,
                        dataList.coating,
                        dataList.note
                    });
                    logger.Debug("CMOSSController / GetPositionsPreorder");
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
                        quantity8 = 0,
                        dataList.summaryWeight,
                        dataList.color,
                        dataList.coating,
                        dataList.note,
                        sku = GetSKU(dataList.name, dataList.index, dataList.designation)
                    });
                    logger.Debug("CMOSSController / GetPositionsOrder");
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetPositionsOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
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
                        dateTN = dataList.dateTN,
                        cost = dataList.cost,
                        factCost = dataList.factCost,
                        planWeight = GetWeigthtOrder(dataList.id),
                        factWeightTN = dataList.weight,
                        curency = dataList.curency,
                        rate = dataList.rate
                    });
                    logger.Debug("CMOSSController / GetOrder");
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
                        foreach (var prd in prds.CMOSPreOrder.CMOSPositionPreOrder)
                        {
                            listPrd.Add(prd);
                        }
                    }
                    var data = listPrd.Select(dataList => new
                    {
                        name = dataList.designation + "<" + dataList.index + ">" + dataList.name,
                        code = dataList.sku,
                        weight = dataList.weight,
                        shortName = dataList.name,
                        norm = dataList.quantity,
                        rate = dataList.flow, 
                        loading = dataList.quantity - dataList.flow,
                        order = (dataList.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + dataList.CMOSPreOrder.CMO_TypeProduct.name).Replace("\r\n", ""),
                        color = "RAL" + dataList.color,
                        id = dataList.id
                    });
                    logger.Debug("CMOSSController / GetPositionsPreorderApi");
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / GetPositionsPreorderApi: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateOrder(int idOrder, int customerOrderId, DateTime? manufDate,
            DateTime? finDate, string numberTN, double? factCost, double? factWeightTN, double rate, 
            DateTime? dateTN)
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
                        if(order.manufDate != manufDate)
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
                        order.numberTN = numberTN;
                        order.dateTN = dateTN;
                        order.cost = Math.Round(order.rate * GetCurrency(order.dateTN.Value) * summaryWeight, 2);
                        order.factCost = factCost;
                        order.weight = factWeightTN.Value;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        new EmailCMOS(order, login, 7);
                    }
                    else if (finDate != null)
                    {
                        order.numberTN = numberTN;
                        order.dateTN = dateTN;
                        order.factCost = factCost;
                        order.weight = factWeightTN.Value;
                        order.finDate = finDate;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    logger.Debug("CMOSSController / UpdateOrder: " + " | " + login + " | " + idOrder);
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
                                indexMaterial = t.Cells[2].Value,
                                designation = t.Cells[3].Value
                            };
                            skuUpList[i] = skuUp;
                            i++;
                        }
                        var dbres = db.SKU.ToArray();
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
                                    sku1 = sku.sku1
                                };
                                db.SKU.Add(skuAdd);
                            }
                            else
                            {
                                skuIn = dbres.First(a => a.sku1 == sku.sku1);
                                if (skuIn.designation != sku.designation || skuIn.name != sku.name || skuIn.indexMaterial != sku.indexMaterial)
                                {
                                    SKU update = db.SKU.First(a => a.sku1 == sku.sku1);
                                    update.designation = sku.designation;
                                    update.name = sku.name;
                                    update.indexMaterial = sku.indexMaterial;
                                    db.Entry(update).State = EntityState.Modified;
                                }
                            }
                        }
                        db.SaveChanges();
                    }
                    logger.Debug("CMOSSController / LoadingMaterialsC: " + " | " + login);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / LoadingMaterialsC: " + " | " + ex + " | " + login);
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
                    logger.Debug("GetBujetList / GetPositionsOrder");
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if(id != 0)
                    logger.Error("GetBujetList / GetPositionsOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
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


        private string GetSKU(string name, string index, string designation)
        {
            return "";
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
                    var pos = db.CMOSPositionPreOrder.AsNoTracking().Where(a => a.CMOSPreOrderId == prdId.id_CMOSPreOrder).ToList();
                    summaryWeight += pos.Sum(a => a.summaryWeight);
                    foreach (var p in pos)
                    {
                        weightGet += p.flow * p.weight;
                    }
                }
                return Math.Round((weightGet / summaryWeight * 100), 2).ToString();
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