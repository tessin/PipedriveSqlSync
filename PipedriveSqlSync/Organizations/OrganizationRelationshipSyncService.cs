using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Shared;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.Organizations
{
    public class OrganizationRelationshipSyncService : ISyncService<DbOrganizationRelationship>
    {
        private readonly OrganizationSyncService _orgService;
        private readonly ILogger _logger;

        public IEnumerable<DbOrganizationRelationship> Entities { get; private set; }

        public OrganizationRelationshipSyncService(OrganizationSyncService orgService, ILogger logger = null)
        {
            if (orgService == null) throw new ArgumentNullException(nameof(orgService));

            _orgService = orgService;
            _logger = logger;
        }

        public async Task Fetch(IPipeDriveClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            _logger.Info("Loading organizations relationships...");
            var service = new OrganizationRelationshipEntityService<DbOrganizationRelationship>(client);
            var entities = new List<DbOrganizationRelationship>();
            foreach (var organization in _orgService.Entities)
            {
                var relationships = await service.GetAllForOrganizationAsync(organization.Id);
                var unique = relationships.Where(rel => entities.All(ent => ent.Id != rel.Id));
                entities.AddRange(unique);
            }
            Entities = entities.ToArray();
        }

        public void Sync(PipeDriveDbContext context)
        {
            _logger?.Info("Synchronizing organizations relationships...");
            foreach (var activity in Entities)
            {
                context.Entry(activity).State = EntityState.Added;
            }
        }
    }
}
