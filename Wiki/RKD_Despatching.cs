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
    
    public partial class RKD_Despatching
    {
        public int id { get; set; }
        public Nullable<int> id_RKD_Order { get; set; }
        public Nullable<System.DateTime> dateEvent { get; set; }
        public string text { get; set; }
        public Nullable<System.DateTime> dateTaskFinishDate { get; set; }
        public string id_AspNetUsers { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual RKD_Order RKD_Order { get; set; }
    }
}
