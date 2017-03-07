using PipedriveSqlSync.Users;
using PipeDriveApi;

namespace PipedriveSqlSync.Organizations
{
    public class DbOrganization : Organization
    {
        public int DbOwnerId
        {
            get { return OwnerId?.Id ?? -1; }
            set { OwnerId = new Owner { Id = value }; }
        }

        public DbUser Owner { get; set; }
    }
}
