using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PipeDriveApi;

namespace PipedriveSqlSync.Shared.Fields
{
    public abstract class FieldEntityConfiguration<T> : EntityTypeConfiguration<T>
        where T : Field
    {
        protected FieldEntityConfiguration(string tableName)
        {
            ToTable(tableName);

            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}