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

namespace Wiki.Areas.CMOS.Controllers
{
    public class CMOSSController : Controller
    {
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
            ViewBag.id_CMO_TypeProduct = new SelectList(db.CMO_TypeProduct.Where(d => d.active == true), "id", "name");
            ViewBag.id_CMOSPreorder = new SelectList(db.CMOSPreOrder.Where(d => d.CMOSOrderPreOrder.Count == 0), "id", "id_PZ_PlanZakaz");
            if (devisionUser == 7 || login == "myi@katek.by")
                ViewBag.userGroupId = 1;
            else if (login == "nrf@katek.by" || login == "vi@katek.by")
                ViewBag.userGroupId = 2;
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
                        .Where(a => a.dateTimeCreate > controlDate && a.remove == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.id,
                        positions = GetPositionsNamePreOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        state = GetStateOrder(dataList.id),
                        startDate = JsonConvert.SerializeObject(dataList.manufDate, shortSetting).Replace(@"""", ""),
                        finishDate = JsonConvert.SerializeObject(dataList.finDate, shortSetting).Replace(@"""", ""),
                        summryWeight = GetWeightOrder(dataList.id),
                        dataList.cost,
                        dataList.factCost,
                        dataList.folder,
                        tnNumber = dataList.numberTN
                    });
                    logger.Debug("CMOSSController / GetTableOrders");
                    return Json(new { data });
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
                        .Where(a => a.CMOSOrderPreOrder.Count == 0 && a.remove != true)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.id,
                        positionName = dataList.CMO_TypeProduct.name,
                        order = dataList.PZ_PlanZakaz.PlanZakaz,
                        dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        dataList.folder,
                        userCreate = dataList.AspNetUsers.CiliricalName,
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemovePreOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>"
                    });
                    logger.Debug("CMOSSController / GetTableNoPlaningPreOrder");
                    return Json(new { data });
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
                        .Where(a => a.manufDate == null && a.remove == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        dataList.id,
                        userCreate = dataList.AspNetUsers.CiliricalName,
                        dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        positionName = GetPositionsNameOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        dateGetMail = JsonConvert.SerializeObject(dataList.workDate, longSetting).Replace(@"""", ""),
                        dataList.folder,
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>"
                    });
                    logger.Debug("CMOSSController / GetTableNoPlaningOrder");
                    return Json(new { data });
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
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.numberTN == null)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        dataList.id,
                        userCreate = dataList.AspNetUsers.CiliricalName,
                        dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        positionName = GetPositionsNameOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        dateGetMail = JsonConvert.SerializeObject(dataList.manufDate, longSetting).Replace(@"""", ""),
                        dataList.folder,
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>"
                    });
                    logger.Debug("CMOSSController / GetTableTNOrder");
                    return Json(new { data });
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
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.finDate == null)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        dataList.id,
                        userCreate = dataList.AspNetUsers.CiliricalName,
                        dateCreate = JsonConvert.SerializeObject(dataList.dateTimeCreate, shortSetting).Replace(@"""", ""),
                        positionName = GetPositionsNameOrder(dataList.id),
                        customer = dataList.CMO_Company.name,
                        dateGetMail = JsonConvert.SerializeObject(dataList.workDate, longSetting).Replace(@"""", ""),
                        dataList.folder,
                        tn = dataList.numberTN,
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>"
                    });
                    logger.Debug("CMOSSController / GetTableNoClothingOrder");
                    return Json(new { data });
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

        public JsonResult AddPreOrder(int[] pzList, HttpPostedFileBase[] filePreorder, int typeProductId)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {

                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    foreach (var ord in pzList)
                    {
                        var preorder = new CMOSPreOrder
                        {
                            id_PZ_PlanZakaz = ord,
                            id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                            id_CMO_TypeProduct = typeProductId,
                            dateTimeCreate = DateTime.Now,
                            reOrder = false,
                            remove = false,
                            folder = "",
                            note = ""
                        };
                        db.CMOSPreOrder.Add(preorder);
                        db.SaveChanges();
                        preorder.folder = CreateFolderAndFileForPreOrder(preorder.id, filePreorder);
                        db.Entry(preorder).State = EntityState.Modified;
                        db.SaveChanges();
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

        public JsonResult AddBackorder(int[] pzListBackorder, int customerBackorder, HttpPostedFileBase[] fileBackorder)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    string note = "";
                    foreach (var ord in pzListBackorder)
                    {
                        note += db.PZ_PlanZakaz.Find(ord).PlanZakaz.ToString() + "; ";
                    }
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var backorder = new CMOSPreOrder
                    {
                        id_PZ_PlanZakaz = 3606,
                        id_AspNetUsersCreate = db.AspNetUsers.First(a => a.Email == login).Id,
                        id_CMO_TypeProduct = customerBackorder,
                        dateTimeCreate = DateTime.Now,
                        reOrder = true,
                        remove = false,
                        folder = "",
                        note = note + "- дозаказ"
                    };
                    db.CMOSPreOrder.Add(backorder);
                    db.SaveChanges();
                    backorder.folder = CreateFolderAndFileForBackorder(backorder.id, fileBackorder);
                    db.Entry(backorder).State = EntityState.Modified;
                    db.SaveChanges();
                    var order = new CMOSOrder
                    {
                        aspNetUsersCreateId = backorder.id_AspNetUsersCreate,
                        dateTimeCreate = DateTime.Now,
                        workDate = DateTime.Now.AddHours(6),
                        folder = backorder.folder,
                        cMO_CompanyId = customerBackorder,
                        cost = 0,
                        remove = false
                    };
                    db.CMOSOrder.Add(order);
                    db.SaveChanges();
                    var relations = new CMOSOrderPreOrder
                    {
                        id_CMOSOrder = order.id,
                        id_CMOSPreOrder = backorder.id
                    };
                    db.CMOSOrderPreOrder.Add(relations);
                    db.SaveChanges();
                    logger.Debug("CMOSSController / AddBackorder: " + " | " + login + " | " + backorder.id);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CMOSSController / AddBackorder: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        //
        //GetOrder
        //UpdateOrder

        public JsonResult AddOrder(int[] preordersList, int customerOrderId, DateTime workDate)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var order = new CMOSOrder
                    {
                        aspNetUsersCreateId = db.AspNetUsers.First(a => a.Email == login).Id,
                        dateTimeCreate = DateTime.Now,
                        workDate = workDate,
                        folder = "",
                        cMO_CompanyId = customerOrderId,
                        cost = 0,
                        remove = false
                    };
                    db.CMOSOrder.Add(order);
                    db.SaveChanges();
                    order.folder = CreateFolderAndFileForOrder(order.id, preordersList);
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
            }
            return directory;
        }

        private string CreateFolderAndFileForBackorder(int id, HttpPostedFileBase[] fileUploadArray)
        {
            string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\CMOS\\Backorder\\" + id.ToString() + "\\";
            Directory.CreateDirectory(directory);
            for (int i = 0; i < fileUploadArray.Length; i++)
            {
                string fileReplace = Path.GetFileName(fileUploadArray[i].FileName);
                fileReplace = ToSafeFileName(fileReplace);
                var fileName = string.Format("{0}\\{1}", directory, fileReplace);
                fileUploadArray[i].SaveAs(fileName);
            }
            return directory;
        }

        private string CreateFolderAndFileForOrder(int id, int[] preordersList)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string directory = "\\\\192.168.1.30\\m$\\_ЗАКАЗЫ\\CMOS\\Order\\" + id.ToString() + "\\";
                Directory.CreateDirectory(directory);
                List<string> fileArray = new List<string>();
                for (int i = 0; i < preordersList.Length; i++)
                {
                    foreach (var data in GetFileArray(db.CMOSPreOrder.Find(preordersList[i]).folder))
                    {
                        fileArray.Add(data);
                    }
                }
                foreach (var file in fileArray)
                {
                    string fileReplace = Path.GetFileName(file);
                    fileReplace = ToSafeFileName(fileReplace);
                    var fileName = string.Format("{0}\\{1}", directory, fileReplace);
                    FileInfo fileInf = new FileInfo(directory);
                    fileInf.CopyTo(fileName, true);
                }
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

        private double GetWeightPreOrder(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    return db.CMOSPositionPreOrder.Where(a => a.CMOSPreOrderId == id).Sum(a => a.summaryWeight);
                }
            }
            catch
            {
                return 0.0;
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
                        .Include(a => a.CMOSPreOrder.CMOSPositionPreOrder)
                        .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                        .Where(a => a.id_CMOSOrder == id)
                        .ToList();
                    foreach (var dataInList in positionsList)
                    {
                        data += dataInList.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + dataInList.CMOSPreOrder.CMO_TypeProduct.name + "\n";
                    }
                    return data;
                }
                catch
                {
                    return "";
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
                    if (data.workDate != null)
                        return "Ожидание сроков";
                    else if (data.manufDate != null)
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

        private double GetWeightOrder(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return db.CMOSPositionOrder.Where(a => a.id_CMOSOrder == id).Sum(a => a.summaryWeight);
                }
                catch
                {
                    return 0.0;
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
                        .Include(a => a.CMOSPreOrder.PZ_PlanZakaz)
                        .Include(a => a.CMOSPreOrder.CMO_TypeProduct)
                        .Where(a => a.id_CMOSOrder == id)
                        .ToList();
                    foreach (var pos in listPos)
                    {
                        data += pos.CMOSPreOrder.PZ_PlanZakaz.PlanZakaz.ToString() + " - " + pos.CMOSPreOrder.CMO_TypeProduct.name + "\n";
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