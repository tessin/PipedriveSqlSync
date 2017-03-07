using PipedriveSqlSync.Users;
using PipeDriveApi;

namespace PipedriveSqlSync.Activities
{
    public class DbActivity : Activity
    {
        public DbUser User { get; set; }
        public DbUser AssignedToUser { get; set; }
        public DbUser CreatedByUser { get; set; }
    }
}
