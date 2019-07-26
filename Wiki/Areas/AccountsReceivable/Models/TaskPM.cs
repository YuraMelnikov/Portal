using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wiki.Areas.AccountsReceivable.Models
{
    public class TaskPM
    {
        public string pmOrderPZName;
        public int idPZ;
        public int ProductType;
        public string powerST;
        public string vnnn;
        public string gbb;
        public string orderRegist;
        public string teoRegist;
        public string planKBM;
        public string planKBE;
        public string prototypeKBM;
        public string prototypeKBE;
        public string prototypeKBMComplited;
        public string prototypeKBEComplited;
        public string contractComplited;
        public string mailManuf;
        public string mailSh;
        public string mailShComplited;

        public TaskPM(int id)
        {
            using (PortalKATEKEntities db = new PortalKATEKEntities())
            {

            }
        }
    }
}