﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SCellsEntities : DbContext
    {
        public SCellsEntities()
            : base("name=SCellsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cells> Cells { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<SectionMap> SectionMap { get; set; }
        public virtual DbSet<Tier> Tier { get; set; }
        public virtual DbSet<TierMap> TierMap { get; set; }
    }
}
