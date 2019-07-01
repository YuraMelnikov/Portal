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
    
    public partial class ServiceRemarks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceRemarks()
        {
            this.ServiceRemarksActions = new HashSet<ServiceRemarksActions>();
            this.ServiceRemarksCauses = new HashSet<ServiceRemarksCauses>();
            this.ServiceRemarksPlanZakazs = new HashSet<ServiceRemarksPlanZakazs>();
            this.ServiceRemarksReclamations = new HashSet<ServiceRemarksReclamations>();
            this.ServiceRemarksTypes = new HashSet<ServiceRemarksTypes>();
        }
    
        public int id { get; set; }
        public string text { get; set; }
        public string description { get; set; }
        public System.DateTime dateTimeCreate { get; set; }
        public string userCreate { get; set; }
        public System.DateTime datePutToService { get; set; }
        public Nullable<System.DateTime> dateClose { get; set; }
        public string folder { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRemarksActions> ServiceRemarksActions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRemarksCauses> ServiceRemarksCauses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRemarksPlanZakazs> ServiceRemarksPlanZakazs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRemarksReclamations> ServiceRemarksReclamations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRemarksTypes> ServiceRemarksTypes { get; set; }
    }
}
