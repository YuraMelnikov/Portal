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
    
    public partial class CMKO_ThisCoefManager
    {
        public int id { get; set; }
        public string id_CMKO_PeriodResult { get; set; }
        public string id_AspNetUsers { get; set; }
        public double coef { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual CMKO_PeriodResult CMKO_PeriodResult { get; set; }
    }
}
