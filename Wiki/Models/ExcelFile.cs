using System;
using System.IO;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;

namespace Wiki.Models
{
    public class ExcelFile
    {
        protected HttpPostedFileBase fileUpload;

        public ExcelFile(HttpPostedFileBase fileUpload)
        {
            this.fileUpload = fileUpload;
        }

        public string DictionarySave { get; set; }
        public Excel.Workbook Workbook { get; set; } = null;
        public Excel.Application Application { get; set; } = new Excel.Application();

        public void SaveFileToServer(string dictionaryToSaveFile)
        {
            Dictionaries dictionaries = new Dictionaries(dictionaryToSaveFile, GetPlusTextForDictionarySaveFileName() + Path.GetFileName(fileUpload.FileName));
            fileUpload.SaveAs(dictionaries.DictionaryString);
            DictionarySave = dictionaries.DictionaryString;
            Workbook = Application.Workbooks.Open(DictionarySave);
        }

        public void CloseWorkbook()
        {
            Workbook.Close();
            Application.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(Application);
            Application = null;
            Workbook = null;
            GC.Collect();
        }

        protected virtual string GetPlusTextForDictionarySaveFileName()
        {
            return "";
        }
    }
}