using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Database;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.EntitySyncs
{
    public class NoteEntitySync : EntitySync<DbNote>
    {
        public NoteEntitySync(
            PagingEntityService<DbNote> entityService, DbSet<DbNote> dbSet, ILogger logger = null)
            : base(entityService, dbSet, logger)
        {
        }

        public void SetNavigationProperties(
            IEnumerable<DbOrganization> orgs, IEnumerable<DbPerson> persons, IEnumerable<DbDeal> deals)
        {
            Logger?.Info("Linking notes to other entities...");
            foreach (var note in Entities)
            {
                note.Organization = note.OrgId.HasValue ? orgs.Single(org => org.Id == note.OrgId.Value) : null;
                note.Person = note.PersonId.HasValue ? persons.Single(per => per.Id == note.PersonId.Value) : null;
                note.Deal = note.DealId.HasValue ? deals.Single(deal => deal.Id == note.DealId.Value) : null;
            }
            Logger?.Info("Notes linked.");
        }

        public override async Task Load()
        {
            Logger?.Info("Loading notes...");
            await base.Load();
            Logger?.Info($"Loaded {Entities.Count} notes...");
        }


        public override async Task Clean()
        {
            Logger?.Info("Cleaning notes from database...");
            await base.Clean();
            Logger?.Info("Notes cleaned.");
        }
    }
}
