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
    
    public partial class StickersPreOrder
    {
        public int id { get; set; }
        public System.DateTime datetimeCreate { get; set; }
        public string id_AspNetUsersCreate { get; set; }
        public System.DateTime deadline { get; set; }
        public string description { get; set; }
        public Nullable<int> id_PZ_PlanZakaz { get; set; }
        public Nullable<int> id_StickersOrder { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
        public virtual StickersOrder StickersOrder { get; set; }
    }
}
