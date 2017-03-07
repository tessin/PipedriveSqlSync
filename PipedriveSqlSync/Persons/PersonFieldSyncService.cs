using System;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Fields;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Persons
{
    public class PersonFieldSyncService : FieldSyncService<DbPersonField>
    {
        private readonly ILogger _logger;

        public PersonFieldSyncService(ILogger logger = null)
        {
            _logger = logger;
        }

        public override async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading person fields...");
            Entities = await new PersonFieldEntityService<DbPersonField>(client).GetAllAsync();
        }

        public override void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing person fields...");
            base.Sync(context);
        }
    }
}
