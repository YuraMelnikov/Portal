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
    
    public partial class ApproveCDVersions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApproveCDVersions()
        {
            this.ApproveCDActions = new HashSet<ApproveCDActions>();
        }
    
        public int id { get; set; }
        public int id_ApproveCDOrders { get; set; }
        public int id_RKD_VersionWork { get; set; }
        public short numberVersion1 { get; set; }
        public short numberVersion2 { get; set; }
        public bool activeVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDActions> ApproveCDActions { get; set; }
        public virtual ApproveCDOrders ApproveCDOrders { get; set; }
        public virtual RKD_VersionWork RKD_VersionWork { get; set; }
    }
}
