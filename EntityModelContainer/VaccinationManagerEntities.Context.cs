﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityModelContainer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class VaccinationManagerEntities : DbContext
    {
        public VaccinationManagerEntities()
            : base("name=VaccinationManagerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Age> Ages { get; set; }
        public virtual DbSet<Allergy> Allergies { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<ChildMeasurement> ChildMeasurements { get; set; }
        public virtual DbSet<Child> Children { get; set; }
        public virtual DbSet<ExtendedFee> ExtendedFees { get; set; }
        public virtual DbSet<Parent> Parents { get; set; }
        public virtual DbSet<UserBranch> UserBranches { get; set; }
        public virtual DbSet<UserStatu> UserStatus { get; set; }
        public virtual DbSet<VaccinationDefinition> VaccinationDefinitions { get; set; }
        public virtual DbSet<VaccinationPrice> VaccinationPrices { get; set; }
        public virtual DbSet<Vaccination> Vaccinations { get; set; }
    }
}
