using System;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Fields;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Notes
{
    public class NoteFieldSyncService : FieldSyncService<DbNoteField>
    {
        private readonly ILogger _logger;

        public NoteFieldSyncService(ILogger logger = null)
        {
            _logger = logger;
        }

        public override async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading note fields...");
            Entities = await new NoteFieldEntityService<DbNoteField>(client).GetAllAsync();
        }

        public override void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing note fields...");
            base.Sync(context);
        }
    }
}
