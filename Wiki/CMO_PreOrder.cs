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
    
    public partial class CMO_PreOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CMO_PreOrder()
        {
            this.CMO_PositionPreOrder = new HashSet<CMO_PositionPreOrder>();
        }
    
        public int id { get; set; }
        public System.DateTime dateCreate { get; set; }
        public string userCreate { get; set; }
        public string folder { get; set; }
        public bool firstTenderStart { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMO_PositionPreOrder> CMO_PositionPreOrder { get; set; }
    }
}
