﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PersonMotion.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class peopleEntities : DbContext
    {
        public peopleEntities()
            : base("name=peopleEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<department> department { get; set; }
        public virtual DbSet<files> files { get; set; }
        public virtual DbSet<metaData> metaData { get; set; }
        public virtual DbSet<section> section { get; set; }
        public virtual DbSet<familia> familia { get; set; }
        public virtual DbSet<position> position { get; set; }
        public virtual DbSet<workhistory> workhistory { get; set; }
        public virtual DbSet<organization> organization { get; set; }
        public virtual DbSet<VW_Position> VW_Position { get; set; }
        public virtual DbSet<education> education { get; set; }
        public virtual DbSet<address> address { get; set; }
        public virtual DbSet<employee> employee { get; set; }
        public virtual DbSet<reproval> reproval { get; set; }
        public virtual DbSet<EMP_LIST> EMP_LIST { get; set; }
        public virtual DbSet<salary> salary { get; set; }
    }
}
