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
    
    public partial class PZ_OperatorDogovora
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PZ_OperatorDogovora()
        {
            this.PZ_PlanZakaz = new HashSet<PZ_PlanZakaz>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PZ_PlanZakaz> PZ_PlanZakaz { get; set; }
    }
}
