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
    
    public partial class Reclamation_TechnicalAdviceProtocol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reclamation_TechnicalAdviceProtocol()
        {
            this.Reclamation_TechnicalAdviceProtocolPosition = new HashSet<Reclamation_TechnicalAdviceProtocolPosition>();
        }
    
        public int id { get; set; }
        public System.DateTime date { get; set; }
        public int number { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_TechnicalAdviceProtocolPosition> Reclamation_TechnicalAdviceProtocolPosition { get; set; }
    }
}
