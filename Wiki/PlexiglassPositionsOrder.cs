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
    
    public partial class PlexiglassPositionsOrder
    {
        public int id { get; set; }
        public int id_PlexiglassOrder { get; set; }
        public string positionNum { get; set; }
        public string designation { get; set; }
        public string name { get; set; }
        public string index { get; set; }
        public int quentity { get; set; }
        public double square { get; set; }
        public string barcode { get; set; }
    
        public virtual PlexiglassOrder PlexiglassOrder { get; set; }
    }
}
