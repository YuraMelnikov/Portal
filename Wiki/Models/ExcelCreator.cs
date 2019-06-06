using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Wiki.Models
{
    public class ExcelCreator : Controller
    {
        List<ExcelRow> excelRow;
        HttpContext context;
        public ExcelCreator(List<ExcelRow> excelRow, HttpContext context)
        {
            this.context = context;
            this.excelRow = excelRow;
        }

        public void GetReport()
        {
            ExcelColumn excelColumnIndex = new ExcelColumn();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            int row = 1;
            int step = 0;
            int countColumn = excelRow[0].CountData;
            foreach (var data in excelRow)
            {
                for (int i = 0; i < countColumn; i++)
                {
                    ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], row)].Value = data.GetData(step);
                    ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], row)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    step++;
                }
                row++;
                step = 0;
            }
            for (int i = 0; i < countColumn; i++)
            {
                ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], 1)].AutoFitColumns();
                ws.Cells[string.Format("{0}{1}", excelColumnIndex.ColumnsArray[i], 1)].AutoFilter = true;
            }
            context.Response.Clear();
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("content-daisposition", "attachment: filename=" + "ExcelReport.xlsx");
            context.Response.BinaryWrite(pck.GetAsByteArray());
            context.Response.End();
        }
    }
}