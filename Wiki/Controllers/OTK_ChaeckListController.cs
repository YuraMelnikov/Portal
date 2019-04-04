using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Wiki.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Wiki.Controllers
{
    public class OTK_ChaeckListController : Controller
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        [Authorize(Roles = "Admin, Sklad, OTK, OS, Manufacturing, OP, Technologist, KBM, KBE,KBMUser, KBEUser,HR, GR, OPTP, Service")]
        public ActionResult Index()
        {
            var oTK_ChaeckList = db.OTK_ChaeckList.OrderByDescending(t => t.PZ_PlanZakaz.PlanZakaz);
            string login = HttpContext.User.Identity.Name;
            int devisionId = (int)db.AspNetUsers.Where(d => d.Email == login).First().Devision;
            ViewBag.myId = devisionId;
            if (devisionId == 6)
                return View(oTK_ChaeckList.OrderByDescending(d => d.PZ_PlanZakaz.PlanZakaz).ToList());
            else
                return RedirectToAction("Index", "ReclamationOTK");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OTK_ChaeckList oTK_ChaeckList = db.OTK_ChaeckList.Find(id);

            if (oTK_ChaeckList == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderNumber = oTK_ChaeckList.PZ_PlanZakaz.PlanZakaz.ToString();

            return View(oTK_ChaeckList);
        }

        public ActionResult Create()
        {
            ViewBag.Order = new MultiSelectList(db.OTKMultiSelectList.OrderBy(d => d.PlanZakaz), "Id", "PlanZakaz");
            ViewBag.UserCreate = new MultiSelectList(db.AspNetUsers, "Id", "CiliricalName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OTK_ChaeckList oTK_ChaeckList)
        {
            var user = db.AspNetUsers;
            if (ModelState.IsValid)
            {
                string login = HttpContext.User.Identity.Name;
                oTK_ChaeckList.UserCreate = db.AspNetUsers.Where(d => d.Email == login).First().Id;
                oTK_ChaeckList.DateCreate = DateTime.Now;
                db.OTK_ChaeckList.Add(oTK_ChaeckList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Order = new SelectList(db.PZ_PlanZakaz, "Id", "MTR", oTK_ChaeckList.Order);
            return View(oTK_ChaeckList);
        }

        [HttpGet]
        public ActionResult QuaReport()
        {
            DateTime dataMin = db.PZ_PlanZakaz.Where(d => d.dataOtgruzkiBP != null & d.dataOtgruzkiBP.Year > 2017).Min(d => d.dataOtgruzkiBP);
            int minQua = (dataMin.Month + 2) / 3;
            int minYear = dataMin.Year;
            DateTime dataMax = DateTime.Now;
            int maxQua = (dataMax.Month + 2) / 3;
            int maxYear = dataMax.Year;
            List<string> list = new List<string>();
            for (int j = minYear; j <= maxYear; j++)
            {
                for (int i = minQua; i < 5; i++)
                {
                    list.Add(j.ToString() + "." + i.ToString());
                }
                minQua = 1;
            }
            List<string> devisionList = new List<string>();
            devisionList.Add("КБМ");
            devisionList.Add("КБЭ");
            devisionList.Add("Производству");
            ViewBag.Period = new SelectList(list);
            ViewBag.Devision = new SelectList(devisionList);
            return View();
        }
        [HttpPost]
        public ActionResult QuaReport(string Period, string Devision)
        {
            List<int> listDevInts = new List<int>();
            if(Devision == "КБМ")
            {
                listDevInts.Add(15);
            }
            else if (Devision == "КБЭ")
            {
                listDevInts.Add(3);
                listDevInts.Add(16);
            }
            else
            {
                listDevInts.Add(8);
                listDevInts.Add(9);
                listDevInts.Add(10);
                listDevInts.Add(20);
                listDevInts.Add(22);
            }
            int yearPeriod = Convert.ToInt32(Period.Substring(0, 4));
            int qua = Convert.ToInt32(Period.Substring(5, 1));
            List<OTK_QuartalReport> listCl = new List<OTK_QuartalReport>();
            List<OTK_ChaeckList> dataForForeach = db.OTK_ChaeckList
                .Where(d => d.PZ_PlanZakaz.dataOtgruzkiBP.Year == yearPeriod 
                            && (d.PZ_PlanZakaz.dataOtgruzkiBP.Month + 2) / 3 == qua).ToList();
            bool correct = false;
            foreach (var data in dataForForeach)
            {
                OTK_QuartalReport cortage = new OTK_QuartalReport();
                cortage.DateShip = data.PZ_PlanZakaz.dataOtgruzkiBP;
                cortage.PlanZakaz = data.PZ_PlanZakaz.PlanZakaz;
                int countBefore4 = 0;
                int countBefore8 = 0;
                int countBefore16 = 0;
                int countBeforeLarge = 0;
                int countBeforeNotClose = 0;
                int countBeforeAll = 0;
                foreach (var datalist in data.OTK_Reclamation.ToList())
                {
                    foreach (var corrDev in listDevInts)
                    {
                        if (datalist.Devision == corrDev)
                            correct = true;
                    }
                    if (correct == true)
                    {
                        int houreTime = (int)datalist.AnswerDate.Value.Subtract(datalist.DateTimeCreate).TotalHours;
                        if (datalist.Complited == false)
                        {
                            countBeforeNotClose++;
                        }
                        else if (houreTime < 5 && houreTime > 0)
                        {
                            countBefore4++;
                        }
                        else if (houreTime < 9 && houreTime > 4)
                        {
                            countBefore8++;
                        }
                        else if (houreTime < 17 && houreTime > 8)
                        {
                            countBefore16++;
                        }
                        else
                        {
                            countBeforeLarge++;
                        }
                        countBeforeAll++;
                    }
                    correct = false;
                }
                cortage.CountBefore4 = countBefore4;
                cortage.CountBefore8 = countBefore8;
                cortage.CountBefore16 = countBefore16;
                cortage.CountBeforeLarge = countBeforeLarge;
                cortage.CountBeforeNotClose = countBeforeNotClose;
                cortage.CountBeforeAll = countBeforeAll;
                cortage.ResultPercent = 100 - countBefore4 * 5 - countBefore8 * 10 -
                                        countBefore16 * 20 - countBeforeNotClose * 10;
                if (cortage.ResultPercent < 0)
                    cortage.ResultPercent = 0;
                listCl.Add(cortage);
            }
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            int rowStart = 1;
            ws.Cells[string.Format("A{0}", rowStart)].Value = "Наименование отчета: Выборка по отгруженным станциям по " + Devision + " за " + qua.ToString() + " квартал " + yearPeriod.ToString() + " года.";
            rowStart++;
            ws.Cells[string.Format("A{0}", rowStart)].Value = string.Format("Дата и время формирования отчета: {0}", DateTime.Now.ToString());
            rowStart += 2;
            ws.Cells[string.Format("A{0}", rowStart)].Value = "Дата отгрузки";
            ws.Cells[string.Format("B{0}", rowStart)].Value = "Станция №";
            ws.Cells[string.Format("C{0}", rowStart)].Value = "Кол-во замечаний устраненных до 4ч (5%)";
            ws.Cells[string.Format("D{0}", rowStart)].Value = "Кол-во замечаний устраненных от 4ч до 8ч (10%)";
            ws.Cells[string.Format("E{0}", rowStart)].Value = "Кол-во замечаний устраненных от 8ч до 16ч (20%)";
            ws.Cells[string.Format("F{0}", rowStart)].Value = "Кол-во замечаний устраненных свыше 16ч (10%)";
            ws.Cells[string.Format("G{0}", rowStart)].Value = "Не устраненные замечания (10%)";
            ws.Cells[string.Format("H{0}", rowStart)].Value = "Общее кол-во замечаний";
            ws.Cells[string.Format("I{0}", rowStart)].Value = "Оценка %";

            
            ws.Cells[string.Format("A{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("B{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("C{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("D{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("E{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("F{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("G{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("H{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            ws.Cells[string.Format("I{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);


            ws.Cells[string.Format("A{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("B{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("C{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("D{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("E{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("F{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("G{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("H{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[string.Format("I{0}", rowStart)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
            ws.Cells[string.Format("A{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("A{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            ws.Cells[string.Format("B{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("B{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            ws.Cells[string.Format("C{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("C{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            ws.Cells[string.Format("D{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("D{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            ws.Cells[string.Format("E{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("E{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            ws.Cells[string.Format("F{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("F{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            ws.Cells[string.Format("G{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("G{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            ws.Cells[string.Format("H{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("H{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            ws.Cells[string.Format("I{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[string.Format("I{0}", rowStart)].Style.Fill.BackgroundColor.SetColor(Color.LemonChiffon);
            
            rowStart++;
            bool colorBackground = false;
            foreach (var item in listCl)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.DateShip.ToString().Substring(0, 10);
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.PlanZakaz;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.CountBefore4;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.CountBefore8;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.CountBefore16;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.CountBeforeLarge;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.CountBeforeNotClose;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.CountBeforeAll;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.ResultPercent;

                ws.Cells[string.Format("A{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("B{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("C{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("D{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("E{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("F{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("G{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("H{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[string.Format("I{0}", rowStart)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                
                ws.Cells[string.Format("A{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("B{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("C{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("D{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("E{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("F{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("G{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("H{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("I{0}", rowStart)].Style
                    .HorizontalAlignment = ExcelHorizontalAlignment.Center;

                if (colorBackground == true)
                {
                    ws.Cells[string.Format("A{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("A{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    ws.Cells[string.Format("B{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("B{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    ws.Cells[string.Format("C{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("C{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    ws.Cells[string.Format("D{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("D{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    ws.Cells[string.Format("E{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("E{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    ws.Cells[string.Format("F{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("F{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    ws.Cells[string.Format("G{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("G{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    ws.Cells[string.Format("H{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("H{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    ws.Cells[string.Format("I{0}", rowStart)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("I{0}", rowStart)].Style
                        .Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                }
                rowStart++;
                if (colorBackground == true)
                    colorBackground = false;
                else
                    colorBackground = true;
            }
            ws.Cells["B:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            return RedirectToAction("Index");
        }
    }
}
