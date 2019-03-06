using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Models
{
    public class COM_UploadResultColor : CMO_UploadResult
    {
        private bool colorStep;

        public bool ColorStep
        {
            get { return colorStep; }
            set { colorStep = value; }
        }

        public COM_UploadResultColor(CMO_UploadResult data)
        {
            colorStep = false;

        }
    }
}