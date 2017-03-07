using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using PipedriveSqlSync.Activities;
using PipedriveSqlSync.Deals;
using PipedriveSqlSync.Notes;
using PipedriveSqlSync.Organizations;
using PipedriveSqlSync.Persons;
using PipedriveSqlSync.Shared.Dynamics;
using PipedriveSqlSync.Users;

namespace PipedriveSqlSync.Shared
{
    public class PipeDriveDbContext : DbContext
    {
        private readonly IEnumerable<DynamicEntityConfiguration> _configurations;

        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbActivity> Activities { get; set; }
        public DbSet<DbActivityField> ActivityFields { get; set; }
        public DbSet<DbPersonField> PersonFields { get; set; }
        public DbSet<DbDealField> DealFields { get; set; }
        public DbSet<DbNote> Notes { get; set; }
        public DbSet<DbNoteField> NoteFields { get; set; }
        public DbSet<DbOrganizationField> OrganizationFields { get; set; }
        public DbSet<DbOrganizationRelationship> OrganizationRelationships { get; set; }

        public PipeDriveDbContext(
            string connectionString,
            IEnumerable<DynamicEntityConfiguration> configurations)
            : base(connectionString)
        {
            if (configurations == null) throw new ArgumentNullException(nameof(configurations));

            _configurations = configurations;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Configurations.Add(new UserEntityConfiguration());
            modelBuilder.Configurations.Add(new ActivityEntityConfiguration());
            modelBuilder.Configurations.Add(new ActivityFieldEntityConfiguration());
            modelBuilder.Configurations.Add(new ActivityTypeEntityConfiguration());
            modelBuilder.Configurations.Add(new PersonFieldEntityConfiguration());
            modelBuilder.Configurations.Add(new DealFieldEntityConfiguration());
            modelBuilder.Configurations.Add(new NoteEntityConfiguration());
            modelBuilder.Configurations.Add(new NoteFieldEntityConfiguration());
            modelBuilder.Configurations.Add(new OrganizationFieldEntityConfiguration());
            modelBuilder.Configurations.Add(new OrganizationRelationshipEntityConfiguration());

            RegisterDynamicConfigurations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void RegisterDynamicConfigurations(DbModelBuilder modelBuilder)
        {
            var addMethod = typeof(ConfigurationRegistrar)
                .GetMethods()
                .Single(m => m.Name == "Add" && m.GetGenericArguments().Any(a => a.Name == "TEntityType"));

            foreach (var configuration in _configurations)
            {
                var configurationInstance = Activator.CreateInstance(
                    configuration.EntityConfigurationGenericType.MakeGenericType(configuration.EntityType));

                addMethod.MakeGenericMethod(configuration.EntityType)
                    .Invoke(modelBuilder.Configurations, new[] { configurationInstance });
            }
        }
    }
}
