using System.Collections.Generic;
using System.Threading.Tasks;
using PipeDriveApi;

namespace PipedriveSqlSync.Shared
{
    public interface ISyncService
    {
        Task Fetch(IPipeDriveClient client);
        void Sync(PipeDriveDbContext context);
    }

    public interface ISyncService<out T> : ISyncService
    {
        IEnumerable<T> Entities { get; }
    }
}
