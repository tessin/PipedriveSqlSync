using System.Threading.Tasks;
using PipeDriveApi;

namespace PipedriveSqlSync.Shared.Dynamics
{
    public interface IDynamicSyncService<out T> : ISyncService<T>, IDynamicSyncService { }

    public interface IDynamicSyncService : ISyncService
    {
        Task<DynamicEntityConfiguration> ConstructType(IPipeDriveClient client);
    }
}
