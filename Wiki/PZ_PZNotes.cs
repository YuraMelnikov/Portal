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
    
    public partial class PZ_PZNotes
    {
        public int id { get; set; }
        public int id_PZ_PlanZakaz { get; set; }
        public Nullable<int> id_PZ_Notes { get; set; }
    
        public virtual PZ_Notes PZ_Notes { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
    }
}
