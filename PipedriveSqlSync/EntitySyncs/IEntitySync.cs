using System.Threading.Tasks;

namespace PipedriveSqlSync.EntitySyncs
{
    public interface IEntitySync
    {
        Task Load();
        Task Clean();
        Task Sync();
    }
}
