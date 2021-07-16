using NLog;
using Syncfusion.XlsIO;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wiki.Areas.Illiquid.Controllers
{
    public class IlliqController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };

        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            ViewBag.Devision = new SelectList(db.Devision.OrderBy(a => a.name), "id", "name");
            ViewBag.Type = new SelectList(db.IlliquidType.Where(d => d.IsActive == true).OrderBy(d => d.Name), "id", "Name");
            return View();
        }

        public JsonResult LoadingStock()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                HttpPostedFileBase fiel = Request.Files[0];
                string fileReplace = Path.GetFileName(fiel.FileName);
                var fileName = Path.Combine(Server.MapPath("~/Areas/Illiquid/ToStock"), fileReplace);
                fiel.SaveAs(fileName);
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    FileStream inputStream = fiel.InputStream as FileStream;
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.OpenReadOnly(fileName);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    int rowsQuantity = worksheet.Rows.Length;
                    var list = worksheet.Rows.ToArray();
                    foreach (var t in list)
                    {
                        int fnd = Convert.ToInt32(t.Cells[0].Value);
                        int idSku = db.SKU.First(a => a.sku1 == fnd).id;
                        IlliquidStockState state = new IlliquidStockState
                        {
                            Date = t.Cells[5].DateTime,
                            IlliquidQue = (float)t.Cells[1].Number,
                            IlliquidSum = (float)t.Cells[2].Number,
                            SurplusQue = (float)t.Cells[3].Number,
                            SurplusSum = (float)t.Cells[4].Number,
                            SKUId = idSku
                        };
                        db.IlliquidStockState.Add(state);
                        db.SaveChanges();
                    }
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetIlliquidPeriod(DateTime startDate, DateTime finishDate)
        {
            string login = HttpContext.User.Identity.Name;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var listBefore = db.IlliquidStockState.AsNoTracking().Where(a => a.Date == startDate).ToList();
                var listNext = db.IlliquidStockState.AsNoTracking().Where(a => a.Date == finishDate).ToList();
                List<Wiki.Illiquid> list = new List<Wiki.Illiquid>();
                foreach (var data in listNext)
                {
                    float queBefore = listBefore.Where(a => a.SKUId == data.SKUId).Sum(a => a.SurplusQue);
                    if (data.SurplusQue - queBefore >= 1.0)
                    {
                        Wiki.Illiquid illiquid = new Wiki.Illiquid
                        {
                            idIlliquidStockStateNext = data.Id,
                            quantityNext = data.SurplusQue,
                            quantityBefore = queBefore,
                            idIlliquidStockStateBefore = null,
                            Note = ""
                        };
                        if (queBefore > 0)
                            illiquid.idIlliquidStockStateBefore = listBefore.First(a => a.SKUId == data.SKUId).Id;
                        db.Illiquid.Add(illiquid);
                        db.SaveChanges();
                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTableNotWorking()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Illiquid
                        .AsNoTracking()
                        .Include(a => a.IlliquidStockState1.SKU)
                        .Include(a => a.IlliquidStockState)
                        .Include(a => a.IlliquidStockState1)
                        .Where(a => a.IsAnalisis == false)
                        .ToList();

                    var data = query.Select(dataList => new
                    {
                        id = dataList.id,
                        code = dataList.IlliquidStockState1.SKU.sku1,
                        max = dataList.IlliquidStockState1.SKU.Max,
                        materialName = dataList.IlliquidStockState1.SKU.designation + " |" + dataList.IlliquidStockState1.SKU.indexMaterial + "| " + dataList.IlliquidStockState1.SKU.name,
                        queBefore = Math.Round(dataList.quantityBefore, 2),
                        queNext = Math.Round(dataList.quantityNext, 2),
                        que = Math.Round(dataList.quantityNext - dataList.quantityBefore, 2),
                        sum = dataList.IlliquidStockState1.SurplusSum - GetNullFloat(dataList.IlliquidStockState)
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("IlliqController / GetTableNotWorking: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTableWorking()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.IlliquidResult
                        .AsNoTracking()
                        .Include(a => a.Illiquid.IlliquidStockState)
                        .Include(a => a.Illiquid.IlliquidStockState1.SKU)
                        .ToList();

                    var data = query.Select(dataList => new
                    {
                        id = dataList.Id,
                        idNlk = dataList.IlliquidId,
                        date = JsonConvert.SerializeObject(dataList.Illiquid.IlliquidStockState1.Date, shortSetting).Replace(@"""", ""),
                        code = dataList.Illiquid.IlliquidStockState1.SKU.sku1,
                        max = dataList.Illiquid.IlliquidStockState1.SKU.Max,
                        materialName = dataList.Illiquid.IlliquidStockState1.SKU.designation + " |" + dataList.Illiquid.IlliquidStockState1.SKU.indexMaterial + "| " + dataList.Illiquid.IlliquidStockState1.SKU.name,
                        que = Math.Round(dataList.Quentity, 2),
                        sum = Math.Round(dataList.Sum, 2),
                        cause = dataList.Cause,
                        note = dataList.Note,
                        changeNorms = GetChangeNormForTable(dataList.IlliquidId),
                        orders = GetOrdersForTable(dataList.IlliquidId),
                        added = GetAddedForTable(dataList.IlliquidId),
                        replacement = GetReplacementForTable(dataList.IlliquidId),
                        sn = GetSNForTable(dataList.IlliquidId),
                        addedX = GetAddedXForTable(dataList.IlliquidId),
                        vozvrat = GetVozvratForTable(dataList.IlliquidId),
                        vipusk = GetVipuskForTable(dataList.IlliquidId),
                        queBefore = Math.Round(dataList.Illiquid.quantityBefore, 2),
                        queNext = Math.Round(dataList.Illiquid.quantityNext, 2),
                        otkl = Math.Round(dataList.Illiquid.quantityNext - dataList.Illiquid.quantityBefore, 2),
                        vozvratMOL = GetVozvratMOLForTable(dataList.IlliquidId)
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("IlliqController / GetTableNotWorking: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetIlliquid(int id)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.Illiquid
                        .AsNoTracking()
                        .Where(a => a.id == id)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.id,
                        noteIlliquid = dataList.Note,
                        idIlliquid = dataList.id
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Illiq / GetIlliquid: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateIlliquid(int idIlliquid, string noteIlliquid)
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var data = db.Illiquid.Find(idIlliquid);
                    data.Note = noteIlliquid;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Illiq / UpdateIlliquid: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        string GetAddedXForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var listX = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.IlliquidId == id && a.ActionId == 7)
                    .ToList();
                int count = listX.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += listX[i].Quentity.ToString() + " | " + listX[i].Date.ToShortDateString();
                    else
                        data += listX[i].Quentity.ToString() + " | " + listX[i].Date.ToShortDateString() + "</br>";
                }
                if (data.Length == 0)
                    data += "-";
                return data;
            }
        }

        string GetSNForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var listSN = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.IlliquidId == id && a.ActionId == 6)
                    .ToList();
                int count = listSN.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += listSN[i].Quentity.ToString() + " | " + listSN[i].Date.ToShortDateString();
                    else
                        data += listSN[i].Quentity.ToString() + " | " + listSN[i].Date.ToShortDateString() + "</br>";
                }

                if (data.Length == 0)
                    data += "-";
                return data;
            }
        }

        string GetVozvratMOLForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var listSN = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.IlliquidId == id && a.ActionId == 11)
                    .ToList();
                int count = listSN.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += listSN[i].Quentity.ToString() + " | " + listSN[i].Date.ToShortDateString();
                    else
                        data += listSN[i].Quentity.ToString() + " | " + listSN[i].Date.ToShortDateString() + "</br>";
                }

                if (data.Length == 0)
                    data += "-";
                return data;
            }
        }

        float GetNullFloat(IlliquidStockState state)
        {
            if (state == null)
                return 0.0f;
            else
                return state.SurplusSum;
        }

        string GetReplacementForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.IlliquidId == id && a.ActionId == 5)
                    .ToList();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString();
                    else
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString() + "</br>";
                }
                if (data.Length == 0)
                    data += "-";
                return data;
            }
        }

        string GetVozvratForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.IlliquidId == id && a.ActionId == 8)
                    .OrderBy(a => a.Date)
                    .ToList();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString();
                    else
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString() + "</br>";
                }
                return data;
            }
        }

        string GetVipuskForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.IlliquidId == id && a.ActionId == 10)
                    .OrderBy(a => a.Date)
                    .ToList();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString();
                    else
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString() + "</br>";
                }
                return data;
            }
        }

        string GetAddedForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.IlliquidId == id && a.ActionId == 4)
                    .OrderBy(a => a.Date)
                    .ToList();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString();
                    else
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString() + "</br>";
                }
                return data;
            }
        }

        string GetOrdersForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.IlliquidId == id && a.ActionId == 3)
                    .OrderBy(a => a.Date)
                    .ToList();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString();
                    else
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString() + "</br>";
                }
                return data;
            }
        }

        string GetChangeNormForTable(int idIlliquid)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
                    .AsNoTracking()
                    .Where(a => a.ActionId == 1 || a.ActionId == 9)
                    .Where(a => a.IlliquidId == idIlliquid)
                    .OrderBy(a => a.Date)
                    .ToList();
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (i + 1 == count)
                        data += list[i].Note + " | " + list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString();
                    else
                        data += list[i].Note + " | " + list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString() + "</br>";
                }
                return data;
            }
        }

        DateTime GetStartDateAction(int id, DateTime finish)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return db.Illiquid
                        .AsNoTracking()
                        .Include(a => a.IlliquidStockState)
                        .First(a => a.id == id).IlliquidStockState.Date;
                }
                catch
                {
                    return finish.AddDays(-16);
                }
            }
        }

        [HttpPost]
        public JsonResult AnalisysIlliquid()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Illiquid
                    .AsNoTracking()
                    .Include(a => a.IlliquidStockState)
                    .Include(a => a.IlliquidStockState1)
                    .Include(a => a.IlliquidStockState1.SKU)
                    .Where(a => a.IsAnalisis == false)
                    .ToList();
                foreach (var data in query)
                {
                    float quentity = data.quantityNext - data.quantityBefore;
                    float cost = data.IlliquidStockState1.SurplusSum / data.IlliquidStockState1.SurplusQue;
                    DateTime finish = data.IlliquidStockState1.Date;
                    DateTime start = GetStartDateAction(data.id, finish).AddDays(-1);
                    DateTime controlDateForLastAdded = DateTime.Now.AddDays(-185);
                    SaveActionsAddStock(data, quentity);
                    SaveActionsOrders(data, quentity);
                    SaveActionsSN(data, quentity);
                    SaveActionsVozvrat(data, quentity, start);
                    SaveActionsVozvratMOL(data, quentity, start);
                    SaveActionsVipusk(data, quentity, start);
                    SaveActionsReplacement(data, quentity, start);
                    DateTime minDateForChange = finish.AddDays(-16);
                    try
                    {
                        DateTime minDateForChangeT = db.IlliquidAction
                            .Where(a => a.IlliquidId == data.id)
                            .Where(a => a.ActionId == 3 || a.ActionId == 4 || a.ActionId == 7)
                            .Min(a => a.Date);
                        minDateForChange = minDateForChangeT;
                    }
                    catch
                    {
                    }
                    SaveActionsChangeNorms(data, minDateForChange, quentity);
                    var listChangeInTheNorm = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 1 && a.IlliquidId == data.id)
                        .Where(a => a.Date > minDateForChange && a.Date <= finish)
                        .OrderBy(a => a.Date)
                        .ToList();
                    var listOrders = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 3 && a.IlliquidId == data.id)
                        .Where(a => a.Date <= finish)
                        .OrderByDescending(a => a.Date)
                        .ToList();
                    var listAddStock = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 4 && a.IlliquidId == data.id)
                        //.Where(a => a.Date >= start && a.Date <= finish)
                        .Where(a => a.Date <= finish)
                        .OrderByDescending(a => a.Date)
                        .ToList();
                    var listReplacement = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 5 && a.IlliquidId == data.id)
                        .Where(a => a.Date >= start && a.Date <= finish)
                        .OrderByDescending(a => a.Date)
                        .ToList();
                    var listSN = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 6 && a.IlliquidId == data.id)
                        .Where(a => a.Date >= minDateForChange && a.Date <= finish)
                        .OrderByDescending(a => a.Date)
                        .ToList();
                    var listAddStockX = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 7 && a.IlliquidId == data.id)
                        .Where(a => a.Date <= finish)
                        .OrderByDescending(a => a.Date)
                        .ToList();
                    var listVozvrat = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 8 && a.IlliquidId == data.id)
                        .Where(a => a.Date >= start && a.Date <= finish)
                        .OrderByDescending(a => a.Date)
                        .ToList();
                    var listVozvratMOL = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 11 && a.IlliquidId == data.id)
                        .Where(a => a.Date >= start && a.Date <= finish)
                        .OrderByDescending(a => a.Date)
                        .ToList();
                    var listVipusk = db.IlliquidAction
                        .AsNoTracking()
                        .Where(a => a.ActionId == 10 && a.IlliquidId == data.id)
                        .Where(a => a.Date >= start && a.Date <= finish)
                        .OrderByDescending(a => a.Date)
                        .ToList();
                    bool no = false;
                    while (quentity > 0)
                    {
                        IlliquidResult result = new IlliquidResult();
                        result.IlliquidId = data.id;
                        if (no == false && data.IlliquidStockState1.SKU.Max > 0)
                        {
                            float max = (float)data.IlliquidStockState1.SKU.Max;
                            if (data.quantityNext <= max)
                            {
                                if (max > quentity)
                                    max = quentity;
                                result.DevisionAuto = 24;
                                result.Cause = "Неснижаемый остаток";
                                result.Quentity = max;
                                result.Sum = cost * max;
                                result.Note = data.Note;
                                db.IlliquidResult.Add(result);
                                db.SaveChanges();
                                quentity -= result.Quentity;
                            }
                            else
                            {
                                max = quentity - (data.quantityNext - max);
                                if (max < quentity && max > 0)
                                {
                                    result.DevisionAuto = 24;
                                    result.Cause = "Неснижаемый остаток";
                                    result.Quentity = max;
                                    result.Sum = cost * max;
                                    result.Note = data.Note;
                                    db.IlliquidResult.Add(result);
                                    db.SaveChanges();
                                    quentity -= result.Quentity;
                                }
                            }
                            no = true;
                        }
                        else if (listVozvratMOL.Sum(a => a.Quentity) > 0)
                        {
                            IlliquidAction action = listVozvratMOL.First();
                            if (action.Quentity > quentity)
                                action.Quentity = quentity;
                            result.DevisionAuto = 24;
                            result.Cause = "Возврат МОЛом";
                            result.IlliquidTypeId = 11;
                            result.Quentity = action.Quentity;
                            result.Sum = cost * action.Quentity;
                            result.Note = data.Note;
                            db.IlliquidResult.Add(result);
                            db.SaveChanges();
                            listVozvratMOL.Remove(action);
                            quentity -= result.Quentity;
                        }
                        else if (listAddStockX.Sum(a => a.Quentity) > 0)
                        {
                            IlliquidAction action = listAddStockX.First();
                            if (action.Date >= start)
                            {
                                if (action.Quentity > quentity)
                                    action.Quentity = quentity;
                                result.DevisionAuto = 24;
                                result.Cause = "Поступление от Х";
                                result.IlliquidTypeId = 11;
                                result.Quentity = action.Quentity;
                                result.Sum = cost * action.Quentity;
                                result.Note = data.Note;
                                db.IlliquidResult.Add(result);
                                db.SaveChanges();
                                listAddStockX.Remove(action);
                                quentity -= result.Quentity;
                            }
                            else
                            {
                                if (action.Quentity > quentity)
                                    action.Quentity = quentity;
                                result.DevisionAuto = 24;
                                result.Cause = "Преждние периоды";
                                result.Quentity = action.Quentity;
                                result.Sum = cost * result.Quentity;
                                result.Note = data.Note;
                                db.IlliquidResult.Add(result);
                                db.SaveChanges();
                                listAddStockX.Remove(action);
                                quentity -= result.Quentity;
                            }
                        }
                        else if (listAddStock.Max(a => a.Date) < controlDateForLastAdded)
                        {
                            IlliquidAction action = listAddStock.First();
                            if (action.Quentity > quentity)
                                action.Quentity = quentity;
                            result.DevisionAuto = 24;
                            result.Cause = "Преждние периоды";
                            result.Quentity = action.Quentity;
                            result.Sum = cost * result.Quentity;
                            result.Note = data.Note;
                            db.IlliquidResult.Add(result);
                            db.SaveChanges();
                            quentity -= result.Quentity;
                            listAddStock.Remove(action);
                        }
                        else if (listReplacement.Sum(a => a.Quentity) > 0)
                        {
                            IlliquidAction action = listReplacement.First();
                            if (action.Quentity > quentity)
                                action.Quentity = quentity;
                            result.DevisionAuto = 24;
                            result.Cause = "Замена ТМЦ";
                            result.IlliquidTypeId = 6;
                            result.Quentity = action.Quentity;
                            result.Sum = cost * action.Quentity;
                            result.Note = data.Note;
                            db.IlliquidResult.Add(result);
                            db.SaveChanges();
                            listReplacement.Remove(action);
                            quentity -= result.Quentity;
                        }
                        else if (listVipusk.Sum(a => a.Quentity) > 0)
                        {
                            IlliquidAction action = listVipusk.First();
                            if (action.Quentity > quentity)
                                action.Quentity = quentity;
                            result.DevisionAuto = 24;
                            result.Cause = "Факт выпуска меньше нормы";
                            result.Quentity = action.Quentity;
                            result.Sum = cost * action.Quentity;
                            result.Note = data.Note;
                            db.IlliquidResult.Add(result);
                            db.SaveChanges();
                            listVipusk.Remove(action);
                            quentity -= result.Quentity;
                        }
                        else if (listAddStock.Sum(a => a.Quentity) + Math.Abs(listChangeInTheNorm.Where(a => a.Quentity < 0).Sum(a => a.Quentity)) > 0)
                        {
                            if (listChangeInTheNorm.Sum(a => a.Quentity) < 0)
                            {
                                float resQue = Math.Abs(listChangeInTheNorm.Sum(a => a.Quentity));
                                if (quentity < Math.Abs(listChangeInTheNorm.Sum(a => a.Quentity)))
                                    resQue = quentity;
                                result.DevisionAuto = 30;
                                result.Cause = "Уменьшение нормы";
                                result.Quentity = resQue;
                                result.Sum = cost * result.Quentity;
                                result.Note = data.Note;
                                db.IlliquidResult.Add(result);
                                db.SaveChanges();
                                quentity -= result.Quentity;
                                listChangeInTheNorm.RemoveAll(a => a.Id > 0);
                            }
                            else if (listAddStock.Sum(a => a.Quentity) > 0)
                            {
                                IlliquidAction action = listAddStock.First();
                                if (action.Quentity > quentity)
                                    action.Quentity = quentity;
                                result.DevisionAuto = 7;
                                result.Cause = "Закуплено не по норме";
                                result.Quentity = action.Quentity;
                                result.Sum = cost * result.Quentity;
                                result.Note = data.Note;
                                db.IlliquidResult.Add(result);
                                db.SaveChanges();
                                quentity -= result.Quentity;
                                listAddStock.Remove(action);
                            }
                            else
                            {
                                result.DevisionAuto = 24;
                                result.Cause = "НД";
                                result.Quentity = quentity;
                                result.Sum = cost * quentity;
                                result.Note = data.Note;
                                db.IlliquidResult.Add(result);
                                db.SaveChanges();
                                quentity -= result.Quentity;
                            }
                        }
                        else
                        {
                            result.DevisionAuto = 24;
                            result.Cause = "Преждние периоды";
                            result.Quentity = quentity;
                            result.Sum = cost * quentity;
                            result.Note = data.Note;
                            db.IlliquidResult.Add(result);
                            db.SaveChanges();
                            quentity -= result.Quentity;
                        }
                    }
                    data.IsAnalisis = true;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        void SaveActionsAddStock(Wiki.Illiquid illiquid, float control)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var addStock = db.IlliquidAddStock
                    .AsNoTracking()
                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId)
                    .OrderByDescending(a => a.Date)
                    .ToList();
                foreach (var data in addStock)
                {
                    IlliquidAction action = new IlliquidAction
                    {
                        ActionId = 4,
                        IlliquidId = illiquid.id,
                        Date = data.Date,
                        Quentity = data.Quentity,
                        Note = data.Provider
                    };
                    if (data.Provider == "X")
                        action.ActionId = 7;
                    db.IlliquidAction.Add(action);
                    db.SaveChanges();
                    control -= action.Quentity;
                    if (control <= 0.0f)
                        break;
                }
            }
        }

        void SaveActionsOrders(Wiki.Illiquid illiquid, float control)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                DateTime filt = illiquid.IlliquidStockState1.Date.AddDays(-185);
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var orders = db.IlliquidOrders
                                .AsNoTracking()
                                .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.Date > filt)
                                .OrderByDescending(a => a.Date)
                                .ToList();
                foreach (var data in orders)
                {
                    IlliquidAction action = new IlliquidAction
                    {
                        ActionId = 3,
                        IlliquidId = illiquid.id,
                        Date = data.Date,
                        Quentity = data.Ordered,
                        Note = ""
                    };
                    db.IlliquidAction.Add(action);
                    db.SaveChanges();
                    control -= action.Quentity;
                    if (control <= 0.0f)
                        break;
                }
            }
        }

        void SaveActionsSN(Wiki.Illiquid illiquid, float control)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                DateTime filt = illiquid.IlliquidStockState1.Date.AddDays(-185);
                var snOrders = db.IlliquidSN
                    .AsNoTracking()
                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.Date > filt)
                    .OrderByDescending(a => a.Date)
                    .ToList();
                foreach (var data in snOrders)
                {
                    IlliquidAction action = new IlliquidAction
                    {
                        ActionId = 6,
                        IlliquidId = illiquid.id,
                        Date = data.Date,
                        Quentity = data.Quentity,
                        Note = data.Devision
                    };
                    db.IlliquidAction.Add(action);
                    db.SaveChanges();
                    control -= action.Quentity;
                    if (control <= 0.0f)
                        break;
                }
            }
        }

        void SaveActionsVozvratMOL(Wiki.Illiquid illiquid, float control, DateTime start)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var list = db.IlliquidVozvratMOL
                    .AsNoTracking()
                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.DateDoc >= start)
                    .OrderByDescending(a => a.DateDoc)
                    .ToList();
                foreach (var data in list)
                {
                    IlliquidAction action = new IlliquidAction
                    {
                        ActionId = 11,
                        IlliquidId = illiquid.id,
                        Date = data.DateDoc,
                        Quentity = data.Quentity,
                        Note = ""
                    };
                    db.IlliquidAction.Add(action);
                    db.SaveChanges();
                    control -= action.Quentity;
                    if (control <= 0.0f)
                        break;
                }
            }
        }

        void SaveActionsVozvrat(Wiki.Illiquid illiquid, float control, DateTime start)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var list = db.IlliquidVozvrat
                    .AsNoTracking()
                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.DateDoc >= start)
                    .OrderByDescending(a => a.DateDoc)
                    .ToList();
                foreach (var data in list)
                {
                    IlliquidAction action = new IlliquidAction
                    {
                        ActionId = 8,
                        IlliquidId = illiquid.id,
                        Date = data.DateDoc,
                        Quentity = data.Quentity,
                        Note = ""
                    };
                    db.IlliquidAction.Add(action);
                    db.SaveChanges();
                    control -= action.Quentity;
                    if (control <= 0.0f)
                        break;
                }
            }
        }

        void SaveActionsVipusk(Wiki.Illiquid illiquid, float control, DateTime start)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var list = db.IlliquidVipusk
                    .AsNoTracking()
                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.DateDoc >= start)
                    .Where(a => a.Norm - a.Fact > 0)
                    .OrderByDescending(a => a.DateDoc)
                    .ToList();
                foreach (var data in list)
                {
                    IlliquidAction action = new IlliquidAction
                    {
                        ActionId = 10,
                        IlliquidId = illiquid.id,
                        Date = data.DateDoc,
                        Quentity = data.Norm - data.Fact,
                        Note = ""
                    };
                    db.IlliquidAction.Add(action);
                    db.SaveChanges();
                    control -= action.Quentity;
                    if (control <= 0.0f)
                        break;
                }
            }
        }

        void SaveActionsReplacement(Wiki.Illiquid illiquid, float control, DateTime start)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var list = db.IlliquidReplacement
                    .AsNoTracking()
                    .Where(a => a.SKUNormId == illiquid.IlliquidStockState1.SKUId && a.DateDoc >= start)
                    .OrderByDescending(a => a.DateDoc)
                    .ToList();
                foreach (var data in list)
                {
                    IlliquidAction action = new IlliquidAction
                    {
                        ActionId = 5,
                        IlliquidId = illiquid.id,
                        Date = data.DateDoc,
                        Quentity = data.QuentityNorm,
                        Note = ""
                    };
                    db.IlliquidAction.Add(action);
                    db.SaveChanges();
                    control -= action.Quentity;
                    if (control <= 0.0f)
                        break;
                }
            }
        }

        void SaveActionsChangeNorms(Wiki.Illiquid illiquid, DateTime startDate, float controlQue)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var listChangeNorms = db.IlliquidChangeInTheNorm
                                            .AsNoTracking()
                                            .Include(a => a.PZ_PlanZakaz)
                                            .Include(a => a.IlliquidChangeInTheNormUsers.Select(b => b.AspNetUsers))
                                            .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.Date >= startDate)
                                            .Where(a => a.PZ_PlanZakaz.dataOtgruzkiBP > a.Date)
                                            .OrderByDescending(a => a.Date)
                                            .ToList();
                foreach (var data in listChangeNorms)
                {
                    IlliquidAction action1 = new IlliquidAction
                    {
                        ActionId = 1,
                        IlliquidId = illiquid.id,
                        Date = data.Date,
                        Quentity = data.ChangeInTheNorm,
                        Note = ""
                    };
                    string note = "";
                    note += data.PZ_PlanZakaz.PlanZakaz.ToString() + " | ";
                    foreach (var user in data.IlliquidChangeInTheNormUsers)
                    {
                        int index = user.AspNetUsers.CiliricalName.IndexOf(" ");
                        note += user.AspNetUsers.CiliricalName.Substring(0, index) + " ";
                    }
                    action1.Note = note;
                    db.IlliquidAction.Add(action1);
                    db.SaveChanges();
                }
                var listChangeForQue = db.IlliquidChangeInTheNorm
                            .AsNoTracking()
                            .Include(a => a.PZ_PlanZakaz)
                            .Include(a => a.IlliquidChangeInTheNormUsers.Select(b => b.AspNetUsers))
                            .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.Date < startDate)
                            .Where(a => a.PZ_PlanZakaz.dataOtgruzkiBP > a.Date)
                            .OrderByDescending(a => a.Date)
                            .ToList();

                foreach (var data in listChangeForQue)
                {
                    IlliquidAction action1 = new IlliquidAction
                    {
                        ActionId = 9,
                        IlliquidId = illiquid.id,
                        Date = data.Date,
                        Quentity = data.ChangeInTheNorm,
                        Note = ""
                    };
                    string note = "";
                    note += data.PZ_PlanZakaz.PlanZakaz.ToString() + " | ";
                    foreach (var user in data.IlliquidChangeInTheNormUsers)
                    {
                        int index = user.AspNetUsers.CiliricalName.IndexOf(" ");
                        note += user.AspNetUsers.CiliricalName.Substring(0, index) + " ";
                    }
                    action1.Note = note;
                    db.IlliquidAction.Add(action1);
                    db.SaveChanges();
                    controlQue -= action1.Quentity;
                    if (controlQue <= 0.0f)
                        break;
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