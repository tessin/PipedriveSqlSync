using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PipedriveSqlSync.Users;

namespace PipedriveSqlSync.Organizations
{
    public class OrganizationEntityConfiguration<T> : EntityTypeConfiguration<T> where T : DbOrganization
    {
        public OrganizationEntityConfiguration()
        {
            ToTable("Organizations");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Ignore(x => x.OwnerId);
            Property(x => x.DbOwnerId).HasColumnName(nameof(DbOrganization.OwnerId));
            HasRequired(x => x.Owner).WithMany().HasForeignKey(x => x.DbOwnerId).WillCascadeOnDelete(false);
        }

        //TODO: add tables and navigation properties:
        //public int? CategoryId { get; set; }
        //public int? NextActivityId { get; set; }
        //public int? LastActivityId { get; set; }
    }
}