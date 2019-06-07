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
    
    public partial class PZ_PlanZakaz
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PZ_PlanZakaz()
        {
            this.CMO_PositionOrder = new HashSet<CMO_PositionOrder>();
            this.CMO_PositionPreOrder = new HashSet<CMO_PositionPreOrder>();
            this.CMO2_Position = new HashSet<CMO2_Position>();
            this.Debit_CostUpdate = new HashSet<Debit_CostUpdate>();
            this.Debit_Name = new HashSet<Debit_Name>();
            this.Debit_Platform = new HashSet<Debit_Platform>();
            this.Debit_WorkBit = new HashSet<Debit_WorkBit>();
            this.DebitReclamation = new HashSet<DebitReclamation>();
            this.OTK_ChaeckList = new HashSet<OTK_ChaeckList>();
            this.OTK_ReclamationKO = new HashSet<OTK_ReclamationKO>();
            this.PZ_Setup = new HashSet<PZ_Setup>();
            this.PZ_TEO = new HashSet<PZ_TEO>();
            this.Reclamation_PZ = new HashSet<Reclamation_PZ>();
            this.RKD_Order = new HashSet<RKD_Order>();
            this.Service_ReclamationPZ = new HashSet<Service_ReclamationPZ>();
            this.VVPZ = new HashSet<VVPZ>();
            this.Reclamation_CloseOrder = new HashSet<Reclamation_CloseOrder>();
        }
    
        public int Id { get; set; }
        public int PlanZakaz { get; set; }
        public System.DateTime DateCreate { get; set; }
        public string MTR { get; set; }
        public string Name { get; set; }
        public string OL { get; set; }
        public int Zapros { get; set; }
        public System.DateTime DateSupply { get; set; }
        public System.DateTime DateShipping { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public int ProductType { get; set; }
        public int Dostavka { get; set; }
        public string Manager { get; set; }
        public int Client { get; set; }
        public string Gruzopoluchatel { get; set; }
        public string PostAdresGruzopoluchatel { get; set; }
        public string INNGruzopoluchatel { get; set; }
        public string OKPOGruzopoluchatelya { get; set; }
        public string KodGruzopoluchatela { get; set; }
        public string StantionGruzopoluchatel { get; set; }
        public string KodStanciiGruzopoluchatelya { get; set; }
        public string OsobieOtmetkiGruzopoluchatelya { get; set; }
        public string DescriptionGruzopoluchatel { get; set; }
        public string Folder { get; set; }
        public string PowerST { get; set; }
        public string VN_NN { get; set; }
        public string Modul { get; set; }
        public string timeContract { get; set; }
        public Nullable<System.DateTime> timeContractDate { get; set; }
        public string timeArr { get; set; }
        public Nullable<System.DateTime> timeArrDate { get; set; }
        public int id_PZ_FIO { get; set; }
        public string numZakupki { get; set; }
        public string numLota { get; set; }
        public System.DateTime dataOtgruzkiBP { get; set; }
        public Nullable<System.Guid> ProjectUID { get; set; }
        public string nameTU { get; set; }
        public int TypeShip { get; set; }
        public Nullable<System.DateTime> criticalDateShip { get; set; }
        public int id_PZ_OperatorDogovora { get; set; }
        public double costPNR { get; set; }
        public double costSMR { get; set; }
        public string nomenklaturNumber { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMO_PositionOrder> CMO_PositionOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMO_PositionPreOrder> CMO_PositionPreOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CMO2_Position> CMO2_Position { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_CostUpdate> Debit_CostUpdate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_Name> Debit_Name { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_Platform> Debit_Platform { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Debit_WorkBit> Debit_WorkBit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DebitReclamation> DebitReclamation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OTK_ChaeckList> OTK_ChaeckList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OTK_ReclamationKO> OTK_ReclamationKO { get; set; }
        public virtual PZ_Client PZ_Client { get; set; }
        public virtual PZ_Dostavka PZ_Dostavka { get; set; }
        public virtual PZ_FIO PZ_FIO { get; set; }
        public virtual PZ_OperatorDogovora PZ_OperatorDogovora { get; set; }
        public virtual PZ_ProductType PZ_ProductType { get; set; }
        public virtual PZ_TypeShip PZ_TypeShip { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PZ_Setup> PZ_Setup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PZ_TEO> PZ_TEO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_PZ> Reclamation_PZ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RKD_Order> RKD_Order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service_ReclamationPZ> Service_ReclamationPZ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VVPZ> VVPZ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reclamation_CloseOrder> Reclamation_CloseOrder { get; set; }
    }
}
