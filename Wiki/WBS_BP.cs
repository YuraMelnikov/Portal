//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wiki
{
    using System;
    using System.Collections.Generic;
    
    public partial class WBS_BP
    {
        public int id { get; set; }
        public int id_PZ_PlanZakaz { get; set; }
        public int id_WBS { get; set; }
        public double duration { get; set; }
        public System.DateTime start { get; set; }
        public System.DateTime finish { get; set; }
        public double work { get; set; }
        public double percentComplite { get; set; }
        public double percentWorkComplite { get; set; }
    }
}
