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
    
    public partial class CMOSPreOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMOSPreOrder()
        {
            this.CMOSOrderPreOrder = new HashSet<CMOSOrderPreOrder>();
            this.CMOSPositionPreOrder = new HashSet<CMOSPositionPreOrder>();
        }
    
        public int id { get; set; }
        public int id_PZ_PlanZakaz { get; set; }
        public string id_AspNetUsersCreate { get; set; }
        public int id_CMO_TypeProduct { get; set; }
        public System.DateTime dateTimeCreate { get; set; }
        public bool reOrder { get; set; }
        public string folder { get; set; }
        public bool remove { get; set; }
        public string note { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual CMO_TypeProduct CMO_TypeProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMOSOrderPreOrder> CMOSOrderPreOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMOSPositionPreOrder> CMOSPositionPreOrder { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
    }
}
