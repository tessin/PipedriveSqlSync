using System.Data.Entity;
using System.Threading.Tasks;
using PipedriveSqlSync.Database;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.EntitySyncs
{
    public class OrganizationEntitySync : EntitySync<DbOrganization>
    {
        public OrganizationEntitySync(
            PagingEntityService<DbOrganization> entityService, DbSet<DbOrganization> dbSet, ILogger logger = null) 
            : base(entityService, dbSet, logger)
        {
        }

        public override async Task Load()
        {
            Logger?.Info("Loading organizations...");
            await base.Load();
            Logger?.Info($"Loaded {Entities.Count} organizations.");
        }

        public override async Task Clean()
        {
            Logger?.Info("Cleaning organizations from database...");
            await base.Clean();
            Logger?.Info("Organizations cleaned.");
        }
    }
}
