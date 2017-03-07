using System;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Fields;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Activities
{
    public class ActivityFieldSyncService : FieldSyncService<DbActivityField>
    {
        private readonly ILogger _logger;

        public ActivityFieldSyncService(ILogger logger = null)
        {
            _logger = logger;
        }

        public override async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading activity fields...");
            Entities = await new ActivityFieldEntityService<DbActivityField>(client).GetAllAsync();
        }

        public override void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing activity fields...");
            base.Sync(context);
        }
    }
}
