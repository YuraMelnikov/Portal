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
    
    public partial class Reclamation_TechnicalAdvice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reclamation_TechnicalAdvice()
        {
            this.Reclamation_TechnicalAdviceProtocolPosition = new HashSet<Reclamation_TechnicalAdviceProtocolPosition>();
        }
    
        public int id { get; set; }
        public int id_Reclamation { get; set; }
        public Nullable<System.DateTime> dateTimeClose { get; set; }
        public string text { get; set; }
        public string description { get; set; }
        public string id_AspNetUsersCreate { get; set; }
        public System.DateTime dateTimeCreate { get; set; }
        public string id_AspNetUsersCorrect { get; set; }
        public Nullable<System.DateTime> dateCorrect { get; set; }
    
        public virtual Reclamation Reclamation { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_TechnicalAdviceProtocolPosition> Reclamation_TechnicalAdviceProtocolPosition { get; set; }
    }
}
