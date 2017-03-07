using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PipedriveSqlSync.Activities
{
    public class ActivityTypeEntityConfiguration : EntityTypeConfiguration<DbActivityType>
    {
        public ActivityTypeEntityConfiguration()
        {
            ToTable("ActivityTypes");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
