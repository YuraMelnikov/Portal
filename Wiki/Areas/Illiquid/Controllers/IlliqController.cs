using NLog;
using Syncfusion.XlsIO;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Wiki.Areas.Illiquid.Controllers
{
    public class IlliqController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult LoadingStock()
        {
            string login = HttpContext.User.Identity.Name;
            try
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
            catch (Exception ex)
            {
                logger.Error("Illiq / LoadingStock: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetIlliquidPeriod(DateTime startDate, DateTime finishDate)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    var listBefore = db.IlliquidStockState.Where(a => a.Date == startDate).ToList();
                    var listNext = db.IlliquidStockState.Where(a => a.Date == finishDate).ToList();
                    List<Wiki.Illiquid> list = new List<Wiki.Illiquid>();
                    foreach (var data in listNext)
                    {
                        float queBefore = listBefore.Where(a => a.SKUId == data.SKUId).Sum(a => a.SurplusQue);
                        if(data.SurplusQue - queBefore >= 1.0)
                        {
                            Wiki.Illiquid illiquid = new Wiki.Illiquid
                            {
                                idIlliquidStockStateNext = data.Id,
                                quantityNext = data.SurplusQue,
                                quantityBefore = queBefore,
                                idIlliquidStockStateBefore = null, 
                                Cause = ""
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
            catch (Exception ex)
            {
                logger.Error("Illiq / GetIlliquidPeriod: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
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
                        .Include(a => a.Devision)
                        .Include(a => a.IlliquidStockState1.SKU)
                        .Include(a => a.IlliquidStockState)
                        .Include(a => a.IlliquidStockState1)
                        .ToList();
  
                    var data = query.Select(dataList => new
                    {
                        id = dataList.id
                        , editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return AnalisysIlliquid('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-flash" + '\u0022' + "></span></a></td>"
                        , code = dataList.IlliquidStockState1.SKU.sku1
                        , materialName = dataList.IlliquidStockState1.SKU.designation + " |" + dataList.IlliquidStockState1.SKU.indexMaterial + "| " + dataList.IlliquidStockState1.SKU.name
                        , queBefore = Math.Round(dataList.quantityBefore, 2)
                        , queNext = Math.Round(dataList.quantityNext, 2)
                        , que = Math.Round(dataList.quantityNext - dataList.quantityBefore, 2)
                        , updateNorm = GetUpdateNorm(dataList.id)
                        , note = dataList.Note
                        , orders = GetOrders(dataList.id)
                        , added = GetAdded(dataList.id)
                        , addedX = GetAddedXForTable(dataList.id)
                        , sn = GetSNForTable(dataList.id)
                        , replacment = GetReplacementForTable(dataList.id)
                        , sum = dataList.IlliquidStockState1.SurplusSum - GetNullFloat(dataList.IlliquidStockState)
                        , cause = dataList.Cause
                        , devision = GetDevisionName(dataList.Devision)
                        , max = dataList.IlliquidStockState1.SKU.Max
                        , moveStock = ""
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

        [HttpPost]
        public JsonResult AnalisysIlliquid()
        {
            //try
            //{
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                DateTime controlDate = DateTime.Now;
                DateTime lastAdd = DateTime.Now;
                DateTime lastOrder = DateTime.Now;
                float controlQuantity = 0.0f;
                var query = db.Illiquid
                    .Include(a => a.IlliquidStockState1.SKU)
                    .Where(a => a.IsAnalisis == false)
                    .ToList();
                foreach (var data in query)
                {
                    controlDate = GetLastAddStockDate(data);
                    lastAdd = controlDate;
                    controlQuantity = GetAddStockDate(data);
                    DateTime dateFoxX = db.IlliquidAction.Where(a => a.ActionId == 4 && a.IlliquidId == data.id).Min(a => a.Date);
                    GetAddStockX(data, dateFoxX);
                    controlDate = GetLastOrders(data, controlQuantity, controlDate);
                    lastOrder = controlDate;
                    GetChangeNorms(data, controlDate, controlQuantity);
                    GetReplacementNorms(data);
                    GetIlliquidAction(data, controlDate);
                    float illiqQue = data.quantityNext - data.quantityBefore;
                    var listCause = db.IlliquidAction
                                        .Where(a => a.IlliquidId == data.id)
                                        .OrderBy(a => a.Date)
                                        .ToList();
                    DateTime date = DateTime.Now.AddDays(-9125);
                    IlliquidGroupAction groupAction = new IlliquidGroupAction();
                    groupAction.Date = date;
                    foreach (var cause in listCause)
                    {
                        if(date != cause.Date)
                        {
                            if(groupAction.Date != date)
                            {
                                db.IlliquidGroupAction.Add(groupAction);
                                db.SaveChanges();
                            }
                            groupAction = new IlliquidGroupAction
                            {
                                Added = 0.0f,
                                Change = 0.0f,
                                Date = cause.Date,
                                IlliquidId = cause.IlliquidId,
                                Ordered = 0.0f,
                                SN = 0.0f,
                                State = 0.0f, 
                                AddedX = 0.0f, 
                                Replacement = 0.0f
                            };
                        }
                        if (cause.ActionId == 1) // +Change
                        {
                            groupAction.Change += cause.Quentity;
                            groupAction.State += cause.Quentity;
                        }
                        else if (cause.ActionId == 3) // -Ordered
                        {
                            groupAction.Ordered -= cause.Quentity;
                        }
                        else if (cause.ActionId == 4) // -Added
                        {
                            groupAction.Added -= cause.Quentity;
                            groupAction.State -= cause.Quentity;
                        }
                        else if (cause.ActionId == 5) // Replacement
                        {
                            groupAction.Replacement += cause.Quentity;
                        }
                        else if (cause.ActionId == 6) // +SN
                        {
                            groupAction.SN += cause.Quentity;
                            groupAction.State += cause.Quentity;
                        }
                        else if (cause.ActionId == 7) // X
                        {
                            groupAction.AddedX += cause.Quentity;
                        }
                    }
                    if(listCause.Count > 1)
                    {
                        db.IlliquidGroupAction.Add(groupAction);
                        db.SaveChanges();
                    }
                    float added = 0.0f;
                    float change = 0.0f;
                    float x = 0.0f;
                    bool isPossitive = false;
                    var listGroupActions = db.IlliquidGroupAction
                        .Where(a => a.IlliquidId == data.id)
                        .OrderBy(a => a.Date)
                        .ToList();
                    foreach (var gAction in listGroupActions)
                    {
                        if (isPossitive == false && gAction.Change < 0)
                        {
                            isPossitive = true;
                        }
                        if (isPossitive == true)
                        {
                            change += gAction.Change;
                        }
                        added += gAction.Added;
                        x += gAction.AddedX;
                    }
                    double queMax = db.SKU.First(a => a.id == data.IlliquidStockState1.SKUId).Max.Value;
                    if(x + added >= 0 && x > 0)
                    {
                        if (lastAdd < DateTime.Now.AddDays(-60))
                        {
                            data.Cause = "Неликвид прошлых периодов";
                            data.DevisionAutomatic = 24;
                        }
                        else
                        {
                            data.Cause = "Поступление от Х";
                            data.DevisionAutomatic = 6;
                        }
                    }
                    else if (x + added < 0 && x > 0)
                    {
                        data.Cause = "Частично поступление от Х";
                        data.DevisionAutomatic = 6;
                    }
                    else if (queMax > 0)
                    {
                        data.Cause = "Неснижаемый остаток";
                        data.DevisionAutomatic = 24;
                    }
                    else if (change < 0)
                    {
                        DateTime filt = DateTime.Now.AddDays(-7300);
                        try
                        {
                            filt = db.IlliquidAction.Where(a => a.ActionId == 3 && a.IlliquidId == data.id).Min(a => a.Date);
                        }
                        catch
                        {
                        }
                        DateTime lastChNorm = db.IlliquidAction.Where(a => a.ActionId == 1 && a.IlliquidId == data.id).Min(a => a.Date);
                        if (lastChNorm > filt && lastChNorm < DateTime.Now.AddDays(-60))
                        {
                            data.Cause = "Неликвид прошлых периодов";
                            data.DevisionAutomatic = 24;
                        }
                        else
                        {
                            if(change * -1 < data.quantityNext - data.quantityBefore)
                            {
                                data.Cause = "Снабжением закуплено ТМЦ свыше потребности";
                                data.DevisionAutomatic = 7;
                            }
                            else
                            {
                                data.Cause = "Уменьшение норм сотрудником КО";
                                data.DevisionAutomatic = 30;
                            }
                        }
                    }
                    else if (change == 0)
                    {
                        if(lastAdd < DateTime.Now.AddDays(-60))
                        {
                            data.Cause = "Неликвид прошлых периодов";
                            data.DevisionAutomatic = 24;
                        }
                        else
                        {
                            data.Cause = "Снабжением закуплено ТМЦ свыше потребности";
                            data.DevisionAutomatic = 7;
                        }
                    }
                    else if (change > 0)
                    {
                        if (lastAdd < DateTime.Now.AddDays(-60))
                        {
                            data.Cause = "Неликвид прошлых периодов";
                            data.DevisionAutomatic = 24;
                        }
                        else 
                        {
                            data.Cause = "Снабжением закуплено ТМЦ свыше потребности";
                            data.DevisionAutomatic = 7;
                        }
                    }
                    data.IsAnalisis = true;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            //}
            //catch (Exception ex)
            //{
            //    logger.Error("CMOSSController / GetTableOrders: " + " | " + ex);
            //    return Json(0, JsonRequestBehavior.AllowGet);
            //}
        }

        string GetAddedXForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var listX = db.IlliquidAction
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

        string GetDevisionName(Devision devision)
        {
            if (devision == null)
                return "";
            else
                return devision.name;
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

        string GetAdded(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
                    .Where(a => a.IlliquidId == id && a.ActionId == 4)
                    .OrderBy(a => a.Date)
                    .ToList();
                int count = list.Count;
                for(int i = 0; i < count; i++)
                {
                    if(i + 1 == count) 
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString();
                    else
                        data += list[i].Quentity.ToString() + " | " + list[i].Date.ToShortDateString() + "</br>";
                }
                return data;
            }
        }

        string GetOrders(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
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

        string GetUpdateNorm(int idIlliquid)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                string data = "";
                var list = db.IlliquidAction
                    .Where(a => a.IlliquidId == idIlliquid && a.ActionId == 1)
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

        bool GetReplacementNorms(Wiki.Illiquid illiquid)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var changeTheNorm = db.IlliquidReplacement
                                            .Where(a => a.SKUNormId == illiquid.IlliquidStockState1.SKUId)
                                            .ToList();
                foreach (var data in changeTheNorm)
                {
                    IlliquidAction action5 = new IlliquidAction
                    {
                        ActionId = 5,
                        IlliquidId = illiquid.id,
                        Date = DateTime.Now,
                        Quentity = data.QuentityNorm,
                        Note = ""
                    };
                    db.IlliquidAction.Add(action5);
                    db.SaveChanges();
                }
            }

            return true;
        }

        bool GetAddStockX(Wiki.Illiquid illiquid, DateTime control)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                var addStockX = db.IlliquidAddStock
                                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.Provider == "X" && a.Date >= control)
                                    .ToList();
                foreach (var data in addStockX)
                {
                    IlliquidAction action7 = new IlliquidAction
                    {
                        ActionId = 7,
                        IlliquidId = illiquid.id,
                        Date = data.Date,
                        Quentity = data.Quentity,
                        Note = data.Provider
                    };
                    db.IlliquidAction.Add(action7);
                    db.SaveChanges();
                }
            }
            return true;
        }

        bool GetChangeNorms(Wiki.Illiquid illiquid, DateTime control, float controlQue)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var listChangeNorms = db.IlliquidChangeInTheNorm
                                            .Include(a => a.PZ_PlanZakaz)
                                            .Include(a => a.IlliquidChangeInTheNormUsers.Select(b => b.AspNetUsers))
                                            .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.Date >= control)
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
                            .Include(a => a.PZ_PlanZakaz)
                            .Include(a => a.IlliquidChangeInTheNormUsers.Select(b => b.AspNetUsers))
                            .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.Date < control)
                            .Where(a => a.PZ_PlanZakaz.dataOtgruzkiBP > a.Date)
                            .OrderByDescending(a => a.Date)
                            .ToList();

                foreach (var data in listChangeForQue)
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
                    controlQue -= action1.Quentity;
                    if (controlQue <= 0.0f)
                        break;
                }

                return true;
            }
        }

        DateTime GetLastAddStockDate(Wiki.Illiquid illiquid)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IlliquidAddStock lastAddStock = GetLastIlliquidAddStock(illiquid.IlliquidStockState1.SKUId);
                if (lastAddStock != null)
                {
                    IlliquidAction action2 = new IlliquidAction
                    {
                        ActionId = 2,
                        IlliquidId = illiquid.id,
                        Date = lastAddStock.Date,
                        Quentity = lastAddStock.Quentity,
                        Note = ""
                    };
                    db.IlliquidAction.Add(action2);
                    db.SaveChanges();
                    return action2.Date;
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        float GetAddStockDate(Wiki.Illiquid illiquid)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var addStock = db.IlliquidAddStock
                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId)
                    .OrderByDescending(a => a.Date)
                    .ToList();
                float control = illiquid.quantityNext - illiquid.quantityBefore;
                foreach (var data in addStock)
                {
                    IlliquidAction action4 = new IlliquidAction
                    {
                        ActionId = 4,
                        IlliquidId = illiquid.id,
                        Date = data.Date,
                        Quentity = data.Quentity,
                        Note = data.Provider
                    };
                    db.IlliquidAction.Add(action4);
                    db.SaveChanges();
                    control -= action4.Quentity;
                    if (control <= 0.0f)
                        break;
                }
                return db.IlliquidAction.Where(a => a.IlliquidId == illiquid.id && a.ActionId == 4).Sum(a => a.Quentity);
            }
        }

        DateTime GetLastOrders(Wiki.Illiquid illiquid, float control, DateTime controlDate)
        {
            DateTime orderDate = DateTime.Now;
            if (control > 0)
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var orders = db.IlliquidOrders
                                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId)
                                    .OrderByDescending(a => a.Date)
                                    .ToList();
                    foreach (var data in orders)
                    {
                        IlliquidAction action3 = new IlliquidAction
                        {
                            ActionId = 3,
                            IlliquidId = illiquid.id,
                            Date = data.Date,
                            Quentity = data.Ordered,
                            Note = ""
                        };
                        db.IlliquidAction.Add(action3);
                        db.SaveChanges();
                        orderDate = action3.Date;
                        if(action3.Quentity > 0)
                        {
                            control -= action3.Quentity;
                            if (control <= 0.0f)
                                break;
                        }
                    }
                }
                if (orderDate < controlDate)
                    return orderDate;
                else
                    return controlDate;
            }
            return controlDate;
        }

        bool GetIlliquidAction(Wiki.Illiquid illiquid, DateTime control)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var snOrders = db.IlliquidSN
                    .Where(a => a.SKUId == illiquid.IlliquidStockState1.SKUId && a.Date >= control)
                    .ToList();
                foreach (var data in snOrders)
                {
                    IlliquidAction action6 = new IlliquidAction
                    {
                        ActionId = 6,
                        IlliquidId = illiquid.id,
                        Date = data.Date,
                        Quentity = data.Quentity,
                        Note = data.Devision
                    };
                    db.IlliquidAction.Add(action6);
                    db.SaveChanges();
                }
            }
            return true;
        }

        IlliquidAddStock GetLastIlliquidAddStock(int skuId)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    return db.IlliquidAddStock
                        .Where(a => a.SKUId == skuId)
                        .OrderByDescending(a => a.Date)
                        .First();
                }
                catch
                {
                    return null;
                }
            }
        }

        string GetPeriod(IlliquidStockState illiq)
        {
            if (illiq == null)
                return " - ";
            else
                return illiq.Date.ToShortDateString();
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