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
    
    public partial class SpecificationsPositions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SpecificationsPositions()
        {
            this.PlanOrders = new HashSet<PlanOrders>();
        }
    
        public int id { get; set; }
        public System.DateTime datetimeCreate { get; set; }
        public string id_AspNetUsersCreate { get; set; }
        public int id_Specifications { get; set; }
        public int numPositions { get; set; }
        public string skuPost { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public int id_Measure { get; set; }
        public int quantity { get; set; }
        public string questionnaireNumber { get; set; }
        public string requestNumber { get; set; }
        public string description { get; set; }
        public string folder { get; set; }
        public string lotNumber { get; set; }
        public bool remove { get; set; }
        public System.DateTime deliveryDate { get; set; }
        public System.DateTime deliveryDateIntoMan { get; set; }
        public bool prepayment { get; set; }
        public Nullable<System.DateTime> datePrepayment { get; set; }
        public int dayPrepayment { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanOrders> PlanOrders { get; set; }
        public virtual Specifications Specifications { get; set; }
    }
}
