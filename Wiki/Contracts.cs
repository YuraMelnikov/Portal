//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wiki
{
    using System;
    using System.Collections.Generic;
    
    public partial class Contracts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contracts()
        {
            this.Specifications = new HashSet<Specifications>();
        }
    
        public int id { get; set; }
        public System.DateTime datetimeCreate { get; set; }
        public string id_AspNetUsersCreate { get; set; }
        public string name { get; set; }
        public System.DateTime date { get; set; }
        public string description { get; set; }
        public int id_PZ_Client { get; set; }
        public bool remove { get; set; }
        public int id_Provider { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Specifications> Specifications { get; set; }
    }
}
