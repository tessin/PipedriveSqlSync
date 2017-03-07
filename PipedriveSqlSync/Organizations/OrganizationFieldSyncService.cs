using System;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Fields;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Organizations
{
    public class OrganizationFieldSyncService : FieldSyncService<DbOrganizationField>
    {
        private readonly ILogger _logger;

        public OrganizationFieldSyncService(ILogger logger = null)
        {
            _logger = logger;
        }

        public override async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading organization fields...");
            Entities = await new OrganizationFieldEntityService<DbOrganizationField>(client).GetAllAsync();
        }

        public override void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing organization fields...");
            base.Sync(context);
        }
    }
}
