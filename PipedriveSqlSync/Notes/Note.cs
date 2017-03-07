using PipedriveSqlSync.Users;
using PipeDriveApi;

namespace PipedriveSqlSync.Notes
{
    public class DbNote : Note
    {
        public DbUser LastUpdateUser { get; set; }
    }
}
