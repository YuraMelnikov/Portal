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
    
    public partial class DashboardBPTaskInsert
    {
        public int id_PZ_PlanZakaz { get; set; }
        public string productName { get; set; }
        public bool TaskIsSummary { get; set; }
        public short TaskOutlineLevel { get; set; }
        public string TaskOutlineNumber { get; set; }
        public string TaskWBS { get; set; }
        public string TaskWBS1 { get; set; }
        public string TaskWBS2 { get; set; }
        public string TaskWBS3 { get; set; }
        public string TaskWBS4 { get; set; }
        public string TaskName { get; set; }
        public string id_AspNetUsers { get; set; }
        public System.DateTime TaskStartDate { get; set; }
        public Nullable<System.DateTime> TaskBaseline0StartDate { get; set; }
        public System.DateTime TaskfinishDate { get; set; }
        public Nullable<System.DateTime> TaskBaseline0FinishDate { get; set; }
        public int TaskClientUniqueId { get; set; }
        public short TaskPriority { get; set; }
        public short TaskPercentWorkCompleted { get; set; }
        public short TaskPercentCompleted { get; set; }
        public Nullable<double> TaskWork { get; set; }
        public Nullable<double> TaskRemainingWork { get; set; }
        public Nullable<double> TaskBaseline0Work { get; set; }
        public Nullable<double> TaskDuration { get; set; }
        public int id { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
    }
}
