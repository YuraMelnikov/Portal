using System.Collections.Generic;

namespace Wiki.Models
{
    public class ReclamationPO
    {
        public List<OTK_ReclamationKO> ActiveList { get; set; }
        public List<OTK_ReclamationKO> DeActiveList { get; set; }
    }
}