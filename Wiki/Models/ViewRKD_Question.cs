using System.Collections.Generic;

namespace Wiki.Models
{
    public class ViewRKD_Question
    {
        public List<RKD_Question> ActiveQuestion { get; set; }
        public List<RKD_Question> CloseQuestion { get; set; }
    }
}