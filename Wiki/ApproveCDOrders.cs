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
    
    public partial class ApproveCDOrders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApproveCDOrders()
        {
            this.ApproveCDQuestions = new HashSet<ApproveCDQuestions>();
            this.ApproveCDTasks = new HashSet<ApproveCDTasks>();
            this.ApproveCDVersions = new HashSet<ApproveCDVersions>();
        }
    
        public int id { get; set; }
        public int id_PZ_PlanZakaz { get; set; }
        public string id_AspNetUsersM { get; set; }
        public string id_AspNetUsersE { get; set; }
        public string description { get; set; }
        public bool remove { get; set; }
        public bool gHand { get; set; }
        public bool isOpening { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDQuestions> ApproveCDQuestions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDTasks> ApproveCDTasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDVersions> ApproveCDVersions { get; set; }
    }
}
