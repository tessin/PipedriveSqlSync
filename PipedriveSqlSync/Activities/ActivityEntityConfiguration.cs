using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PipedriveSqlSync.Activities
{
    public class ActivityEntityConfiguration : EntityTypeConfiguration<DbActivity>
    {
        public ActivityEntityConfiguration()
        {
            ToTable("Activities");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasOptional(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            HasOptional(x => x.AssignedToUser).WithMany().HasForeignKey(x => x.AssignedToUserId);
            HasOptional(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedByUserId);
        }

        //TODO: add tables and navigation properties for:
        //public int? ReferenceId { get; set; }
    }
}