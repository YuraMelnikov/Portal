//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wiki
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceRemarksPlanZakazs
    {
        public int id { get; set; }
        public int id_PZ_PlanZakaz { get; set; }
        public int id_ServiceRemarks { get; set; }
    
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
        public virtual ServiceRemarks ServiceRemarks { get; set; }
    }
}
