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
    
    public partial class ApproveCDQuestions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApproveCDQuestions()
        {
            this.ApproveCDQuestionCorr = new HashSet<ApproveCDQuestionCorr>();
        }
    
        public int id { get; set; }
        public int id_ApproveCDOrders { get; set; }
        public System.DateTime dateTimeCreate { get; set; }
        public string id_AspNetUsersCreate { get; set; }
        public string textQuestion { get; set; }
        public bool active { get; set; }
    
        public virtual ApproveCDOrders ApproveCDOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDQuestionCorr> ApproveCDQuestionCorr { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
