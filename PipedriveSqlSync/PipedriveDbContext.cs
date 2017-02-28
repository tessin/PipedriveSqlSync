using System.Data.Entity;
using PipedriveSqlSync.Database;

namespace PipedriveSqlSync
{
    public class PipedriveDbContext : DbContext
    {
        public PipedriveDbContext(): base("PipedriveSqlSync.PipedriveDbContext") { }

        public PipedriveDbContext(string connectionString) : base(connectionString) { }

        public DbSet<DbOrganization> Organizations { get; set; }
        public DbSet<DbPerson> Persons { get; set; }
        public DbSet<DbDeal> Deals { get; set; }
        public DbSet<DbActivity> Activities { get; set; }
        public DbSet<DbNote> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrganizationEntityConfiguration());
            modelBuilder.Configurations.Add(new PersonEntityConfiguration());
            modelBuilder.Configurations.Add(new DealEntityConfiguration());
            modelBuilder.Configurations.Add(new ActivityEntityConfiguration());
            modelBuilder.Configurations.Add(new NoteEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
