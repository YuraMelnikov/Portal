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
    
    public partial class PlanVerificationItemsLog
    {
        public int id { get; set; }
        public int id_PlanVerificationItems { get; set; }
        public string id_AspNetUsers { get; set; }
        public System.DateTime date { get; set; }
        public string action { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual PlanVerificationItems PlanVerificationItems { get; set; }
    }
}
