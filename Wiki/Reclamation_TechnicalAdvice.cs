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
    
    public partial class Reclamation_TechnicalAdvice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reclamation_TechnicalAdvice()
        {
            this.Reclamation_TechnicalAdviceProtocolPosition = new HashSet<Reclamation_TechnicalAdviceProtocolPosition>();
            this.Reclamation_TechnicalAdviceTasks = new HashSet<Reclamation_TechnicalAdviceTasks>();
        }
    
        public int id { get; set; }
        public int id_Reclamation { get; set; }
        public Nullable<System.DateTime> dateTimeClose { get; set; }
        public string text { get; set; }
        public string description { get; set; }
        public string id_AspNetUsersCreate { get; set; }
        public System.DateTime dateTimeCreate { get; set; }
        public string id_AspNetUserResponsible { get; set; }
        public Nullable<System.DateTime> deadline { get; set; }
        public bool close { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual Reclamation Reclamation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_TechnicalAdviceProtocolPosition> Reclamation_TechnicalAdviceProtocolPosition { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_TechnicalAdviceTasks> Reclamation_TechnicalAdviceTasks { get; set; }
    }
}
