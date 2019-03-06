using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;

namespace Wiki.Models
{
    public class ExcelParserCMO : ExcelFile
    {
        private PortalKATEKEntities db = new PortalKATEKEntities();

        public ExcelParserCMO(HttpPostedFileBase fileUpload)
            : base(fileUpload)
        {
            
        }

        public void GetData()
        {
            SaveFileToServer(@"\\192.168.1.30\m$\_ЗАКАЗЫ\OrderCMO\");
            //re/FacK
            int row = 9;
            int colCost = 4;
            Excel.Worksheet sheet = null;
            foreach (Excel.Worksheet data in Workbook.Worksheets)
            {
                sheet = data;
                break;
            }
            Excel.Range range = sheet.UsedRange;
            List<TenderOffer> offerList = new List<TenderOffer>();
            int countPosition = 0;
            while (row < 1000)
            {
                TenderOffer offer = new TenderOffer();
                offer.Id_CMO_Order = Convert.ToInt32(((Excel.Range) range.Cells[row, 1]).Text);
                offer.Id_CMO_Company = 1;
                offer.Cost = Convert.ToInt32(((Excel.Range)range.Cells[row, colCost]).Text);
                offer.Duration = Convert.ToInt32(((Excel.Range)range.Cells[row, colCost + 1]).Text);
                offerList.Add(offer);
                offer = new TenderOffer();
                offer.Id_CMO_Order = Convert.ToInt32(((Excel.Range)range.Cells[row, 1]).Text);
                offer.Id_CMO_Company = 2;
                offer.Cost = Convert.ToInt32(((Excel.Range)range.Cells[row, colCost + 2]).Text);
                offer.Duration = Convert.ToInt32(((Excel.Range)range.Cells[row, colCost + 3]).Text);
                offerList.Add(offer);
                offer = new TenderOffer();
                offer.Id_CMO_Order = Convert.ToInt32(((Excel.Range)range.Cells[row, 1]).Text);
                offer.Id_CMO_Company = 3;
                offer.Cost = Convert.ToInt32(((Excel.Range)range.Cells[row, colCost + 4]).Text);
                offer.Duration = Convert.ToInt32(((Excel.Range)range.Cells[row, colCost + 5]).Text);
                offerList.Add(offer);
                row++;
                if (((Excel.Range) range.Cells[row, 1]).Text == "")
                {
                    break;
                }
            }
            int orderId = 0;
            int tenderId = 0;
            foreach (var VARIABLE in offerList)
            {
                if (orderId != VARIABLE.Id_CMO_Order)
                {
                    CMO_Tender tender = new CMO_Tender();
                    tender.id_CMO_Order = VARIABLE.Id_CMO_Order;
                    tender.id_CMO_TypeTask = 1;
                    tender.finishPlanClose = DateTime.Now;
                    tender.close = true;
                    tender.closeDateTime = DateTime.Now;
                    db.CMO_Tender.Add(tender);
                    db.SaveChanges();
                    tenderId = tender.id;
                    orderId = tender.id_CMO_Order;
                    CMO_Order order = db.CMO_Order.Find(orderId);
                    order.firstTenderStart = true;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    countPosition++;
                }
                CMO_UploadResult result = new CMO_UploadResult();
                result.id_CMO_Tender = tenderId;
                result.dateTimeUpload = DateTime.Now;
                result.cost = VARIABLE.Cost;
                result.dateComplited = DateTime.Now;
                result.id_CMO_Company = VARIABLE.Id_CMO_Company;
                result.day = VARIABLE.Duration;
                db.CMO_UploadResult.Add(result);
                db.SaveChanges();
            }
            row += 4;
            offerList = new List<TenderOffer>();
            for (int i = 0; i < countPosition; i++)
            {
                TenderOffer offer = new TenderOffer();
                offer.Id_CMO_Order = Convert.ToInt32(((Excel.Range)range.Cells[row, 1]).Text);
                offer.Id_CMO_Company = GetIdCompany(((Excel.Range)range.Cells[row, 4]).Text);
                offer.Cost = Convert.ToInt32(((Excel.Range)range.Cells[row, 6]).Text);
                offer.Duration = Convert.ToInt32(((Excel.Range)range.Cells[row, 8]).Text);
                offerList.Add(offer);
                row++;
            }
            foreach (var VARIABLE in offerList)
            {
                CMO_Tender tender = new CMO_Tender();
                tender.id_CMO_Order = VARIABLE.Id_CMO_Order;
                tender.id_CMO_TypeTask = 3;
                tender.finishPlanClose = DateTime.Now;
                tender.close = true;
                tender.closeDateTime = DateTime.Now;
                db.CMO_Tender.Add(tender);
                db.SaveChanges();

                CMO_UploadResult result = new CMO_UploadResult();
                result.id_CMO_Tender = tender.id;
                result.dateTimeUpload = DateTime.Now;
                result.cost = VARIABLE.Cost;
                result.dateComplited = DateTime.Now;
                result.id_CMO_Company = VARIABLE.Id_CMO_Company;
                result.day = VARIABLE.Duration;
                db.CMO_UploadResult.Add(result);
                db.SaveChanges();

                CMO_Order order = db.CMO_Order.Find(tender.id_CMO_Order);
                order.companyWin = result.id_CMO_Company;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            CloseWorkbook();
        }

        protected override string GetPlusTextForDictionarySaveFileName()
        {
            return DateTime.Now.ToString() + "_";
        }

        private int GetIdCompany(string companyName)
        {
            companyName += "\r\n";
            var list = db.CMO_Company.ToList();
            return db.CMO_Company.First(d => d.name == companyName).id;
        }
    }
}