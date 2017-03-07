using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Users
{
    public class UserSyncService : ISyncService<DbUser>
    {
        private readonly ILogger _logger;

        public IEnumerable<DbUser> Entities { get; private set; }

        public UserSyncService(ILogger logger = null)
        {
            _logger = logger;
        }

        public async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading users...");
            Entities = await new UserEntityService<DbUser>(client).GetAllAsync();
        }

        public void Sync(PipeDriveDbContext context)
        {
            foreach (var user in Entities)
            {
                context.Entry(user).State = EntityState.Added;
            }
        }
    }
}
