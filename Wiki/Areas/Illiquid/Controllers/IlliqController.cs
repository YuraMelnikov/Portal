using NLog;
using Syncfusion.XlsIO;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Areas.CMOS.Struct;

namespace Wiki.Areas.Illiquid.Controllers
{
    public class IlliqController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
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

        public JsonResult LoadingSku()
        {
            string login = HttpContext.User.Identity.Name;
            try
            {
                using (PortalKATEKEntities db = new PortalKATEKEntities())
                {
                    HttpPostedFileBase fiel = Request.Files[0];
                    string fileReplace = Path.GetFileName(fiel.FileName);
                    var fileName = Path.Combine(Server.MapPath("~/Areas/Illiquid/ToSKU"), fileReplace);
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
                                indexMaterial = t.Cells[4].Value,
                                designation = t.Cells[5].Value,
                                weightR = 0.0
                                ,
                                xml = t.Cells[3].Value
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
                                    ,
                                    XMLCODE = sku.xml
                                };
                                db.SKU.Add(skuAdd);
                                db.SaveChanges();
                            }
                            else
                            {
                                bool isUpdate = false;
                                skuIn = dbres.First(a => a.sku1 == sku.sku1);
                                if (skuIn.designation != sku.designation)
                                {
                                    skuIn.designation = sku.designation;
                                    isUpdate = true;
                                }
                                if (skuIn.name != sku.name)
                                {
                                    skuIn.name = sku.name;
                                    isUpdate = true;
                                }
                                if (skuIn.indexMaterial != sku.indexMaterial)
                                {
                                    skuIn.indexMaterial = sku.indexMaterial;
                                    isUpdate = true;
                                }
                                if (skuIn.weight != (double)sku.weight)
                                {
                                    skuIn.weight = (double)sku.weight;
                                    isUpdate = true;
                                }
                                if (skuIn.XMLCODE != sku.xml)
                                {
                                    skuIn.XMLCODE = sku.xml;
                                    isUpdate = true;
                                }
                                if (isUpdate == true)
                                {
                                    db.Entry(skuIn).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Illiq / LoadingSku: " + " | " + ex + " | " + login);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}