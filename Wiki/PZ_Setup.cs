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
    
    public partial class PZ_Setup
    {
        public int id { get; set; }
        public int id_PZ_PlanZakaz { get; set; }
        public int KolVoDneyNaPrijemku { get; set; }
        public string PunktDogovoraOSrokahPriemki { get; set; }
        public string UslovieOplatyText { get; set; }
        public int UslovieOplatyInt { get; set; }
        public int TimeNaRKD { get; set; }
        public int RassmotrenieRKD { get; set; }
        public int SrokZamechanieRKD { get; set; }
        public string userTP { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual PZ_PlanZakaz PZ_PlanZakaz { get; set; }
    }
}
