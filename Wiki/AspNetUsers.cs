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
    
    public partial class AspNetUsers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetUsers()
        {
            this.ApproveCDActions = new HashSet<ApproveCDActions>();
            this.ApproveCDOrders = new HashSet<ApproveCDOrders>();
            this.ApproveCDOrders1 = new HashSet<ApproveCDOrders>();
            this.ApproveCDQuestionCorr = new HashSet<ApproveCDQuestionCorr>();
            this.ApproveCDQuestions = new HashSet<ApproveCDQuestions>();
            this.AspNetUserClaims = new HashSet<AspNetUserClaims>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogins>();
            this.CMKO_BujetList = new HashSet<CMKO_BujetList>();
            this.CMKO_BujetList1 = new HashSet<CMKO_BujetList>();
            this.CMKO_Optimization = new HashSet<CMKO_Optimization>();
            this.CMKO_Optimization1 = new HashSet<CMKO_Optimization>();
            this.CMKO_Optimization2 = new HashSet<CMKO_Optimization>();
            this.CMKO_RemarksList = new HashSet<CMKO_RemarksList>();
            this.CMKO_RemarksListG = new HashSet<CMKO_RemarksListG>();
            this.CMKO_SummaryResultToMonth = new HashSet<CMKO_SummaryResultToMonth>();
            this.CMKO_Teach = new HashSet<CMKO_Teach>();
            this.CMKO_Teach1 = new HashSet<CMKO_Teach>();
            this.CMKO_Teach2 = new HashSet<CMKO_Teach>();
            this.CMKO_Teach3 = new HashSet<CMKO_Teach>();
            this.CMKO_ThisAccrued = new HashSet<CMKO_ThisAccrued>();
            this.CMKO_ThisAccruedG = new HashSet<CMKO_ThisAccruedG>();
            this.CMKO_ThisCoefManager = new HashSet<CMKO_ThisCoefManager>();
            this.CMKO_ThisIndicatorsUsers = new HashSet<CMKO_ThisIndicatorsUsers>();
            this.CMO2_Order = new HashSet<CMO2_Order>();
            this.CMOSOrder = new HashSet<CMOSOrder>();
            this.CMOSPreOrder = new HashSet<CMOSPreOrder>();
            this.DashboardBP_ProjectTasks = new HashSet<DashboardBP_ProjectTasks>();
            this.DashboardBPTaskInsert = new HashSet<DashboardBPTaskInsert>();
            this.DashboardKO_UsersMonthPlan = new HashSet<DashboardKO_UsersMonthPlan>();
            this.OrdersTables = new HashSet<OrdersTables>();
            this.PlanVerificationItemsLog = new HashSet<PlanVerificationItemsLog>();
            this.PZ_Notes = new HashSet<PZ_Notes>();
            this.PZ_PlanZakaz = new HashSet<PZ_PlanZakaz>();
            this.PZ_Setup = new HashSet<PZ_Setup>();
            this.Reclamation_Answer = new HashSet<Reclamation_Answer>();
            this.Reclamation = new HashSet<Reclamation>();
            this.Reclamation1 = new HashSet<Reclamation>();
            this.Reclamation_CloseOrder = new HashSet<Reclamation_CloseOrder>();
            this.Reclamation_TechnicalAdvice = new HashSet<Reclamation_TechnicalAdvice>();
            this.Reclamation_TechnicalAdvice1 = new HashSet<Reclamation_TechnicalAdvice>();
            this.Reclamation_TechnicalAdviceTasks = new HashSet<Reclamation_TechnicalAdviceTasks>();
            this.RKD_Despatching = new HashSet<RKD_Despatching>();
            this.RKD_GIP = new HashSet<RKD_GIP>();
            this.RKD_GIP1 = new HashSet<RKD_GIP>();
            this.RKD_Question = new HashSet<RKD_Question>();
            this.RKD_QuestionData = new HashSet<RKD_QuestionData>();
            this.RKD_QuestionData1 = new HashSet<RKD_QuestionData>();
            this.SandwichPanel = new HashSet<SandwichPanel>();
            this.ServiceRemarks = new HashSet<ServiceRemarks>();
            this.ServiceRemarksActions = new HashSet<ServiceRemarksActions>();
            this.StickersPreOrder = new HashSet<StickersPreOrder>();
            this.TaskForPZ = new HashSet<TaskForPZ>();
            this.AspNetRoles = new HashSet<AspNetRoles>();
            this.PlexiglassOrder = new HashSet<PlexiglassOrder>();
        }
    
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string CiliricalName { get; set; }
        public Nullable<int> Devision { get; set; }
        public Nullable<System.Guid> ResourceUID { get; set; }
        public Nullable<int> id_CMKO_TaxCatigories { get; set; }
        public Nullable<System.DateTime> dateToCMKO { get; set; }
        public Nullable<double> tax { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDActions> ApproveCDActions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDOrders> ApproveCDOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDOrders> ApproveCDOrders1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDQuestionCorr> ApproveCDQuestionCorr { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApproveCDQuestions> ApproveCDQuestions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual CMKO_TaxCatigories CMKO_TaxCatigories { get; set; }
        public virtual Devision Devision1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_BujetList> CMKO_BujetList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_BujetList> CMKO_BujetList1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_Optimization> CMKO_Optimization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_Optimization> CMKO_Optimization1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_Optimization> CMKO_Optimization2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_RemarksList> CMKO_RemarksList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_RemarksListG> CMKO_RemarksListG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_SummaryResultToMonth> CMKO_SummaryResultToMonth { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_Teach> CMKO_Teach { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_Teach> CMKO_Teach1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_Teach> CMKO_Teach2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_Teach> CMKO_Teach3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_ThisAccrued> CMKO_ThisAccrued { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_ThisAccruedG> CMKO_ThisAccruedG { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_ThisCoefManager> CMKO_ThisCoefManager { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMKO_ThisIndicatorsUsers> CMKO_ThisIndicatorsUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMO2_Order> CMO2_Order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMOSOrder> CMOSOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMOSPreOrder> CMOSPreOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DashboardBP_ProjectTasks> DashboardBP_ProjectTasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DashboardBPTaskInsert> DashboardBPTaskInsert { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DashboardKO_UsersMonthPlan> DashboardKO_UsersMonthPlan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersTables> OrdersTables { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanVerificationItemsLog> PlanVerificationItemsLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PZ_Notes> PZ_Notes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PZ_PlanZakaz> PZ_PlanZakaz { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PZ_Setup> PZ_Setup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_Answer> Reclamation_Answer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation> Reclamation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation> Reclamation1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_CloseOrder> Reclamation_CloseOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_TechnicalAdvice> Reclamation_TechnicalAdvice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_TechnicalAdvice> Reclamation_TechnicalAdvice1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_TechnicalAdviceTasks> Reclamation_TechnicalAdviceTasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_Despatching> RKD_Despatching { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_GIP> RKD_GIP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_GIP> RKD_GIP1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_Question> RKD_Question { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_QuestionData> RKD_QuestionData { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_QuestionData> RKD_QuestionData1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SandwichPanel> SandwichPanel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRemarks> ServiceRemarks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRemarksActions> ServiceRemarksActions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StickersPreOrder> StickersPreOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskForPZ> TaskForPZ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRoles> AspNetRoles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlexiglassOrder> PlexiglassOrder { get; set; }
    }
}
