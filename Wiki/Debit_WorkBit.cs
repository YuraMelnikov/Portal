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
    
    public partial class Debit_WorkBit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Debit_WorkBit()
        {
            this.Debit_CMR = new HashSet<Debit_CMR>();
            this.Debit_DataReportOprih = new HashSet<Debit_DataReportOprih>();
            this.Debit_IstPost = new HashSet<Debit_IstPost>();
            this.Debit_TN = new HashSet<Debit_TN>();
            this.PostAlertShip = new HashSet<PostAlertShip>();
        }
    
        public int id { get; set; }
        public int id_TaskForPZ { get; set; }
        public System.DateTime dateCreate { get; set; }
        public System.DateTime datePlanFirst { get; set; }
        public Nullable<System.DateTime> dateClose { get; set; }
        public bool close { get; set; }
        public int id_PlanZakaz { get; set; }
        public System.DateTime datePlan { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_CMR> Debit_CMR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_DataReportOprih> Debit_DataReportOprih { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_IstPost> Debit_IstPost { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_TN> Debit_TN { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
        public virtual TaskForPZ TaskForPZ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostAlertShip> PostAlertShip { get; set; }
    }
}
