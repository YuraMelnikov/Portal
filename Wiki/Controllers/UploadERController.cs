using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wiki.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace Wiki.Controllers
{
    public class UploadERController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();
        string directory = @"\\192.168.1.30\m$\Пользователи\myi\UploadTEO\";

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UploadFoundationNorm()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFoundationNorm(HttpPostedFileBase[] fileUpload)
        {
            GetData(fileUpload[0]);
            return RedirectToAction("Index");
        }

        private void GetData(HttpPostedFileBase fileUpload)
        {
            string nameSheet = "факт";
            //сохраняем файл в папке с заказом
            string fileReplace = Path.GetFileName(fileUpload.FileName);
            fileReplace = ToSafeFileName(fileReplace);
            var fileName = string.Format("{0}\\{1}", directory, fileReplace);
            fileUpload.SaveAs(fileName);
            //Открываем экселевский файл
            Excel.Application application = new Excel.Application();
            Excel.Workbook workbook = application.Workbooks.Open(fileName);
            Excel.Worksheet worksheet = null;
            //Ищем нужные нам листы
            foreach (Excel.Worksheet data in workbook.Worksheets)
            {
                if (data.Name == nameSheet)
                {
                    worksheet = data;
                    break;
                }
            }
            Excel.Range range = worksheet.UsedRange;
            List <UploadHTEO> list = new List<UploadHTEO>();
            for (int row = 1; row < range.Rows.Count; row++)
            {
                UploadHTEO hteo = new UploadHTEO();
                try
                {
                    hteo.PlanZakaz = Convert.ToInt32(((Excel.Range)range.Cells[row, 2]).Text);
                    try
                    {
                        hteo.DateShip = Convert.ToDateTime(((Excel.Range) range.Cells[row, 4]).Text);
                    }
                    catch
                    {
                        hteo.DateShip = new DateTime();
                    }
                    try
                    {
                        hteo.Virucha = Convert.ToDouble(((Excel.Range)range.Cells[row, 11]).Text);
                    }
                    catch
                    {
                        hteo.Virucha = 0;
                    }
                    try
                    {
                        hteo.OChena = Convert.ToDouble(((Excel.Range)range.Cells[row, 12]).Text);
                    }
                    catch
                    {
                        hteo.OChena = 0;
                    }
                    try
                    {
                        hteo.IzKom = Convert.ToDouble(((Excel.Range)range.Cells[row, 17]).Text);
                    }
                    catch
                    {
                        hteo.IzKom = 0;
                    }
                    try
                    {
                        hteo.PercKredit = Convert.ToDouble(((Excel.Range)range.Cells[row, 18]).Text);
                    }
                    catch
                    {
                        hteo.PercKredit = 0;
                    }
                    try
                    {
                        hteo.SSM = Convert.ToDouble(((Excel.Range)range.Cells[row, 27]).Text);
                    }
                    catch
                    {
                        hteo.SSM = 0;
                    }
                    try
                    {
                        hteo.FSSM = Convert.ToDouble(((Excel.Range)range.Cells[row, 32]).Text);
                    }
                    catch
                    {
                        hteo.FSSM = 0;
                    }
                    try
                    {
                        hteo.SSPO = Convert.ToDouble(((Excel.Range)range.Cells[row, 29]).Text);
                    }
                    catch
                    {
                        hteo.SSPO = 0;
                    }
                    try
                    {
                        hteo.FSSPO = Convert.ToDouble(((Excel.Range)range.Cells[row, 33]).Text);
                    }
                    catch
                    {
                        hteo.FSSPO = 0;
                    }
                    hteo.Curency = 0;
                    try
                    {
                        hteo.PI = Convert.ToDouble(((Excel.Range)range.Cells[row, 16]).Text);
                    }
                    catch
                    {
                        hteo.PI = 0;
                    }
                    try
                    {
                        hteo.DateOpen = Convert.ToDateTime(((Excel.Range)range.Cells[row, 3]).Text);
                    }
                    catch
                    {
                        hteo.DateOpen = new DateTime();
                    }
                    hteo.Valut = "";
                    list.Add(hteo);
                }
                catch
                {

                }
            }
            foreach (var variable in list)
            {
                try
                {
                    int idPz = db.PZ_PlanZakaz.First(d => d.PlanZakaz == variable.PlanZakaz).Id;
                    PZ_TEO teo = db.PZ_TEO.First(d => d.Id_PlanZakaz == idPz);
                    teo.SSRFact = variable.FSSPO;
                    db.Entry(teo).State = EntityState.Modified;
                    db.SaveChanges();
                    if (db.XTEO.Any(d => d.planZakaz == variable.PlanZakaz))
                    {
                        XTEO xteo = db.XTEO.First(d => d.planZakaz == variable.PlanZakaz);
                        xteo.planZakaz = Convert.ToInt32(variable.PlanZakaz);
                        xteo.curency = variable.Curency;
                        xteo.dateShip = variable.DateShip;
                        xteo.fssm = variable.FSSM;
                        xteo.izKom = variable.IzKom;
                        xteo.viruchka = variable.Virucha;
                        xteo.fsspo = variable.FSSPO;
                        xteo.oChena = variable.OChena;
                        xteo.percKredit = variable.PercKredit;
                        xteo.ssm = variable.SSM;
                        xteo.sspo = variable.SSPO;
                        xteo.pi = variable.PI;
                        xteo.dateOpen = variable.DateOpen;
                        xteo.valut = variable.Valut;
                        db.Entry(xteo).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        XTEO xteo = new XTEO();
                        xteo.planZakaz = Convert.ToInt32(variable.PlanZakaz);
                        xteo.curency = variable.Curency;
                        xteo.dateShip = variable.DateShip;
                        xteo.fssm = variable.FSSM;
                        xteo.izKom = variable.IzKom;
                        xteo.viruchka = variable.Virucha;
                        xteo.fsspo = variable.FSSPO;
                        xteo.oChena = variable.OChena;
                        xteo.percKredit = variable.PercKredit;
                        xteo.ssm = variable.SSM;
                        xteo.sspo = variable.SSPO;
                        xteo.pi = variable.PI;
                        xteo.valut = variable.Valut;
                        xteo.dateOpen = variable.DateOpen;
                        db.XTEO.Add(xteo);
                        db.SaveChanges();
                    }
                }
                catch 
                {

                }
            }
            application.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
            application = null;
            workbook = null;
            worksheet = null;
            GC.Collect();
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
    }
}