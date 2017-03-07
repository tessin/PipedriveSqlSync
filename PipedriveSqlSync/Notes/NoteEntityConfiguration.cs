using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PipedriveSqlSync.Notes
{
    public class NoteEntityConfiguration : EntityTypeConfiguration<DbNote>
    {
        public NoteEntityConfiguration()
        {
            ToTable("Notes");

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasOptional(x => x.LastUpdateUser)
                .WithMany()
                .HasForeignKey(x => x.LastUpdateUserId)
                .WillCascadeOnDelete(false);
        }
    }
}