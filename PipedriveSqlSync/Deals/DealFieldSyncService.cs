using System;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Fields;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Deals
{
    public class DealFieldSyncService : FieldSyncService<DbDealField>
    {
        private readonly ILogger _logger;

        public DealFieldSyncService(ILogger logger = null)
        {
            _logger = logger;
        }

        public override async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading deal fields...");
            Entities = await new DealFieldEntityService<DbDealField>(client).GetAllAsync();
        }

        public override void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing deal fields...");
            base.Sync(context);
        }
    }
}
