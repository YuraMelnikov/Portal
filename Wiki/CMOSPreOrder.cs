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
