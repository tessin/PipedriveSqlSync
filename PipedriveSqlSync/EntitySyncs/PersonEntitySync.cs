using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Database;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.EntitySyncs
{
    public class PersonEntitySync : EntitySync<DbPerson>
    {
        public PersonEntitySync(
            PagingEntityService<DbPerson> entityService, DbSet<DbPerson> dbSet, ILogger logger = null)
            : base(entityService, dbSet, logger)
        {
        }

        public void SetNavigationProperties(IEnumerable<DbOrganization> orgs)
        {
            Logger?.Info("Linking persons to other entities...");
            foreach (var person in Entities)
            {
                person.Organization = orgs.Single(org => org.Id == person.OrgId);
            }
            Logger?.Info("Persons linked.");
        }

        public override async Task Load()
        {
            Logger?.Info("Loading persons...");
            await base.Load();
            Logger?.Info($"Loaded {Entities.Count} persons...");
        }


        public override async Task Clean()
        {
            Logger?.Info("Cleaning persons from database...");
            await base.Clean();
            Logger?.Info("Persons cleaned.");
        }
    }
}
