﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PowiedzMiDataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PowiedzMiEntities1 : DbContext
    {
        public PowiedzMiEntities1()
            : base("name=PowiedzMiEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Ankiety> Ankiety { get; set; }
        public virtual DbSet<CzlonekKregu> CzlonekKregu { get; set; }
        public virtual DbSet<Kregi> Kregi { get; set; }
        public virtual DbSet<Pytania> Pytania { get; set; }
        public virtual DbSet<PytanieWAnkiecie> PytanieWAnkiecie { get; set; }
        public virtual DbSet<Uzytkownicy> Uzytkownicy { get; set; }
    }
}
