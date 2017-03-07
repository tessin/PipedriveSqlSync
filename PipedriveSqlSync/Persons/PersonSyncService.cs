using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PipedriveSqlSync.Organizations;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Dynamics;
using PipedriveSqlSync.Users;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Persons
{
    public class PersonSyncService : IDynamicSyncService<DbPerson>
    {
        private readonly UserSyncService _userService;
        private readonly ILogger _logger;
        private readonly EntityTypeBuilder _entityTypeBuilder = new EntityTypeBuilder();
        private Type _userType;
        private IEnumerable<Field> _customFields;

        public IEnumerable<DbPerson> Entities { get; private set; }

        public PersonSyncService(UserSyncService userService, ILogger logger = null)
        {
            if (userService == null) throw new ArgumentNullException(nameof(userService));

            _userService = userService;
            _logger = logger;
        }

        public async Task<DynamicEntityConfiguration> ConstructType(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var service = new PersonFieldEntityService<Field>(client);
            _logger?.Info("Loading person fields...");
            var fields = await service.GetAllAsync();

            _logger?.Info("Constructing person data type...");
            _customFields = fields
                .Where(field => Regex.IsMatch(field.Key ?? "", "^[0-9A-Fa-f]{40}")); // custom field's Key starts with 40 hex chars

            _userType = _entityTypeBuilder.Build(typeof(DbPerson), _customFields);

            return new DynamicEntityConfiguration(typeof(PersonEntityConfiguration<>), _userType);
        }

        public async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading persons...");
            Entities = await new DynamicEntityService<DbPerson>(
                    client, typeof(PersonEntityService<>), _userType)
                .GetAllAsync();

            _entityTypeBuilder.FillIdsForComplexTypes(Entities, _customFields);
        }

        public void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing persons...");
            foreach (var person in Entities)
            {
                context.Entry((object)person).State = EntityState.Added;
                person.Owner = _userService.Entities.Single(user => user.Id == person.DbOwnerId);
            }
        }
    }
}
