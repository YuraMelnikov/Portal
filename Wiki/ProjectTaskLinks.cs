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
    
    public partial class ProjectTaskLinks
    {
        public int id { get; set; }
        public int id_ProjectTaskPredecessor { get; set; }
        public int id_ProjectTaskSuccessor { get; set; }
        public int id_ProjectTypesLine { get; set; }
        public int lag { get; set; }
    
        public virtual ProjectTask ProjectTask { get; set; }
        public virtual ProjectTask ProjectTask1 { get; set; }
        public virtual ProjectTypesLine ProjectTypesLine { get; set; }
    }
}
