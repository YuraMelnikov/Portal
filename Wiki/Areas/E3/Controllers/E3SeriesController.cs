using Newtonsoft.Json;
using NLog;
using System;
using System.Linq;
using System.Web.Mvc;
using Wiki.Areas.E3.Models;

namespace Wiki.Areas.E3.Controllers
{
    public class E3SeriesController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };

        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            string login = HttpContext.User.Identity.Name;
            int devisionUser = GetDevision(login);
            if (login == "myi@katek.by" || login == "dkv@katek.by")
                ViewBag.userGroupId = 1;
            else if (devisionUser == 16)
                ViewBag.userGroupId = 2;
            else
                ViewBag.userGroupId = 0;
            return View();
        }

        public JsonResult GetTableModelData()
        {
            try
            {
                using (E3_Components_Symbols_EditEntities db = new E3_Components_Symbols_EditEntities())
                {
                    DateTime controlDate = DateTime.Now.AddDays(-90);
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.ModelData.AsNoTracking().ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return PostModel('" + dataList.ID + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-alert" + '\u0022' + "></span></a></td>",
                        dataList.ID,
                        dataList.Entry,
                        dataList.Type,
                        dataList.Class,
                        dataList.Description,
                        LASTUPDT = JsonConvert.SerializeObject(dataList.LASTUPDT, shortDefaultSetting).Replace(@"""", ""),
                        DATEMAIN = JsonConvert.SerializeObject(dataList.DATEMAIN, shortDefaultSetting).Replace(@"""", ""),
                        removeLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveModel('" + dataList.ID + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        note = GetNoteModelData(dataList)
                    });
                    logger.Debug("E3SeriesController / GetTableModelData");
                    return Json(new { data });
                }
            }
            catch (Exception ex)
            {
                logger.Error("E3SeriesController / GetTableModelData: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTableComponentData()
        {
            try
            {
                using (E3_Components_Symbols_EditEntities db = new E3_Components_Symbols_EditEntities())
                {
                    DateTime controlDate = DateTime.Now.AddDays(-90);
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.ComponentData.AsNoTracking().ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return PostComponent('" + dataList.ID + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-alert" + '\u0022' + "></span></a></td>",
                        dataList.ID,
                        dataList.ENTRY,
                        dataList.VERSION,
                        dataList.DeviceLetterCode,
                        dataList.Class,
                        dataList.ENTRYTYP,
                        dataList.Description,
                        dataList.LPNTR,
                        dataList.VSTATUS,
                        dataList.VersionText,
                        LASTUPDT = JsonConvert.SerializeObject(dataList.LASTUPDT, shortDefaultSetting).Replace(@"""", ""),
                        DATEMAIN = JsonConvert.SerializeObject(dataList.DATEMAIN, shortDefaultSetting).Replace(@"""", ""),
                        dataList.MPNTR,
                        dataList.Flags,
                        dataList.ArticleNumber,
                        dataList.Supplier,
                        dataList.SPNTR,
                        dataList.Class_main,
                        dataList.katek_1C,
                        dataList.katek_type_rkd,
                        dataList.katek_name_rkd,
                        removeLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveComponent('" + dataList.ID + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        note = GetNoteComponentData(dataList)
                    });
                    logger.Debug("E3SeriesController / GetTableComponentData");
                    return Json(new { data });
                }
            }
            catch (Exception ex)
            {
                logger.Error("E3SeriesController / GetTableComponentData: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTableSymbolData()
        {
            try
            {
                using (E3_Components_Symbols_EditEntities db = new E3_Components_Symbols_EditEntities())
                {
                    DateTime controlDate = DateTime.Now.AddDays(-90);
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.SymbolData.AsNoTracking().ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return PostSymbol('" + dataList.ID + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-alert" + '\u0022' + "></span></a></td>",
                        dataList.ID,
                        dataList.ENTRY,
                        dataList.VERSION,
                        dataList.Norm,
                        dataList.LSHDES,
                        dataList.LSHTYP,
                        dataList.ENTRYTYP,
                        dataList.LPNTR,
                        dataList.VSTATUS,
                        dataList.VersionText,
                        LASTUPDT = JsonConvert.SerializeObject(dataList.LASTUPDT, shortDefaultSetting).Replace(@"""", ""),
                        DATEMAIN = JsonConvert.SerializeObject(dataList.DATEMAIN, shortDefaultSetting).Replace(@"""", ""),
                        dataList.Class,
                        dataList.Description,
                        dataList.SYMINDEX,
                        dataList.FLAGS2,
                        dataList.FLAGS,
                        removeLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveSymbol('" + dataList.ID + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        note = GetNoteSymbolData(dataList)
                    });
                    logger.Debug("E3SeriesController / GetTableSymbolData");
                    return Json(new { data });
                }
            }
            catch (Exception ex)
            {
                logger.Error("E3SeriesController / GetTableSymbolData: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PostModel(string ID)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (E3_Components_Symbols_EditEntities db = new E3_Components_Symbols_EditEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var model = db.ModelAttribute.First(a => a.ID == ID);
                    new EmailE3(login, "Просьба срочно добавить/изменить модель: " + model.Entry);
                    logger.Debug("E3SeriesController / PostModel");
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("E3SeriesController / PostModel: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PostComponent(string ID)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (E3_Components_Symbols_EditEntities db = new E3_Components_Symbols_EditEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var model = db.ComponentData.First(a => a.ID == ID);
                    new EmailE3(login, "Просьба срочно добавить/изменить компонент: " + model.ENTRY);
                    logger.Debug("E3SeriesController / PostComponent");
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("E3SeriesController / PostComponent: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PostSymbol(string ID)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (E3_Components_Symbols_EditEntities db = new E3_Components_Symbols_EditEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var model = db.SymbolData.First(a => a.ID == ID);
                    new EmailE3(login, "Просьба срочно добавить/изменить символ: " + model.ENTRY);
                    logger.Debug("E3SeriesController / PostComponent");
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("E3SeriesController / PostComponent: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
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

        private string GetNoteModelData(ModelData newModel)
        {
            using (E3_Components_SymbolsEntities db = new E3_Components_SymbolsEntities())
            {
                string note = "Изменено: ";
                try
                {
                    ModelData before = db.ModelData.First(a => a.Entry == newModel.Entry);
                    if (before.Type != newModel.Type)
                        note += "Type: " + before.Type + " | " + newModel.Type + "; ";
                    if (before.Class != newModel.Class)
                        note += "Class: " + before.Class + " | " + newModel.Class + "; ";
                    if (before.Description != newModel.Description)
                        note += "Description: " + before.Description + " | " + newModel.Description + "; ";
                    return note;
                }
                catch
                {
                    return "Новая модель";
                }
            }
        }

        private string GetNoteComponentData(ComponentData newModel)
        {
            using (E3_Components_SymbolsEntities db = new E3_Components_SymbolsEntities())
            {
                string note = "Изменено: ";
                try
                {
                    ComponentData before = db.ComponentData.First(a => a.ENTRY == newModel.ENTRY);
                    if (before.VERSION != newModel.VERSION)
                        note += "VERSION: " + before.VERSION + " | " + newModel.VERSION + "; ";
                    if (before.VERSION != newModel.VERSION)
                        note += "VERSION: " + before.VERSION + " | " + newModel.VERSION + "; ";
                    if (before.DeviceLetterCode != newModel.DeviceLetterCode)
                        note += "DeviceLetterCode: " + before.DeviceLetterCode + " | " + newModel.DeviceLetterCode + "; ";
                    if (before.Class != newModel.Class)
                        note += "Class: " + before.Class + " | " + newModel.Class + "; ";
                    if (before.ENTRYTYP != newModel.ENTRYTYP)
                        note += "ENTRYTYP: " + before.ENTRYTYP + " | " + newModel.ENTRYTYP + "; ";
                    if (before.Description != newModel.Description)
                        note += "Description: " + before.Description + " | " + newModel.Description + "; ";
                    if (before.LPNTR != newModel.LPNTR)
                        note += "LPNTR: " + before.LPNTR + " | " + newModel.LPNTR + "; ";
                    if (before.VSTATUS != newModel.VSTATUS)
                        note += "VSTATUS: " + before.VSTATUS + " | " + newModel.VSTATUS + "; ";
                    if (before.VersionText != newModel.VersionText)
                        note += "VersionText: " + before.VersionText + " | " + newModel.VersionText + "; ";
                    if (before.MPNTR != newModel.MPNTR)
                        note += "MPNTR: " + before.MPNTR + " | " + newModel.MPNTR + "; ";
                    if (before.Flags != newModel.Flags)
                        note += "Flags: " + before.Flags + " | " + newModel.Flags + "; ";
                    if (before.ArticleNumber != newModel.ArticleNumber)
                        note += "ArticleNumber: " + before.ArticleNumber + " | " + newModel.ArticleNumber + "; ";
                    if (before.Supplier != newModel.Supplier)
                        note += "Supplier: " + before.Supplier + " | " + newModel.Supplier + "; ";
                    if (before.SPNTR != newModel.SPNTR)
                        note += "SPNTR: " + before.SPNTR + " | " + newModel.SPNTR + "; ";
                    if (before.Class_main != newModel.Class_main)
                        note += "Class_main: " + before.Class_main + " | " + newModel.Class_main + "; ";
                    if (before.katek_1C != newModel.katek_1C)
                        note += "katek_1C: " + before.katek_1C + " | " + newModel.katek_1C + "; ";
                    if (before.katek_type_rkd != newModel.katek_type_rkd)
                        note += "katek_type_rkd: " + before.katek_type_rkd + " | " + newModel.katek_type_rkd + "; ";
                    if (before.katek_name_rkd != newModel.katek_name_rkd)
                        note += "katek_name_rkd: " + before.katek_name_rkd + " | " + newModel.katek_name_rkd + "; ";
                    return note;
                }
                catch
                {
                    return "Новый компонент";
                }
            }
        }

        private string GetNoteSymbolData(SymbolData newModel)
        {
            using (E3_Components_SymbolsEntities db = new E3_Components_SymbolsEntities())
            {
                string note = "Изменено: ";
                try
                {
                    SymbolData before = db.SymbolData.First(a => a.ENTRY == newModel.ENTRY);
                    if (before.VERSION != newModel.VERSION)
                        note += "VERSION: " + before.VERSION + " | " + newModel.VERSION + "; ";
                    if (before.Norm != newModel.Norm)
                        note += "Norm: " + before.Norm + " | " + newModel.Norm + "; ";
                    if (before.LSHDES != newModel.LSHDES)
                        note += "LSHDES: " + before.LSHDES + " | " + newModel.LSHDES + "; ";
                    if (before.LSHTYP != newModel.LSHTYP)
                        note += "LSHTYP: " + before.LSHTYP + " | " + newModel.LSHTYP + "; ";
                    if (before.ENTRYTYP != newModel.ENTRYTYP)
                        note += "ENTRYTYP: " + before.ENTRYTYP + " | " + newModel.ENTRYTYP + "; ";
                    if (before.LPNTR != newModel.LPNTR)
                        note += "LPNTR: " + before.LPNTR + " | " + newModel.LPNTR + "; ";
                    if (before.VersionText != newModel.VersionText)
                        note += "VersionText: " + before.VersionText + " | " + newModel.VersionText + "; ";
                    if (before.Class != newModel.Class)
                        note += "Class: " + before.Class + " | " + newModel.Class + "; ";
                    if (before.Description != newModel.Description)
                        note += "Description: " + before.Description + " | " + newModel.Description + "; ";
                    if (before.SYMINDEX != newModel.SYMINDEX)
                        note += "SYMINDEX: " + before.SYMINDEX + " | " + newModel.SYMINDEX + "; ";
                    if (before.FLAGS2 != newModel.FLAGS2)
                        note += "FLAGS2: " + before.FLAGS2 + " | " + newModel.FLAGS2 + "; ";
                    if (before.FLAGS != newModel.FLAGS)
                        note += "FLAGS: " + before.FLAGS + " | " + newModel.FLAGS + "; ";
                    return note;
                }
                catch
                {
                    return "Новый символ";
                }
            }
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
    }
}