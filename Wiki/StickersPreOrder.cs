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
    
    public partial class StickersPreOrder
    {
        public int id { get; set; }
        public System.DateTime datetimeCreate { get; set; }
        public string id_AspNetUsersCreate { get; set; }
        public System.DateTime deadline { get; set; }
        public string description { get; set; }
        public Nullable<int> id_PZ_PlanZakaz { get; set; }
        public Nullable<System.DateTime> datetimePush { get; set; }
        public Nullable<System.DateTime> datetimeClose { get; set; }
        public System.DateTime datePlanUpload { get; set; }
        public string orderNumString { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
    }
}
