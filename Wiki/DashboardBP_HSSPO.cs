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
    
    public partial class DashboardBP_HSSPO
    {
        public int id { get; set; }
        public int id_DashboardBP_State { get; set; }
        public int id_PZ_PlanZakaz { get; set; }
        public System.Guid taskUID { get; set; }
        public string taskName { get; set; }
        public System.DateTime timeByDay { get; set; }
        public double assignmentWork { get; set; }
        public double rateUSD { get; set; }
        public double ssmPlan { get; set; }
        public double ssmFact { get; set; }
        public double ssrPlan { get; set; }
        public double ik { get; set; }
        public double ppk { get; set; }
        public double pi { get; set; }
        public double nop { get; set; }
        public double rateHoure { get; set; }
        public double ssmPlanHoure { get; set; }
        public double ssmFactHoure { get; set; }
        public double ssrHoure { get; set; }
        public double ikHoure { get; set; }
        public double ppkHoure { get; set; }
        public double piHoure { get; set; }
        public double nopHoure { get; set; }
        public double xReate { get; set; }
        public double xSsm { get; set; }
        public double xSsmFact { get; set; }
        public double xSsr { get; set; }
        public double xIk { get; set; }
        public double xPpk { get; set; }
        public double xPi { get; set; }
        public double xNop { get; set; }
    
        public virtual DashboardBP_State DashboardBP_State { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
    }
}
