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
    
    public partial class Debit_PeriodReportOprih
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Debit_PeriodReportOprih()
        {
            this.Debit_DataReportOprih = new HashSet<Debit_DataReportOprih>();
        }
    
        public int id { get; set; }
        public string period { get; set; }
        public System.DateTime dateTimeCreate { get; set; }
        public Nullable<System.DateTime> dateTimeClose { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_DataReportOprih> Debit_DataReportOprih { get; set; }
    }
}
