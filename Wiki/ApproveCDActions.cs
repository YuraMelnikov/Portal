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
    
    public partial class ApproveCDActions
    {
        public int id { get; set; }
        public int id_ApproveCDVersions { get; set; }
        public int id_RKD_VersionWork { get; set; }
        public string id_AspNetUsers { get; set; }
        public System.DateTime datetime { get; set; }
        public double counterKBM { get; set; }
        public double counterKBE { get; set; }
    
        public virtual ApproveCDVersions ApproveCDVersions { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual TypeRKD_Mail_Version TypeRKD_Mail_Version { get; set; }
    }
}
