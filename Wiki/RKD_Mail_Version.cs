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
    
    public partial class RKD_Mail_Version
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RKD_Mail_Version()
        {
            this.RKD_FileMailVersion = new HashSet<RKD_FileMailVersion>();
            this.RKD_Mail_TimeForComplited = new HashSet<RKD_Mail_TimeForComplited>();
        }
    
        public int id { get; set; }
        public int id_RKD_Version { get; set; }
        public int id_TypeRKD_Mail_Version { get; set; }
        public System.DateTime dateTimeUpload { get; set; }
        public string id_AspNetUser_Upload { get; set; }
        public string linkFile { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_FileMailVersion> RKD_FileMailVersion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_Mail_TimeForComplited> RKD_Mail_TimeForComplited { get; set; }
        public virtual RKD_Version RKD_Version { get; set; }
        public virtual TypeRKD_Mail_Version TypeRKD_Mail_Version { get; set; }
    }
}
