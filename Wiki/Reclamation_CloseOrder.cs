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
    
    public partial class Reclamation_CloseOrder
    {
        public int id { get; set; }
        public string userClose { get; set; }
        public System.DateTime dateTimeClose { get; set; }
        public string description { get; set; }
        public int id_PZ_PlanZakaz { get; set; }
        public bool close { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
    }
}
