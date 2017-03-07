using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PipedriveSqlSync.Deals
{
    public class DealEntityConfiguration<T> : EntityTypeConfiguration<T>
        where T : DbDeal
    {
        public DealEntityConfiguration()
        {
            ToTable("Deals");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Ignore(x => x.CreatorUserId);
            Property(x => x.DbCreatorUserId).HasColumnName(nameof(DbDeal.CreatorUserId));
            HasRequired(x => x.CreatorUser).WithMany().HasForeignKey(x => x.DbCreatorUserId).WillCascadeOnDelete(false);

            Ignore(x => x.UserId);
            Property(x => x.DbUserId).HasColumnName(nameof(DbDeal.UserId));
            HasOptional(x => x.User).WithMany().HasForeignKey(x => x.DbUserId).WillCascadeOnDelete(false);

            Ignore(x => x.PersonId);
            Property(x => x.DbPersonId).HasColumnName(nameof(DbDeal.PersonId));

            Ignore(x => x.OrgId);
            Property(x => x.DbOrgId).HasColumnName(nameof(DbDeal.OrgId));
        }

        //TODO: add tables and navigation properties for:
        //public int StageId { get; set; }
        //public int? NextActivityId { get; set; }
        //public int? LastActivityId { get; set; }
        //public int? PipelineId { get; set; }
    }
}