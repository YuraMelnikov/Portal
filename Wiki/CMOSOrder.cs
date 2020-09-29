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
    
    public partial class CMOSOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMOSOrder()
        {
            this.CMOSOrderPreOrder = new HashSet<CMOSOrderPreOrder>();
            this.CMOSPositionOrder = new HashSet<CMOSPositionOrder>();
        }
    
        public int id { get; set; }
        public string aspNetUsersCreateId { get; set; }
        public System.DateTime dateTimeCreate { get; set; }
        public Nullable<System.DateTime> workDate { get; set; }
        public Nullable<System.DateTime> manufDate { get; set; }
        public Nullable<System.DateTime> finDate { get; set; }
        public string folder { get; set; }
        public int cMO_CompanyId { get; set; }
        public string numberTN { get; set; }
        public double cost { get; set; }
        public Nullable<double> factCost { get; set; }
        public double weight { get; set; }
        public bool remove { get; set; }
        public double rate { get; set; }
        public double curency { get; set; }
        public Nullable<System.DateTime> dateTN { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual CMO_Company CMO_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMOSOrderPreOrder> CMOSOrderPreOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMOSPositionOrder> CMOSPositionOrder { get; set; }
    }
}
