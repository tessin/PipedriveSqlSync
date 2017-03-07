using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Users;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Activities
{
    public class ActivitySyncService : ISyncService<DbActivity>
    {
        private readonly UserSyncService _userService;
        private readonly ILogger _logger;

        public IEnumerable<DbActivity> Entities { get; private set; }

        public ActivitySyncService(UserSyncService userService, ILogger logger = null)
        {
            if (userService == null) throw new ArgumentNullException(nameof(userService));

            _userService = userService;
            _logger = logger;
        }

        public async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading activities...");
            Entities = await new ActivityEntityService<DbActivity>(client).GetAllAsync();
        }

        public void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing activities...");
            foreach (var activity in Entities)
            {
                context.Entry(activity).State = EntityState.Added;
                if (activity.UserId.HasValue)
                    activity.User = _userService.Entities.Single(user => user.Id == activity.UserId.Value);
                if (activity.AssignedToUserId.HasValue)
                    activity.AssignedToUser = _userService.Entities.Single(user => user.Id == activity.AssignedToUserId.Value);
                if (activity.CreatedByUserId.HasValue)
                    activity.CreatedByUser = _userService.Entities.Single(user => user.Id == activity.CreatedByUserId.Value);
            }
        }
    }
}
