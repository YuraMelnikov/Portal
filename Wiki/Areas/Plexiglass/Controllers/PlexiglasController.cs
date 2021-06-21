using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq.Dynamic;
using Syncfusion.XlsIO;
using Wiki.Areas.Plexiglass.Models;

namespace Wiki.Areas.Plexiglass.Controllers
{
    public class PlexiglasController : Controller
    {
        #region Fields

        private static Logger logger = LogManager.GetCurrentClassLogger();
        readonly JsonSerializerSettings shortDefaultSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy" };
        readonly JsonSerializerSettings shortSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd" };
        readonly JsonSerializerSettings longUsSetting = new JsonSerializerSettings { DateFormatString = "yyyy.MM.dd HH:mm" };
        readonly JsonSerializerSettings longSetting = new JsonSerializerSettings { DateFormatString = "dd.MM.yyyy HH:mm" };
        private readonly string pathPlexiglas = @"\\192.168.1.30\m$\_ЗАКАЗЫ\Plexiglas\";
        private readonly string pathKOPlexiglas = @"\\192.168.1.16\public$\Katek\Pror&D\Archive\Предразработка\Поликарбонат";

        #endregion

        #region View

        public ActionResult Index()
        {
            PortalKATEKEntities db = new PortalKATEKEntities();
            string login = HttpContext.User.Identity.Name;
            int devisionUser = GetDevision(login);
            ViewBag.id_PlanZakaz = new SelectList(db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP > DateTime.Now).OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.id_CMO_TypeProduct = new SelectList(db.CMO_TypeProduct.Where(d => d.active == true).OrderBy(d => d.name), "id", "name");
            ViewBag.id_CMO_Company = new SelectList(db.PlexiglassCompany.Where(d => d.remove == false).OrderBy(d => d.name), "id", "name"); ;
            if (devisionUser == 15)
                ViewBag.userGroupId = 1;
            else if (devisionUser == 7 || login == "myi@katek.by")
                ViewBag.userGroupId = 2;
            else
                ViewBag.userGroupId = 3;
            return View();
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

        #endregion

        #region GetTables

        public JsonResult GetOrders()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.PlexiglassOrder
                        .AsNoTracking()
                        .Include(a => a.CMO_TypeProduct)
                        .Include(a => a.PlexiglassCompany)
                        .Include(a => a.PZ_PlanZakaz)
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.finishDate == null && a.remove == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        editLink = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-pencil" + '\u0022' + "></span></a></td>",
                        dataList.id,
                        PZ_PlanZakaz = dataList.PZ_PlanZakaz.PlanZakaz,
                        CMO_TypeProduct = dataList.CMO_TypeProduct.name,
                        datetimeCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, shortSetting).Replace(@"""", ""),
                        AspNetUsers = dataList.AspNetUsers.CiliricalName,
                        PlexiglassCompany = dataList.PlexiglassCompany.name,
                        dataList.continueDate,
                        dataList.note,
                        folder = @"<a href =" + dataList.folder + "> Папка </a>",
                        remove = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return RemoveOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-remove" + '\u0022' + "></span></a></td>",
                        posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>"
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("PlexiglasController / GetOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetReportOrder()
        {
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    DateTime controlDate = DateTime.Now.AddDays(-85);
                    db.Configuration.ProxyCreationEnabled = false;
                    db.Configuration.LazyLoadingEnabled = false;
                    var query = db.PlexiglassOrder
                        .AsNoTracking()
                        .Include(a => a.CMO_TypeProduct)
                        .Include(a => a.PlexiglassCompany)
                        .Include(a => a.PZ_PlanZakaz)
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.datetimeCreate > controlDate && a.remove == false)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.id,
                        PZ_PlanZakaz = dataList.PZ_PlanZakaz.PlanZakaz,
                        CMO_TypeProduct = dataList.CMO_TypeProduct.name,
                        datetimeCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, shortSetting).Replace(@"""", ""),
                        finishDate = JsonConvert.SerializeObject(dataList.finishDate, shortSetting).Replace(@"""", ""),
                        AspNetUsers = dataList.AspNetUsers.CiliricalName,
                        PlexiglassCompany = dataList.PlexiglassCompany.name,
                        continueDate = JsonConvert.SerializeObject(dataList.continueDate, shortSetting).Replace(@"""", ""),
                        dataList.note,
                        folder = @"<a href =" + dataList.folder + "> Папка </a>",
                        posList = "<td><a href=" + '\u0022' + "#" + '\u0022' + " onclick=" + '\u0022' + "return GetPositionsOrder('" + dataList.id + "')" + '\u0022' + "><span class=" + '\u0022' + "glyphicon glyphicon-list" + '\u0022' + "></span></a></td>"
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("PlexiglasController / GetReportOrder: " + " | " + ex);
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
                    var query = db.PlexiglassPositionsOrder
                        .AsNoTracking()
                        .Where(a => a.id_PlexiglassOrder == id)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.positionNum,
                        dataList.id_PlexiglassOrder,
                        dataList.designation,
                        dataList.name,
                        dataList.index,
                        quentity = dataList.quentity.ToString(),
                        dataList.square,
                        dataList.barcode
                    });
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("PlexiglasController / GetPositionsOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region GetPostToJS

        [HttpPost]
        public JsonResult AddOrder()
        {
            string login = HttpContext.User.Identity.Name;
            int returnId = 0;
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                HttpPostedFileBase[] files = new HttpPostedFileBase[Request.Files.Count];
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Request.Files[i];
                }
                int[] ord = GetOrdersArray(Request.Form.ToString());
                int typeMaterials = GetTypeMaterials(Request.Form.ToString());
                int customer = GetCustomer(Request.Form.ToString());


                int fileSize = files[0].ContentLength;
                var fileName = Path.GetFileName(files[0].FileName);
                byte[] fileByteArray = new byte[fileSize];
                files[0].InputStream.Read(fileByteArray, 0, fileSize);
                string fileLocation = Path.Combine(Server.MapPath("~/temp"), fileName);
                if (!Directory.Exists(Server.MapPath("~/temp")))
                    Directory.CreateDirectory(Server.MapPath("~/temp"));
                files[0].SaveAs(fileLocation);
                string error = "";
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    IWorkbook workbook = application.Workbooks.Open(fileLocation);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    int lenght = worksheet.Rows.Length;
                    for (int i = 0; i < lenght; i++)
                    {
                        if (worksheet.Rows[i].Columns[1].DisplayText.Contains("PCAM") == true)
                        {
                            PlexiglassPositionsOrder pos = new PlexiglassPositionsOrder
                            {
                                id_PlexiglassOrder = 0,
                                positionNum = "",
                                designation = worksheet.Rows[i].Cells[1].Value.Trim(),
                                name = worksheet.Rows[i].Cells[2].Value.Trim(),
                                index = worksheet.Rows[i].Cells[3].Value.Trim(),
                                quentity = 0,
                                square = 0.0,
                                barcode = ""
                            };
                            bool correct = GetFileMaterials(pos.designation);
                            if (correct == false)
                                error += pos.designation + " - файл не найден " + "\n";
                        }
                    }
                    workbook.Close();
                }
                if (error != "")
                    return Json(error, JsonRequestBehavior.AllowGet);
                foreach (var p in ord)
                {
                    var order = new PlexiglassOrder
                    {
                        id_PZ_PlanZakaz = p,
                        id_CMO_TypeProduct = typeMaterials,
                        id_AspNetUsers = db.AspNetUsers.First(a => a.Email == login).Id,
                        id_PlexiglassCompany = customer,
                        datetimeCreate = DateTime.Now,
                        folder = "",
                        remove = false,
                        note = ""
                    };
                    db.PlexiglassOrder.Add(order);
                    db.SaveChanges();
                    order.folder = CreateFolderAndFileForPreOrder(order.id, files);
                    CreatingPositionsPreorder(order.id, order.folder);
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    new EmailPlexiglas(order, login, 0);
                }
                return Json(returnId, JsonRequestBehavior.AllowGet);
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
                    var query = db.PlexiglassOrder
                        .Include(a => a.CMO_TypeProduct)
                        .Include(a => a.PlexiglassCompany)
                        .Include(a => a.PZ_PlanZakaz)
                        .Include(a => a.AspNetUsers)
                        .Where(a => a.id == id)
                        .ToList();
                    var data = query.Select(dataList => new
                    {
                        dataList.id,
                        PZ_PlanZakaz = dataList.PZ_PlanZakaz.PlanZakaz,
                        CMO_TypeProduct = dataList.CMO_TypeProduct.name,
                        AspNetUsers = dataList.AspNetUsers.CiliricalName,
                        PlexiglassCompany = dataList.PlexiglassCompany.name,
                        datetimeCreate = JsonConvert.SerializeObject(dataList.datetimeCreate, longSetting).Replace(@"""", ""),
                        continueDate = JsonConvert.SerializeObject(dataList.continueDate, shortDefaultSetting).Replace(@"""", ""),
                        finishDate = JsonConvert.SerializeObject(dataList.finishDate, shortDefaultSetting).Replace(@"""", ""),
                        dataList.note
                    });
                    return Json(data.First(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("PlexiglasController / GetOrder: " + " | " + ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateOrder(int id, DateTime? continueDate, DateTime? finishDate)
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    PlexiglassOrder order = db.PlexiglassOrder.Find(id);
                    if (order.continueDate != continueDate)
                    {
                        order.continueDate = continueDate;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (order.finishDate != finishDate)
                    {
                        order.finishDate = finishDate;
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("PlexiglasController / UpdateOrder: " + " | " + ex + " | " + login);
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
                    var ord = db.PlexiglassOrder.Find(id);
                    ord.remove = true;
                    db.Entry(ord).State = EntityState.Modified;
                    db.SaveChanges();
                    new EmailPlexiglas(ord, login, 1);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("PlexiglasController / RemoveOrder: " + " | " + ex + " | " + id.ToString() + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Methods
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

        private int GetCustomer(string str)
        {
            var split = str.Split('=');
            str = split[1].Split('&').Last();
            return Convert.ToInt32(str);
        }

        private string CreateTmpFile(HttpPostedFileBase[] fileUploadArray)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(fileUploadArray[0].InputStream);
                string fullPath = Path.Combine(Server.MapPath("~/temp"), fileUploadArray[0].FileName);
                return fullPath;
            }
        }

        private string CreateFolderAndFileForPreOrder(int id, HttpPostedFileBase[] fileUploadArray)
        {
            string directory = pathPlexiglas + id.ToString() + "\\";
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


        private void CreatingPositionsPreorder(int orderId, string path)
        {
            List<string> fiels = GetFileArray(path);
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                foreach (var fiel in fiels.Where(a => a.Contains("~") == false))
                {
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        IApplication application = excelEngine.Excel;
                        IWorkbook workbook = application.Workbooks.Open(fiel);
                        IWorksheet worksheet = workbook.Worksheets[0];
                        int lenght = worksheet.Rows.Length;
                        for (int i = 0; i < lenght; i++)
                        {
                            if (worksheet.Rows[i].Columns[1].DisplayText.Contains("PCAM") == true)
                            {
                                PlexiglassPositionsOrder pos = new PlexiglassPositionsOrder
                                {
                                    id_PlexiglassOrder = orderId,
                                    positionNum = worksheet.Rows[i].Cells[0].Value,
                                    designation = worksheet.Rows[i].Cells[1].Value.Trim(),
                                    name = worksheet.Rows[i].Cells[2].Value.Trim(),
                                    index = worksheet.Rows[i].Cells[3].Value.Trim(),
                                    quentity = (int)worksheet.Rows[i].Cells[4].Number,
                                    square = 0.0,
                                    barcode = GetBarcode(worksheet.Rows[i].Cells[1].Value, worksheet.Rows[i].Cells[3].Value)
                                };
                                if (double.IsNaN(worksheet.Rows[i].Cells[5].Number))
                                {
                                    try
                                    {
                                        pos.square = Convert.ToDouble(worksheet.Rows[i].Cells[5].DisplayText);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            pos.square = Convert.ToDouble(worksheet.Rows[i].Cells[5].DisplayText.Replace(".", ","));
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                else
                                    pos.square = worksheet.Rows[i].Cells[5].Number;
                                db.PlexiglassPositionsOrder.Add(pos);
                                db.SaveChanges();
                                GetFileMaterials(pos.designation, path);
                                worksheet.Rows[i].Cells[9].Value = pos.barcode;
                                worksheet.Rows[i].Cells[9].Text = pos.barcode;
                            }
                        }
                        workbook.Version = ExcelVersion.Excel2013;
                        workbook.Save();
                        workbook.Close();
                    }
                }
            }
        }

        private bool GetFileMaterials(string name)
        {
            bool result = false;
            name = name.ToLower();
            var fileList = Directory.GetFiles(pathKOPlexiglas).ToList();
            foreach (var data in fileList)
            {
                if (data.ToLower().Contains(name) == true)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void GetFileMaterials(string name, string path)
        {
            name = name.ToLower();
            var fileList = Directory.GetFiles(pathKOPlexiglas).ToList();
            foreach (var data in fileList)
            {
                if (data.ToLower().Contains(name) == true)
                {
                    System.IO.File.Copy(data, path + name + ".dxf", true);
                    break;
                }
            }
        }

        private List<string> GetFileArray(string folder)
        {
            var fileList = Directory.GetFiles(folder).ToList();
            return fileList;
        }

        private string GetBarcode(string designation, string index)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                try
                {
                    var sku = db.SKU.First(a => a.designation == designation && a.indexMaterial == index);
                    return "10000000" + GetCode(sku.sku1.ToString());
                }
                catch
                {
                    return "1000000000000";
                }
            }
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

        #endregion
    }
}