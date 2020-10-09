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
    
    public partial class E3_Components_Symbols_EditEntities : DbContext
    {
        public E3_Components_Symbols_EditEntities()
            : base("name=E3_Components_Symbols_EditEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Assembly> Assembly { get; set; }
        public virtual DbSet<BlockedInfos> BlockedInfos { get; set; }
        public virtual DbSet<Cable> Cable { get; set; }
        public virtual DbSet<COMPONENT_STATES> COMPONENT_STATES { get; set; }
        public virtual DbSet<ComponentAttribute> ComponentAttribute { get; set; }
        public virtual DbSet<ComponentData> ComponentData { get; set; }
        public virtual DbSet<DatabaseInfo> DatabaseInfo { get; set; }
        public virtual DbSet<Drill> Drill { get; set; }
        public virtual DbSet<FUNCTION_ATTRIBUTES> FUNCTION_ATTRIBUTES { get; set; }
        public virtual DbSet<FUNCTIONAL_ASSIGNMENTS> FUNCTIONAL_ASSIGNMENTS { get; set; }
        public virtual DbSet<FUNCTIONAL_PORTS> FUNCTIONAL_PORTS { get; set; }
        public virtual DbSet<FUNCTIONAL_UNIT_STATES> FUNCTIONAL_UNIT_STATES { get; set; }
        public virtual DbSet<FUNCTIONAL_UNITS> FUNCTIONAL_UNITS { get; set; }
        public virtual DbSet<Gate> Gate { get; set; }
        public virtual DbSet<GATE_FUNCTION_ATTRIBUTES> GATE_FUNCTION_ATTRIBUTES { get; set; }
        public virtual DbSet<GatePinRelation> GatePinRelation { get; set; }
        public virtual DbSet<Layer> Layer { get; set; }
        public virtual DbSet<LOCKINFO> LOCKINFO { get; set; }
        public virtual DbSet<LogicAttribute> LogicAttribute { get; set; }
        public virtual DbSet<ModelAttribute> ModelAttribute { get; set; }
        public virtual DbSet<ModelData> ModelData { get; set; }
        public virtual DbSet<Outlines> Outlines { get; set; }
        public virtual DbSet<OutlineSymbols> OutlineSymbols { get; set; }
        public virtual DbSet<PadStack> PadStack { get; set; }
        public virtual DbSet<PhysPin> PhysPin { get; set; }
        public virtual DbSet<Pin> Pin { get; set; }
        public virtual DbSet<PinAttribute> PinAttribute { get; set; }
        public virtual DbSet<PinDesign> PinDesign { get; set; }
        public virtual DbSet<PlugInfo> PlugInfo { get; set; }
        public virtual DbSet<RetainingRailTypes> RetainingRailTypes { get; set; }
        public virtual DbSet<SheetInformation> SheetInformation { get; set; }
        public virtual DbSet<SlotAttribute> SlotAttribute { get; set; }
        public virtual DbSet<Slots> Slots { get; set; }
        public virtual DbSet<Supply> Supply { get; set; }
        public virtual DbSet<SYMBOL_FUNCTION> SYMBOL_FUNCTION { get; set; }
        public virtual DbSet<SYMBOL_STATES> SYMBOL_STATES { get; set; }
        public virtual DbSet<SymbolAttribute> SymbolAttribute { get; set; }
        public virtual DbSet<SymbolData> SymbolData { get; set; }
        public virtual DbSet<SymbolDimension> SymbolDimension { get; set; }
        public virtual DbSet<SymbolGraphic> SymbolGraphic { get; set; }
        public virtual DbSet<SymbolNodes> SymbolNodes { get; set; }
        public virtual DbSet<SymbolPoints> SymbolPoints { get; set; }
        public virtual DbSet<SymbolText> SymbolText { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Technology> Technology { get; set; }
        public virtual DbSet<ValidRetainingRailTypes> ValidRetainingRailTypes { get; set; }
    }
}