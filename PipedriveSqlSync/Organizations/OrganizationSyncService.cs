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

namespace PipedriveSqlSync.Organizations
{
    public class OrganizationSyncService : IDynamicSyncService<DbOrganization>
    {
        private readonly UserSyncService _userService;
        private readonly ILogger _logger;
        private readonly EntityTypeBuilder _entityTypeBuilder = new EntityTypeBuilder();
        private Type _organizationType;
        private IEnumerable<Field> _customFields;

        public IEnumerable<DbOrganization> Entities { get; private set; }

        public OrganizationSyncService(UserSyncService userService, ILogger logger = null)
        {
            if (userService == null) throw new ArgumentNullException(nameof(userService));

            _userService = userService;
            _logger = logger;
        }

        public async Task<DynamicEntityConfiguration> ConstructType(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var service = new OrganizationFieldEntityService<Field>(client);
            _logger?.Info("Loading organization fields...");
            var fields = await service.GetAllAsync();

            _logger?.Info("Constructing organization data type...");
            _customFields = fields
                .Where(field => Regex.IsMatch(field.Key ?? "", "^[0-9A-Fa-f]{40}")) // custom field's Key starts with 40 hex chars
                .ToArray();

            _organizationType = _entityTypeBuilder.Build(typeof(DbOrganization), _customFields);

            return new DynamicEntityConfiguration(typeof(OrganizationEntityConfiguration<>), _organizationType);
        }

        public async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading organizations...");
            Entities = await new DynamicEntityService<DbOrganization>(
                    client, typeof(OrganizationEntityService<>), _organizationType)
                .GetAllAsync();

            _entityTypeBuilder.FillIdsForComplexTypes(Entities, _customFields);
        }

        public void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing organizations...");
            foreach (var organization in Entities)
            {
                context.Entry((object)organization).State = EntityState.Added;
                organization.Owner = _userService.Entities.Single(user => user.Id == organization.DbOwnerId);
            }
        }
    }
}
