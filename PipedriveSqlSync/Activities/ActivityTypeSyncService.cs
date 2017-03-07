using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Activities
{
    public class ActivityTypeSyncService : ISyncService<DbActivityType>
    {
        private readonly ILogger _logger;

        public IEnumerable<DbActivityType> Entities { get; private set; }

        public ActivityTypeSyncService(ILogger logger = null)
        {
            _logger = logger;
        }

        public async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading activity types...");
            Entities = await new ActivityTypeEntityService<DbActivityType>(client).GetAllAsync();
        }

        public void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing activity types...");
            foreach (var activity in Entities)
            {
                context.Entry(activity).State = EntityState.Added;
            }
        }
    }
}
