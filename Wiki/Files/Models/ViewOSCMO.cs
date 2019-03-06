using System.Collections.Generic;

namespace Wiki.Models
{
    public class ViewOSCMO
    {
        public List<CMO_Order> ActiveOrder { get; set; }
        public List<CMO_UploadResult> ActiveFirstUpload { get; set; }
        public List<CMO_Tender> ActiveFirstTender { get; set; }
        public List<CMO_UploadResult> ActiveSecondUpload { get; set; }
        public List<CMO_Tender> ActiveSecondTender { get; set; }
        public List<CMO_Order> ActiveOrderClose { get; set; }
    }
}