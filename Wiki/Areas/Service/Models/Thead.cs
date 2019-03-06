using System.Collections.Generic;

namespace Wiki.Areas.Service.Models
{
    public class Thead : TheadBasicNames
    {
        protected List<string> columnList;

        public Thead()
        {
            columnList = new List<string>();
        }

        public List<string> ColumnList { get => columnList; }
    }
}