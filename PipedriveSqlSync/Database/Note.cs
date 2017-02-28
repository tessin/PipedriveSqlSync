using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PipeDriveApi;

namespace PipedriveSqlSync.Database
{
    public class DbNote : Note
    {
        public DbDeal Deal { get; set; }
        public DbPerson Person { get; set; }
        public DbOrganization Organization { get; set; }
    }

    public class NoteEntityConfiguration : EntityTypeConfiguration<DbNote>
    {
        public NoteEntityConfiguration()
        {
            ToTable("Notes");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasOptional(x => x.Deal).WithMany().HasForeignKey(x => x.DealId);
            HasOptional(x => x.Person).WithMany().HasForeignKey(x => x.PersonId);
            HasOptional(x => x.Organization).WithMany().HasForeignKey(x => x.OrgId);
        }

        //TODO: add tables and navigation properties for:
        //public int? LastUpdateUserId { get; set; }
    }
}
