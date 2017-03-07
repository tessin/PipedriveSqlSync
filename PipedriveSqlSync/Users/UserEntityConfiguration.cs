using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PipedriveSqlSync.Users
{
    public class UserEntityConfiguration : EntityTypeConfiguration<DbUser>
    {
        public UserEntityConfiguration()
        {
            ToTable("Users");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }

        //TODO: add tables and navigation properties:
        //public int RoleId { get; set; }
    }
}
