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
    
    public partial class Reclamation_TechnicalAdviceTasks
    {
        public int id { get; set; }
        public int id_Reclamation_TechnicalAdvice { get; set; }
        public string id_AspNetUsers { get; set; }
        public string textTask { get; set; }
        public System.DateTime deadline { get; set; }
        public System.DateTime datetimeCreate { get; set; }
        public string textAnswer { get; set; }
        public Nullable<System.DateTime> dateClose { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Reclamation_TechnicalAdvice Reclamation_TechnicalAdvice { get; set; }
    }
}