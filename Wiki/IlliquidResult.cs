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
    
    public partial class IlliquidResult
    {
        public int Id { get; set; }
        public int IlliquidId { get; set; }
        public int DevisionAuto { get; set; }
        public Nullable<int> DevisionCorrect { get; set; }
        public string Cause { get; set; }
        public Nullable<int> IlliquidTypeId { get; set; }
        public float Quentity { get; set; }
        public float Sum { get; set; }
        public string Note { get; set; }
    
        public virtual Devision Devision { get; set; }
        public virtual Devision Devision1 { get; set; }
        public virtual Illiquid Illiquid { get; set; }
        public virtual IlliquidType IlliquidType { get; set; }
    }
}
