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
    
    public partial class DashboardBP_TasksList
    {
        public int id { get; set; }
        public int id_ProjectTask { get; set; }
        public int id_DashboardBP_ProjectList { get; set; }
        public System.DateTime startDate { get; set; }
        public System.DateTime finishDate { get; set; }
        public System.DateTime deadline { get; set; }
        public System.DateTime bStartDate { get; set; }
        public System.DateTime bFinishDate { get; set; }
        public double duration { get; set; }
        public double bDuration { get; set; }
        public double work { get; set; }
        public double bWork { get; set; }
        public double actualWork { get; set; }
        public double remainingWork { get; set; }
        public double percentCompleted { get; set; }
        public double percentWorkCompleted { get; set; }
        public bool isCritical { get; set; }
        public int priority { get; set; }
        public string id_AspNetUsers { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual DashboardBP_ProjectList DashboardBP_ProjectList { get; set; }
        public virtual ProjectTask ProjectTask { get; set; }
    }
}
