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
    
    public partial class CMO_Report
    {
        public int Id_Order { get; set; }
        public string PositionOrder { get; set; }
        public int Company_IdSort { get; set; }
        public string CompanyName { get; set; }
        public System.DateTime DateCreate_Order { get; set; }
        public string UserCreate { get; set; }
        public Nullable<double> Cost_FT { get; set; }
        public Nullable<int> Day_FT { get; set; }
        public Nullable<double> Cost_ST { get; set; }
        public Nullable<int> Day_ST { get; set; }
        public Nullable<double> Cost_WT { get; set; }
        public Nullable<int> Day_WT { get; set; }
        public Nullable<System.DateTime> DateStartWork { get; set; }
        public Nullable<System.DateTime> PlanFinishWork1 { get; set; }
        public Nullable<System.DateTime> PlanFinishWork2 { get; set; }
        public Nullable<System.DateTime> FactFinishDate { get; set; }
        public string folder { get; set; }
    }
}
