using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Users;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Notes
{
    public class NoteSyncService : ISyncService<DbNote>
    {
        private readonly UserSyncService _userService;
        private readonly ILogger _logger;

        public IEnumerable<DbNote> Entities { get; private set; }

        public NoteSyncService(UserSyncService userService, ILogger logger = null)
        {
            if (userService == null) throw new ArgumentNullException(nameof(userService));

            _userService = userService;
            _logger = logger;
        }

        public async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading notes...");
            Entities = await new NoteEntityService<DbNote>(client).GetAllAsync();
        }

        public void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing notes...");
            foreach (var activity in Entities)
            {
                context.Entry(activity).State = EntityState.Added;
                if (activity.LastUpdateUserId.HasValue)
                {
                    activity.LastUpdateUser =
                        _userService.Entities.Single(user => user.Id == activity.LastUpdateUserId.Value);
                }
            }
        }
    }
}
