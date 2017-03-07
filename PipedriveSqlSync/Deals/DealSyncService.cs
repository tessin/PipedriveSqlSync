using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Dynamics;
using PipedriveSqlSync.Users;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Deals
{
    public class DealSyncService : IDynamicSyncService<DbDeal>
    {
        private readonly UserSyncService _userService;
        private readonly ILogger _logger;
        private readonly EntityTypeBuilder _entityTypeBuilder = new EntityTypeBuilder();
        private Type _dealType;
        private IEnumerable<Field> _customFields;

        public IEnumerable<DbDeal> Entities { get; private set; }

        public DealSyncService(UserSyncService userService, ILogger logger = null)
        {
            if (userService == null) throw new ArgumentNullException(nameof(userService));

            _userService = userService;
            _logger = logger;
        }

        public async Task<DynamicEntityConfiguration> ConstructType(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var service = new DealFieldEntityService<Field>(client);
            _logger?.Info("Loading deal fields...");
            var fields = await service.GetAllAsync();

            _logger?.Info("Constructing deals data type...");
            _customFields = fields
                .Where(field => Regex.IsMatch(field.Key ?? "", "^[0-9A-Fa-f]{40}")); // custom field's Key starts with 40 hex chars

            _dealType = _entityTypeBuilder.Build(typeof(DbDeal), _customFields);

            return new DynamicEntityConfiguration(typeof(DealEntityConfiguration<>), _dealType);
        }

        public async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading deals...");
            Entities = await new DynamicEntityService<DbDeal>(
                    client, typeof(DealEntityService<>), _dealType)
                .GetAllAsync();

            _entityTypeBuilder.FillIdsForComplexTypes(Entities, _customFields);
        }

        public void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing deals...");
            foreach (var person in Entities)
            {
                context.Entry((object)person).State = EntityState.Added;
                person.CreatorUser = _userService.Entities.Single(user => user.Id == person.DbCreatorUserId);
                person.User = _userService.Entities.Single(user => user.Id == person.DbUserId);
            }
        }
    }
}
