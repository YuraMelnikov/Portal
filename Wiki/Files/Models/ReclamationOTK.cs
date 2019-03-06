using System.Collections.Generic;

namespace Wiki.Models
{
    public class ReclamationOTK
    {
        public List<OTK_ReclamationAnswer> ActiveList { get; set; }
        public List<OTK_ReclamationAnswer> DeActiveList { get; set; }
    }
}