using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PipeDriveApi;

namespace PipedriveSqlSync.Database
{
    public class DbOrganization : Organization { }

    public class OrganizationEntityConfiguration : EntityTypeConfiguration<DbOrganization>
    {
        public OrganizationEntityConfiguration()
        {
            ToTable("Organizations");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Ignore(x => x.OwnerId);
        }

        //TODO: add tables and navigation properties for:
        //public Owner OwnerId { get; set; }
        //public int? CategoryId { get; set; }
        //public int? NextActivityId { get; set; }
        //public int? LastActivityId { get; set; }
    }
}
