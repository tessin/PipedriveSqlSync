using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PipedriveSqlSync.Persons
{
    public class PersonEntityConfiguration<T> : EntityTypeConfiguration<T> where T : DbPerson
    {
        public PersonEntityConfiguration()
        {
            ToTable("Persons");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Ignore(x => x.OwnerId);
            Property(x => x.DbOwnerId).HasColumnName(nameof(DbPerson.OwnerId));
            HasRequired(x => x.Owner).WithMany().HasForeignKey(x => x.DbOwnerId).WillCascadeOnDelete(false);

            Ignore(x => x.OrgId);
            Property(x => x.DbOrgId).HasColumnName(nameof(DbPerson.OrgId));

            Ignore(x => x.PictureId);
            Property(x => x.DbPictureId).HasColumnName(nameof(DbPerson.PictureId));

            Ignore(x => x.Phone);
            Ignore(x => x.Email);
        }

        //TODO: add tables and navigation properties for:
        //public List<Phone> Phone { get; set; }
        //public List<Email> Email { get; set; }
        //public int? NextActivityId { get; set; }
        //public int? LastActivityId { get; set; }
    }
}
