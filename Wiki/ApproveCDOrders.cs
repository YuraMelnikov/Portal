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