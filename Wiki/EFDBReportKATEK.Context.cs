﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ReportKATEKEntities : DbContext
    {
        public ReportKATEKEntities()
            : base("name=ReportKATEKEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BasicPlan> BasicPlan { get; set; }
        public virtual DbSet<DashboardCountPZInDevision> DashboardCountPZInDevision { get; set; }
        public virtual DbSet<DashboardDH> DashboardDH { get; set; }
        public virtual DbSet<DashboardDHCustomer> DashboardDHCustomer { get; set; }
        public virtual DbSet<DashboardDN> DashboardDN { get; set; }
        public virtual DbSet<DashboardDPercent> DashboardDPercent { get; set; }
        public virtual DbSet<DashboardProjectLiast> DashboardProjectLiast { get; set; }
        public virtual DbSet<DashboardRateToDateShipNextYear> DashboardRateToDateShipNextYear { get; set; }
        public virtual DbSet<DashboardRateToDateShipThisYear> DashboardRateToDateShipThisYear { get; set; }
        public virtual DbSet<DashboardRemainingWorkDevision> DashboardRemainingWorkDevision { get; set; }
        public virtual DbSet<DashboardYearsSpidimetrs> DashboardYearsSpidimetrs { get; set; }
        public virtual DbSet<DateActualReport> DateActualReport { get; set; }
        public virtual DbSet<FactCostsER> FactCostsER { get; set; }
        public virtual DbSet<FilterYearReport> FilterYearReport { get; set; }
        public virtual DbSet<FinCostsClient> FinCostsClient { get; set; }
        public virtual DbSet<FinPercentSredniyVzveshenniyNOP> FinPercentSredniyVzveshenniyNOP { get; set; }
        public virtual DbSet<ManpowerPO> ManpowerPO { get; set; }
        public virtual DbSet<Month> Month { get; set; }
        public virtual DbSet<PercentSredniyVzveshenniyNOP> PercentSredniyVzveshenniyNOP { get; set; }
        public virtual DbSet<PlanFactCosts> PlanFactCosts { get; set; }
        public virtual DbSet<PlanFactCostsLastYear> PlanFactCostsLastYear { get; set; }
        public virtual DbSet<PlanFactCostsThisYear> PlanFactCostsThisYear { get; set; }
        public virtual DbSet<ProjectList> ProjectList { get; set; }
        public virtual DbSet<ReeportStatusPlanZakaz> ReeportStatusPlanZakaz { get; set; }
        public virtual DbSet<ReportStatusPlanZakazAndWBS> ReportStatusPlanZakazAndWBS { get; set; }
        public virtual DbSet<ResultReeportStatusPlanZakaz> ResultReeportStatusPlanZakaz { get; set; }
        public virtual DbSet<Sklad> Sklad { get; set; }
        public virtual DbSet<SVNLast120> SVNLast120 { get; set; }
        public virtual DbSet<TEOInWorkKBE> TEOInWorkKBE { get; set; }
        public virtual DbSet<TEOInWorkKBM> TEOInWorkKBM { get; set; }
        public virtual DbSet<TEOInWorkPO> TEOInWorkPO { get; set; }
        public virtual DbSet<TEOInWorkPOHist> TEOInWorkPOHist { get; set; }
        public virtual DbSet<TVManuf_DateReport> TVManuf_DateReport { get; set; }
        public virtual DbSet<WBS> WBS { get; set; }
        public virtual DbSet<Week> Week { get; set; }
        public virtual DbSet<ResultGraphicStatusPlanZakaz> ResultGraphicStatusPlanZakaz { get; set; }
        public virtual DbSet<forT3> forT3 { get; set; }
        public virtual DbSet<NoPlaningPZ> NoPlaningPZ { get; set; }
        public virtual DbSet<NOPOpenOrderLastTheerYears> NOPOpenOrderLastTheerYears { get; set; }
        public virtual DbSet<PostControlRazreshenieProizvodstva> PostControlRazreshenieProizvodstva { get; set; }
        public virtual DbSet<RKD> RKD { get; set; }
        public virtual DbSet<RKD_DateAction> RKD_DateAction { get; set; }
        public virtual DbSet<TEOInWorkPOGroupBy> TEOInWorkPOGroupBy { get; set; }
    }
}
