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
    
    public partial class CMKO_BujetList
    {
        public System.Guid ProjectUID { get; set; }
        public System.Guid TaskUID { get; set; }
        public string TaskWBS { get; set; }
        public System.Guid ResourceUID { get; set; }
        public string ResourceName { get; set; }
        public string quartalFinishTask { get; set; }
        public string monthFinishTask { get; set; }
        public short TaskPercentCompleted { get; set; }
        public Nullable<int> id_PZ_PlanZakaz { get; set; }
        public string orderNumber { get; set; }
        public int counterKO { get; set; }
        public string TaskName { get; set; }
        public System.DateTime TaskFinishDate { get; set; }
        public string Devision { get; set; }
        public string DevisionKBE { get; set; }
        public double SSM { get; set; }
        public double bujetWorkers { get; set; }
        public double bujetManagers { get; set; }
        public double TaskWork { get; set; }
        public double normH { get; set; }
        public double normHOrderKBM { get; set; }
        public double normHOrderKBE { get; set; }
        public double cost1NHWorkerKBM { get; set; }
        public double cost1NHManagerKBM { get; set; }
        public double accruedWorkerForTaskKBM { get; set; }
        public double accruedManagerForTaskKBM { get; set; }
        public double cost1NHWorkerKBE { get; set; }
        public double cost1NHManagerKBE { get; set; }
        public double accruedWorkerForTaskKBE { get; set; }
        public double accruedManagerForTaskKBE { get; set; }
        public double accruedWorkerForNTaskKBM { get; set; }
        public double accruedManagerForNTaskKBM { get; set; }
        public double accruedWorkerForNTaskKBE { get; set; }
        public double accruedManagerForNTaskKBE { get; set; }
        public string id_RKD_GIP_KBM { get; set; }
        public string id_RKD_GIP_KBE { get; set; }
        public bool nHourRecorded { get; set; }
        public int id { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual AspNetUsers AspNetUsers1 { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
    }
}
