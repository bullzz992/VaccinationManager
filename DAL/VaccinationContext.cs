using VaccinationManager.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

namespace VaccinationManager.DAL
{
    public class VaccinationContext : DbContext
    {
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<UserBranch> Branches { get; set; }
        public DbSet<VaccinationDefinition> VaccinationDefinitions { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Age> Ages { get; set; }
        public DbSet<ChildMeasurement> ChildMeasurements { get; set; }

        public VaccinationContext()
        {
            Database.SetInitializer<VaccinationContext>(null);
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            Database.SetInitializer<VaccinationContext>(null);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<Vaccination>()
            //    .HasMany(c => c.Administrators).WithMany(i => i.Vaccinations)
            //    .Map(t => t.MapLeftKey("VaccinationID")
            //        .MapRightKey("AdministratorID")
            //        .ToTable("VaccinationAdministrator"));

            //modelBuilder.Entity<Department>().MapToStoredProcedures();
        }

        public System.Data.Entity.DbSet<VaccinationManager.Models.Parent> Parents { get; set; }

        public System.Data.Entity.DbSet<VaccinationManager.Models.VaccinationPrice> VaccinationPrices { get; set; }

        public System.Data.Entity.DbSet<VaccinationManager.Models.ExtendedFee> ExtendedFees { get; set; }

        public System.Data.Entity.DbSet<VaccinationManager.Models.Branch> Branches1 { get; set; }

        public System.Data.Entity.DbSet<VaccinationManager.Models.UserStatus> UserStatus { get; set; }

        public System.Data.Entity.DbSet<VaccinationManager.Models.Address> Address { get; set; }

        public System.Data.Entity.DbSet<VaccinationManager.Models.SMSConfig> SmsConfig { get; set; }

    }
}