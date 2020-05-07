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
    
    public partial class PortalKATEKEntities : DbContext
    {
        public PortalKATEKEntities()
            : base("name=PortalKATEKEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Calendar> Calendar { get; set; }
        public virtual DbSet<CMKO_CounterError> CMKO_CounterError { get; set; }
        public virtual DbSet<CMKO_ThisDeductionsBonusFund> CMKO_ThisDeductionsBonusFund { get; set; }
        public virtual DbSet<CMKO_ThisFinalBonus> CMKO_ThisFinalBonus { get; set; }
        public virtual DbSet<CMKO_ThisHSS> CMKO_ThisHSS { get; set; }
        public virtual DbSet<CMKO_ThisPeriod> CMKO_ThisPeriod { get; set; }
        public virtual DbSet<CMKO_ThisWageFund> CMKO_ThisWageFund { get; set; }
        public virtual DbSet<CMKO_ThisWithheldToBonusFund> CMKO_ThisWithheldToBonusFund { get; set; }
        public virtual DbSet<CurencyBYN> CurencyBYN { get; set; }
        public virtual DbSet<DashboardBP_HSSPOSmall> DashboardBP_HSSPOSmall { get; set; }
        public virtual DbSet<DashboardBPComments> DashboardBPComments { get; set; }
        public virtual DbSet<DashboardHSSPlan> DashboardHSSPlan { get; set; }
        public virtual DbSet<DashboardKO_Quartal> DashboardKO_Quartal { get; set; }
        public virtual DbSet<DashboardKOHssPO> DashboardKOHssPO { get; set; }
        public virtual DbSet<DashboardKOKBHss> DashboardKOKBHss { get; set; }
        public virtual DbSet<DashboardKOM1> DashboardKOM1 { get; set; }
        public virtual DbSet<DashboardKOM2> DashboardKOM2 { get; set; }
        public virtual DbSet<DashboardKOM3> DashboardKOM3 { get; set; }
        public virtual DbSet<DashboardKOMP1> DashboardKOMP1 { get; set; }
        public virtual DbSet<DashboardKOMP2> DashboardKOMP2 { get; set; }
        public virtual DbSet<DashboardKOMP3> DashboardKOMP3 { get; set; }
        public virtual DbSet<DashboardKOQuaHumen> DashboardKOQuaHumen { get; set; }
        public virtual DbSet<DashboardKOQuartal> DashboardKOQuartal { get; set; }
        public virtual DbSet<DashboardKORemainingWork> DashboardKORemainingWork { get; set; }
        public virtual DbSet<DashboardKORemainingWorkAll> DashboardKORemainingWorkAll { get; set; }
        public virtual DbSet<DashboardKOTimesheet> DashboardKOTimesheet { get; set; }
        public virtual DbSet<DashboardRatePlan> DashboardRatePlan { get; set; }
        public virtual DbSet<DashboardRemaining> DashboardRemaining { get; set; }
        public virtual DbSet<DashboardTV_BasicPlanData> DashboardTV_BasicPlanData { get; set; }
        public virtual DbSet<DashboardTV_DataForProjectPortfolio> DashboardTV_DataForProjectPortfolio { get; set; }
        public virtual DbSet<DashboardTV_MonthPlan> DashboardTV_MonthPlan { get; set; }
        public virtual DbSet<Debit_EmailList> Debit_EmailList { get; set; }
        public virtual DbSet<Debit_MatchingType> Debit_MatchingType { get; set; }
        public virtual DbSet<Folder> Folder { get; set; }
        public virtual DbSet<FolderDocument> FolderDocument { get; set; }
        public virtual DbSet<PostMatching> PostMatching { get; set; }
        public virtual DbSet<ProjectServer_CreateTasks> ProjectServer_CreateTasks { get; set; }
        public virtual DbSet<ProjectServer_UpdateMustStartOn> ProjectServer_UpdateMustStartOn { get; set; }
        public virtual DbSet<ProjectServer_UpdateTasks> ProjectServer_UpdateTasks { get; set; }
        public virtual DbSet<RenameTasksKBM> RenameTasksKBM { get; set; }
        public virtual DbSet<RKD_FailBPlan> RKD_FailBPlan { get; set; }
        public virtual DbSet<RKD_HistoryTaskVersion> RKD_HistoryTaskVersion { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UserPO> UserPO { get; set; }
        public virtual DbSet<WBS_BP> WBS_BP { get; set; }
        public virtual DbSet<WBS_ClassicStruct> WBS_ClassicStruct { get; set; }
        public virtual DbSet<XTEO> XTEO { get; set; }
        public virtual DbSet<CMO_Report> CMO_Report { get; set; }
        public virtual DbSet<PWA_EmpProject> PWA_EmpProject { get; set; }
        public virtual DbSet<PWA_EmpTask> PWA_EmpTask { get; set; }
        public virtual DbSet<PWA_TasksForBP> PWA_TasksForBP { get; set; }
        public virtual DbSet<PWA_EmpTaskAll> PWA_EmpTaskAll { get; set; }
        public virtual DbSet<RKD_TaskUIDProjectUD> RKD_TaskUIDProjectUD { get; set; }
        public virtual DbSet<CMKO_RemainingWork> CMKO_RemainingWork { get; set; }
        public virtual DbSet<ApproveCDActions> ApproveCDActions { get; set; }
        public virtual DbSet<ApproveCDOrders> ApproveCDOrders { get; set; }
        public virtual DbSet<ApproveCDQuestionCorr> ApproveCDQuestionCorr { get; set; }
        public virtual DbSet<ApproveCDQuestions> ApproveCDQuestions { get; set; }
        public virtual DbSet<ApproveCDTasks> ApproveCDTasks { get; set; }
        public virtual DbSet<ApproveCDVersions> ApproveCDVersions { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<BurnDown> BurnDown { get; set; }
        public virtual DbSet<CMKO_BujetList> CMKO_BujetList { get; set; }
        public virtual DbSet<CMKO_Optimization> CMKO_Optimization { get; set; }
        public virtual DbSet<CMKO_PeriodResult> CMKO_PeriodResult { get; set; }
        public virtual DbSet<CMKO_ProjectFactBujet> CMKO_ProjectFactBujet { get; set; }
        public virtual DbSet<CMKO_RemarksList> CMKO_RemarksList { get; set; }
        public virtual DbSet<CMKO_RemarksListG> CMKO_RemarksListG { get; set; }
        public virtual DbSet<CMKO_SummaryResultToMonth> CMKO_SummaryResultToMonth { get; set; }
        public virtual DbSet<CMKO_TaxCatigories> CMKO_TaxCatigories { get; set; }
        public virtual DbSet<CMKO_Teach> CMKO_Teach { get; set; }
        public virtual DbSet<CMKO_ThisAccrued> CMKO_ThisAccrued { get; set; }
        public virtual DbSet<CMKO_ThisAccruedG> CMKO_ThisAccruedG { get; set; }
        public virtual DbSet<CMKO_ThisCoefManager> CMKO_ThisCoefManager { get; set; }
        public virtual DbSet<CMKO_ThisIndicatorsUsers> CMKO_ThisIndicatorsUsers { get; set; }
        public virtual DbSet<CMKO_ThisOverflowsBujet> CMKO_ThisOverflowsBujet { get; set; }
        public virtual DbSet<CMO_Company> CMO_Company { get; set; }
        public virtual DbSet<CMO_CompanyMailList> CMO_CompanyMailList { get; set; }
        public virtual DbSet<CMO_FileOrder> CMO_FileOrder { get; set; }
        public virtual DbSet<CMO_HoureTender> CMO_HoureTender { get; set; }
        public virtual DbSet<CMO_Mail> CMO_Mail { get; set; }
        public virtual DbSet<CMO_Order> CMO_Order { get; set; }
        public virtual DbSet<CMO_PositionOrder> CMO_PositionOrder { get; set; }
        public virtual DbSet<CMO_PositionPreOrder> CMO_PositionPreOrder { get; set; }
        public virtual DbSet<CMO_PreOrder> CMO_PreOrder { get; set; }
        public virtual DbSet<CMO_Tender> CMO_Tender { get; set; }
        public virtual DbSet<CMO_TypeProduct> CMO_TypeProduct { get; set; }
        public virtual DbSet<CMO_TypeTask> CMO_TypeTask { get; set; }
        public virtual DbSet<CMO_UploadResult> CMO_UploadResult { get; set; }
        public virtual DbSet<CMO2_Order> CMO2_Order { get; set; }
        public virtual DbSet<CMO2_Position> CMO2_Position { get; set; }
        public virtual DbSet<DashboardBP_HSSPO> DashboardBP_HSSPO { get; set; }
        public virtual DbSet<DashboardBP_ProjectList> DashboardBP_ProjectList { get; set; }
        public virtual DbSet<DashboardBP_ProjectTasks> DashboardBP_ProjectTasks { get; set; }
        public virtual DbSet<DashboardBP_State> DashboardBP_State { get; set; }
        public virtual DbSet<DashboardBPDevisionCoef> DashboardBPDevisionCoef { get; set; }
        public virtual DbSet<DashboardBPManpowerManuf> DashboardBPManpowerManuf { get; set; }
        public virtual DbSet<DashboardBPTaskInsert> DashboardBPTaskInsert { get; set; }
        public virtual DbSet<DashboardKO_UsersMonthPlan> DashboardKO_UsersMonthPlan { get; set; }
        public virtual DbSet<DashboardTV_FinishWorkInStartWeek> DashboardTV_FinishWorkInStartWeek { get; set; }
        public virtual DbSet<Debit_CMR> Debit_CMR { get; set; }
        public virtual DbSet<Debit_CostUpdate> Debit_CostUpdate { get; set; }
        public virtual DbSet<Debit_DataReportOprih> Debit_DataReportOprih { get; set; }
        public virtual DbSet<Debit_IstPost> Debit_IstPost { get; set; }
        public virtual DbSet<Debit_PeriodReportOprih> Debit_PeriodReportOprih { get; set; }
        public virtual DbSet<Debit_Platform> Debit_Platform { get; set; }
        public virtual DbSet<Debit_PostingOffType> Debit_PostingOffType { get; set; }
        public virtual DbSet<Debit_PostingOnType> Debit_PostingOnType { get; set; }
        public virtual DbSet<Debit_TN> Debit_TN { get; set; }
        public virtual DbSet<Debit_WorkBit> Debit_WorkBit { get; set; }
        public virtual DbSet<DebitReclamation> DebitReclamation { get; set; }
        public virtual DbSet<Devision> Devision { get; set; }
        public virtual DbSet<MailGraphic> MailGraphic { get; set; }
        public virtual DbSet<PF> PF { get; set; }
        public virtual DbSet<PlanVerificationItems> PlanVerificationItems { get; set; }
        public virtual DbSet<PlanVerificationItemsLog> PlanVerificationItemsLog { get; set; }
        public virtual DbSet<PostAlertShip> PostAlertShip { get; set; }
        public virtual DbSet<ProductionCalendar> ProductionCalendar { get; set; }
        public virtual DbSet<ProjectTask> ProjectTask { get; set; }
        public virtual DbSet<ProjectTaskLinks> ProjectTaskLinks { get; set; }
        public virtual DbSet<ProjectTypesLine> ProjectTypesLine { get; set; }
        public virtual DbSet<PZ_Client> PZ_Client { get; set; }
        public virtual DbSet<PZ_Currency> PZ_Currency { get; set; }
        public virtual DbSet<PZ_Dostavka> PZ_Dostavka { get; set; }
        public virtual DbSet<PZ_FIO> PZ_FIO { get; set; }
        public virtual DbSet<PZ_Notes> PZ_Notes { get; set; }
        public virtual DbSet<PZ_OperatorDogovora> PZ_OperatorDogovora { get; set; }
        public virtual DbSet<PZ_PlanZakaz> PZ_PlanZakaz { get; set; }
        public virtual DbSet<PZ_ProductType> PZ_ProductType { get; set; }
        public virtual DbSet<PZ_PZNotes> PZ_PZNotes { get; set; }
        public virtual DbSet<PZ_Setup> PZ_Setup { get; set; }
        public virtual DbSet<PZ_TEO> PZ_TEO { get; set; }
        public virtual DbSet<PZ_TypeShip> PZ_TypeShip { get; set; }
        public virtual DbSet<Reclamation> Reclamation { get; set; }
        public virtual DbSet<Reclamation_Answer> Reclamation_Answer { get; set; }
        public virtual DbSet<Reclamation_CloseOrder> Reclamation_CloseOrder { get; set; }
        public virtual DbSet<Reclamation_CountError> Reclamation_CountError { get; set; }
        public virtual DbSet<Reclamation_PZ> Reclamation_PZ { get; set; }
        public virtual DbSet<Reclamation_TechnicalAdvice> Reclamation_TechnicalAdvice { get; set; }
        public virtual DbSet<Reclamation_TechnicalAdviceProtocol> Reclamation_TechnicalAdviceProtocol { get; set; }
        public virtual DbSet<Reclamation_TechnicalAdviceProtocolPosition> Reclamation_TechnicalAdviceProtocolPosition { get; set; }
        public virtual DbSet<Reclamation_TechnicalAdviceTasks> Reclamation_TechnicalAdviceTasks { get; set; }
        public virtual DbSet<Reclamation_Type> Reclamation_Type { get; set; }
        public virtual DbSet<ReclamationTypeKB> ReclamationTypeKB { get; set; }
        public virtual DbSet<RKD_Despatching> RKD_Despatching { get; set; }
        public virtual DbSet<RKD_FileMailVersion> RKD_FileMailVersion { get; set; }
        public virtual DbSet<RKD_GIP> RKD_GIP { get; set; }
        public virtual DbSet<RKD_Institute> RKD_Institute { get; set; }
        public virtual DbSet<RKD_Mail_TimeForComplited> RKD_Mail_TimeForComplited { get; set; }
        public virtual DbSet<RKD_Mail_Version> RKD_Mail_Version { get; set; }
        public virtual DbSet<RKD_Order> RKD_Order { get; set; }
        public virtual DbSet<RKD_PostList> RKD_PostList { get; set; }
        public virtual DbSet<RKD_Question> RKD_Question { get; set; }
        public virtual DbSet<RKD_QuestionData> RKD_QuestionData { get; set; }
        public virtual DbSet<RKD_Task> RKD_Task { get; set; }
        public virtual DbSet<RKD_TaskVersion> RKD_TaskVersion { get; set; }
        public virtual DbSet<RKD_TypeTask> RKD_TypeTask { get; set; }
        public virtual DbSet<RKD_Version> RKD_Version { get; set; }
        public virtual DbSet<RKD_VersionDay> RKD_VersionDay { get; set; }
        public virtual DbSet<RKD_VersionWork> RKD_VersionWork { get; set; }
        public virtual DbSet<SandwichPanel> SandwichPanel { get; set; }
        public virtual DbSet<SandwichPanel_PZ> SandwichPanel_PZ { get; set; }
        public virtual DbSet<SandwichPanelCustomer> SandwichPanelCustomer { get; set; }
        public virtual DbSet<ServiceRemarks> ServiceRemarks { get; set; }
        public virtual DbSet<ServiceRemarksActions> ServiceRemarksActions { get; set; }
        public virtual DbSet<ServiceRemarksCause> ServiceRemarksCause { get; set; }
        public virtual DbSet<ServiceRemarksCauses> ServiceRemarksCauses { get; set; }
        public virtual DbSet<ServiceRemarksPlanZakazs> ServiceRemarksPlanZakazs { get; set; }
        public virtual DbSet<ServiceRemarksReclamations> ServiceRemarksReclamations { get; set; }
        public virtual DbSet<ServiceRemarksTypes> ServiceRemarksTypes { get; set; }
        public virtual DbSet<StickersPreOrder> StickersPreOrder { get; set; }
        public virtual DbSet<TaskForPZ> TaskForPZ { get; set; }
        public virtual DbSet<TypeRKD_Mail_Version> TypeRKD_Mail_Version { get; set; }
        public virtual DbSet<TypeTaskForPZ> TypeTaskForPZ { get; set; }
        public virtual DbSet<WBS> WBS { get; set; }
    }
}
