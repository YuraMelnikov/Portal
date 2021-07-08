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
            PortalKATEKEntities db = new PortalKATEKEntities();
            ViewBag.Devision = new SelectList(db.Devision.OrderBy(a => a.name), "id", "name");
            ViewBag.Type = new SelectList(db.IlliquidType.Where(d => d.IsActive == true).OrderBy(d => d.Name), "id", "Name");
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
                                idIlliquidStockStateBefore = null
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
                        .Include(a => a.IlliquidStockState1.SKU)
                        .Include(a => a.IlliquidStockState)
                        .Include(a => a.IlliquidStockState1)
                        .ToList();
  
                    var data = query.Select(dataList => new
                    {
                        id = dataList.id
                        , editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetIlliquid('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>"
                        , code = dataList.IlliquidStockState1.SKU.sku1
                        , materialName = dataList.IlliquidStockState1.SKU.designation + " |" + dataList.IlliquidStockState1.SKU.indexMaterial + "| " + dataList.IlliquidStockState1.SKU.name
                        , queBefore = Math.Round(dataList.quantityBefore, 2)
                        , queNext = Math.Round(dataList.quantityNext, 2)
                        , que = Math.Round(dataList.quantityNext - dataList.quantityBefore, 2)
                        , updateNorm = GetChangeNormForTable(dataList.id)
                        , note = dataList.Note
                        , orders = GetOrdersForTable(dataList.id)
                        , added = GetAddedForTable(dataList.id)
                        , addedX = GetAddedXForTable(dataList.id)
                        , sn = GetSNForTable(dataList.id)
                        , replacment = GetReplacementForTable(dataList.id)
                        , sum = dataList.IlliquidStockState1.SurplusSum - GetNullFloat(dataList.IlliquidStockState)
                        , cause = GetCauseForTable(dataList.id)
                        //, devision = GetDevisionNameForTable(dataList.Devision)
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

        //idIlliquid
        //devision
        //typeError
        //noteIlliquid
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

        string GetCauseForTable(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                string result = "";
                var res = db.IlliquidResult.Where(a => a.IlliquidId == id).ToList();
                foreach (var data in res)
                {
                    result += data.Cause + "; ";
                }
                return result;
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

        [HttpPost]
        public JsonResult AnalisysIlliquid()
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var query = db.Illiquid
                    .Include(a => a.IlliquidStockState)
                    .Include(a => a.IlliquidStockState1)
                    .Include(a => a.IlliquidStockState1.SKU)
                    .Where(a => a.IsAnalisis == false)
                    .ToList();
                foreach (var data in query)
                {
                    bool isMax = false;
                    if (data.IlliquidStockState1.SKU.Max > 0)
                        isMax = true;
                    float addStock = 0.0f;
                    addStock = GetAddStockActions(data);
                    GetOrdersActions(data, addStock);
                    DateTime finishPeriod = data.IlliquidStockState1.Date;
                    DateTime startPeriod = finishPeriod.AddDays(-16);
                    try
                    {
                        startPeriod = data.IlliquidStockState.Date.AddDays(-1);
                    }
                    catch
                    {
                    }
                    DateTime minDateForChange = finishPeriod.AddDays(-16);
                    try
                    {
                        minDateForChange = db.IlliquidAction
                            .Where(a => a.IlliquidId == data.id)
                            .Where(a => a.ActionId == 3 || a.ActionId == 4 || a.ActionId == 7)
                            .Min(a => a.Date);
                    }
                    catch
                    {
                    }
                    GetChangeNormsActions(data, minDateForChange, addStock);
                    GetSNActions(data, minDateForChange);
                    //8 - возврат из производства
                    //5 - замена в производстве
                    //- - списано меньше чем по норме

                    var actions = db.IlliquidAction
                        .Where(a => a.Date >= minDateForChange && a.IlliquidId == data.id)
                        .ToList();
                    foreach (var act in actions)
                    {
                        IlliquidGroupAction ga = new IlliquidGroupAction();
                        if (act.ActionId == 1)
                            ga.Change = act.Quentity;
                        else if (act.ActionId == 3)
                            ga.Ordered = act.Quentity;
                        else if (act.ActionId == 4)
                            ga.Added = act.Quentity;
                        else if (act.ActionId == 5) 
                            ga.Replacement = act.Quentity;
                        else if (act.ActionId == 6)
                            ga.SN = act.Quentity;
                        else if (act.ActionId == 7)
                            ga.AddedX = act.Quentity;
                        else if (act.ActionId == 8)
                            ga.LastMoveStock = act.Quentity;
                        ga.Date = act.Date;
                        ga.IlliquidId = act.IlliquidId;
                        if (act.Date >= startPeriod)
                        {
                            if(act.ActionId == 1 && act.Quentity > 0.0f)
                                ga.IsPeriod = false;
                            else
                                ga.IsPeriod = true;
                        }
                        else
                            ga.IsPeriod = false;
                        db.IlliquidGroupAction.Add(ga);
                        db.SaveChanges();
                    }
                    float control = data.quantityNext - data.quantityBefore;
                    DateTime controlDate = DateTime.Now.AddDays(-185);
                    var groupres = db.IlliquidGroupAction.Where(a => a.IlliquidId == data.id).ToList();
                    if (isMax == true)
                    {
                        IlliquidResult result = new IlliquidResult
                        {
                            Cause = "Неснижаемый остаток",
                            DevisionAuto = 24,
                            IlliquidId = data.id
                        };
                        AddedResult(result);
                    }
                    else if (groupres.Count(a => a.IsPeriod == true) == 0)
                    {
                        IlliquidResult result = new IlliquidResult
                        {
                            Cause = "Преждние периоды",
                            DevisionAuto = 24,
                            IlliquidId = data.id
                        };
                        AddedResult(result);
                    }
                    else if (groupres.Where(a => a.IsPeriod == true).Sum(a => a.AddedX) > 0)
                    {
                        if (groupres.Where(a => a.IsPeriod == true).Sum(a => a.AddedX) - control >= 0.0f)
                        {
                            IlliquidResult result = new IlliquidResult
                            {
                                Cause = "Поступление от Х",
                                DevisionAuto = 25,
                                IlliquidId = data.id
                            };
                            AddedResult(result);
                        }
                        else
                        {
                            IlliquidResult result = new IlliquidResult
                            {
                                Cause = "Поступление от Х",
                                DevisionAuto = 25,
                                IlliquidId = data.id
                            };
                            AddedResult(result);
                            ////////остаток по 2 алгоритму!
                            //////IlliquidResult result = new IlliquidResult
                            //////{
                            //////    Cause = "Остаток 2 алгоритма",
                            //////    DevisionAuto = 24,
                            //////    IlliquidId = data.id
                            //////};
                            //////AddedResult(result);
                            if (groupres.Where(a => a.Added > 0 || a.AddedX > 0).Max(a => a.Date) < controlDate)
                            {
                                IlliquidResult result1 = new IlliquidResult
                                {
                                    Cause = "Преждние периоды",
                                    DevisionAuto = 24,
                                    IlliquidId = data.id
                                };
                                AddedResult(result1);
                            }
                            else if (groupres.Sum(a => a.Change) < 0.0f)
                            {
                                IlliquidResult result2 = new IlliquidResult
                                {
                                    Cause = "Уменьшение нормы",
                                    DevisionAuto = 30,
                                    IlliquidId = data.id
                                };
                                AddedResult(result2);
                            }
                            else if (groupres.Sum(a => a.Added) - groupres.Sum(a => a.Change) >= 0)
                            {
                                IlliquidResult result3 = new IlliquidResult
                                {
                                    Cause = "Закуплено не по норме",
                                    DevisionAuto = 7,
                                    IlliquidId = data.id
                                };
                                AddedResult(result3);
                            }
                            else if (groupres.Sum(a => a.Change) - control > 0)
                            {
                                IlliquidResult result4 = new IlliquidResult
                                {
                                    Cause = "Преждние периоды, закуплено не по норме",
                                    DevisionAuto = 24,
                                    IlliquidId = data.id
                                };
                                AddedResult(result4);
                            }
                            else
                            {
                                IlliquidResult result5 = new IlliquidResult
                                {
                                    Cause = "НД",
                                    DevisionAuto = 24,
                                    IlliquidId = data.id
                                };
                                AddedResult(result5);
                            }
                        }
                    }
                    else if (groupres.Where(a => a.Added > 0 || a.AddedX > 0).Max(a => a.Date) < controlDate)
                    {
                        IlliquidResult result = new IlliquidResult
                        {
                            Cause = "Преждние периоды",
                            DevisionAuto = 24,
                            IlliquidId = data.id
                        };
                        AddedResult(result);
                    }
                    else if (groupres.Sum(a => a.Change) < 0.0f)
                    {
                        IlliquidResult result = new IlliquidResult
                        {
                            Cause = "Уменьшение нормы",
                            DevisionAuto = 30,
                            IlliquidId = data.id
                        };
                        AddedResult(result);
                    }
                    else if (groupres.Sum(a => a.Added) - groupres.Sum(a => a.Change) >= 0)
                    {
                        IlliquidResult result = new IlliquidResult
                        {
                            Cause = "Закуплено не по норме",
                            DevisionAuto = 7,
                            IlliquidId = data.id
                        };
                        AddedResult(result);
                    }
                    else if (groupres.Sum(a => a.Change) - control > 0)
                    {
                        IlliquidResult result = new IlliquidResult
                        {
                            Cause = "Преждние периоды, закуплено не по норме",
                            DevisionAuto = 24,
                            IlliquidId = data.id
                        };
                        AddedResult(result);
                    }
                    else
                    {
                        IlliquidResult result = new IlliquidResult
                        {
                            Cause = "НД",
                            DevisionAuto = 24,
                            IlliquidId = data.id
                        };
                        AddedResult(result);
                    }
                    data.IsAnalisis = true;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    //IsAnalisis
                    //DevisionAutomatic
                    //Cause
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        void AddedResult(IlliquidResult result)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.IlliquidResult.Add(result);
                db.SaveChanges();
            }
        }

        float GetAddStockActions(Wiki.Illiquid illiquid)
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

                float result = 0.0f;
                try
                {
                    result += db.IlliquidAction.Where(a => a.IlliquidId == illiquid.id && a.ActionId == 4).Sum(a => a.Quentity);
                }
                catch
                {
                }
                return result;
            }
        }

        bool GetOrdersActions(Wiki.Illiquid illiquid, float control)
        {
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
                        if (action.Quentity > 0.0f)
                        {
                            control -= action.Quentity;
                            if (control <= 0.0f)
                                break;
                        }
                    }
                }
            }
            return true;
        }

        bool GetChangeNormsActions(Wiki.Illiquid illiquid, DateTime startDate, float controlQue)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                var listChangeNorms = db.IlliquidChangeInTheNorm
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
                return true;
            }
        }

        bool GetSNActions(Wiki.Illiquid illiquid, DateTime control)
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