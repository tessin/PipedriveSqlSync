using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PipeDriveApi;

namespace PipedriveSqlSync.Database
{
    public class DbDeal : Deal
    {
        public int DbPersonId
        {
            get { return PersonId?.Value ?? -1; }
            set { PersonId = new PersonId { Value = value }; }
        }

        public int DbOrgId
        {
            get { return OrgId?.Id ?? -1; }
            set { OrgId = new OrganizationId { Id = value }; }
        }

        public DbPerson Person { get; set; }
        public DbOrganization Organization { get; set; }
    }

    public class DealEntityConfiguration : EntityTypeConfiguration<DbDeal>
    {
        public DealEntityConfiguration()
        {
            ToTable("Deals");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Ignore(x => x.CreatorUserId);
            Ignore(x => x.UserId);
            Ignore(x => x.OrgId);

            Ignore(x => x.PersonId);
            Property(x => x.DbPersonId).HasColumnName(nameof(DbDeal.PersonId));
            HasRequired(x => x.Person).WithMany().HasForeignKey(x => x.DbPersonId).WillCascadeOnDelete(false);

            Ignore(x => x.OrgId);
            Property(x => x.DbOrgId).HasColumnName(nameof(DbDeal.OrgId));
            HasRequired(x => x.Organization).WithMany().HasForeignKey(x => x.DbOrgId).WillCascadeOnDelete(false);
        }

        //TODO: add tables and navigation properties for:
        //public Owner CreatorUserId { get; set; }
        //public Owner UserId { get; set; }
        //public int StageId { get; set; }
        //public int? NextActivityId { get; set; }
        //public int? LastActivityId { get; set; }
        //public int? PipelineId { get; set; }
    }
}
