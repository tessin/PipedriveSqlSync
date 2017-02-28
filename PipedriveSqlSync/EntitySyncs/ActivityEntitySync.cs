using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Database;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.EntitySyncs
{
    public class ActivityEntitySync : EntitySync<DbActivity>
    {
        public ActivityEntitySync(
            PagingEntityService<DbActivity> entityService, DbSet<DbActivity> dbSet, ILogger logger = null)
            : base(entityService, dbSet, logger)
        {
        }

        public void SetNavigationProperties(
            IEnumerable<DbOrganization> orgs, IEnumerable<DbPerson> persons, IEnumerable<DbDeal> deals)
        {
            Logger?.Info("Linking activities to other entities...");
            foreach (var act in Entities)
            {
                act.Organization = act.OrgId.HasValue ? orgs.Single(org => org.Id == act.OrgId.Value) : null;
                act.Person = act.PersonId.HasValue ? persons.Single(per => per.Id == act.PersonId.Value) : null;
                act.Deal = act.DealId.HasValue ? deals.Single(deal => deal.Id == act.DealId.Value) : null;
            }
            Logger?.Info("Activities linked.");
        }

        public override async Task Load()
        {
            Logger?.Info("Loading activities...");
            await base.Load();
            Logger?.Info($"Loaded {Entities.Count} activities...");
        }


        public override async Task Clean()
        {
            Logger?.Info("Cleaning activities from database...");
            await base.Clean();
            Logger?.Info("Activities cleaned.");
        }
    }
}
