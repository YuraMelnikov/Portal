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
    
    public partial class IlliquidSN
    {
        public int Id { get; set; }
        public int SKUId { get; set; }
        public string Number { get; set; }
        public System.DateTime Date { get; set; }
        public float Quentity { get; set; }
        public string Devision { get; set; }
    
        public virtual SKU SKU { get; set; }
    }
}