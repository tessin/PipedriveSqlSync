using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PipeDriveApi;

namespace PipedriveSqlSync.Database
{
    public class DbActivity : Activity
    {
        public DbDeal Deal { get; set; }
        public DbOrganization Organization { get; set; }
        public DbPerson Person { get; set; }
    }

    public class ActivityEntityConfiguration : EntityTypeConfiguration<DbActivity>
    {
        public ActivityEntityConfiguration()
        {
            ToTable("Activities");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasOptional(x => x.Deal).WithMany().HasForeignKey(x => x.DealId);
            HasOptional(x => x.Organization).WithMany().HasForeignKey(x => x.OrgId);
            HasOptional(x => x.Person).WithMany().HasForeignKey(x => x.PersonId);
        }

        //TODO: add tables and navigation properties for:
        //public int? UserId { get; set; }
        //public int? ReferenceId { get; set; }
        //public int? AssignedToUserId { get; set; }
        //public int? CreatedByUserId { get; set; }
    }
}
