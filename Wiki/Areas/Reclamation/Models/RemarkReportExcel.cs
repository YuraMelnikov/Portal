using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Areas.Reclamation.Models
{
    public class RemarkReportExcel
    {
        bool allReport;
        bool myDevisionReport;
        bool myRemarkReport;
        bool myReport;

        public bool AllReport { get => allReport; set => allReport = value; }
        public bool MyDevisionReport { get => myDevisionReport; set => myDevisionReport = value; }
        public bool MyRemarkReport { get => myRemarkReport; set => myRemarkReport = value; }
        public bool MyReport { get => myReport; set => myReport = value; }
    }
}