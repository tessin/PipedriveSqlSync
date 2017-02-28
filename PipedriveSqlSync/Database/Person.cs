using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PipeDriveApi;

namespace PipedriveSqlSync.Database
{
    public class DbPerson : Person
    {
        public int DbOrgId
        {
            get { return OrgId?.Id ?? -1; }
            set { OrgId = new OrganizationId { Id = value }; }
        }

        public DbOrganization Organization { get; set; }
    }

    public class PersonEntityConfiguration : EntityTypeConfiguration<DbPerson>
    {
        public PersonEntityConfiguration()
        {
            ToTable("Persons");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Ignore(x => x.OwnerId);

            Ignore(x => x.OrgId);
            Property(x => x.DbOrgId).HasColumnName(nameof(DbPerson.OrgId));
            HasRequired(x => x.Organization).WithMany().HasForeignKey(x => x.DbOrgId).WillCascadeOnDelete(false);

            Ignore(x => x.Phone);
            Ignore(x => x.Email);
        }

        //TODO: add tables and navigation properties for:
        //public Owner OwnerId { get; set; }
        //public List<Phone> Phone { get; set; }
        //public List<Email> Email { get; set; }
        //public int? NextActivityId { get; set; }
        //public int? LastActivityId { get; set; }
    }
}
