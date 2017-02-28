using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Database;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.EntitySyncs
{
    public class DealEntitySync : EntitySync<DbDeal>
    {
        public DealEntitySync(
            PagingEntityService<DbDeal> entityService, DbSet<DbDeal> dbSet, ILogger logger = null)
            : base(entityService, dbSet, logger)
        {
        }

        public void SetNavigationProperties(IEnumerable<DbOrganization> orgs, IEnumerable<DbPerson> persons)
        {
            Logger?.Info("Linking deals to other entities...");
            foreach (var deal in Entities)
            {
                deal.Organization = orgs.Single(org => org.Id == deal.OrgId);
                deal.Person = persons.Single(per => per.Id == deal.PersonId);
            }
            Logger?.Info("Deals linked.");
        }

        public override async Task Load()
        {
            Logger?.Info("Loading deals...");
            await base.Load();
            Logger?.Info($"Loaded {Entities.Count} deals...");
        }


        public override async Task Clean()
        {
            Logger?.Info("Cleaning deals from database...");
            await base.Clean();
            Logger?.Info("Deals cleaned.");
        }
    }
}
