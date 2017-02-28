using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Database;
using PipedriveSqlSync.EntitySyncs;
using PipeDriveApi;

namespace PipedriveSqlSync
{
    public class PipedriveSqlSyncEngine
    {
        private readonly string _pipedriveApiKey;
        private readonly string _sqlConnectionString;
        private readonly ILogger _logger;

        public PipedriveSqlSyncEngine(string pipedriveApiKey, string sqlConnectionString, ILogger logger = null)
        {
            if (pipedriveApiKey == null) throw new ArgumentNullException(nameof(pipedriveApiKey));
            if (sqlConnectionString == null) throw new ArgumentNullException(nameof(sqlConnectionString));

            _pipedriveApiKey = pipedriveApiKey;
            _sqlConnectionString = sqlConnectionString;
            _logger = logger;
        }

        public async Task SynchronizeAsync()
        {
            _logger?.Info("Updating database structure...");
            EnsureDatabaseMigration();
            _logger?.Info("Database structure updated.");

            var client =
                new PipeDriveClient<DbPerson, DbOrganization, DbDeal, Product, DbActivity, DbNote>(_pipedriveApiKey);

            using (var db = new PipedriveDbContext(_sqlConnectionString))
            {
                var orgSync = new EntitySync<DbOrganization>(client.Organizations, db.Organizations, _logger);
                var personSync = new PersonEntitySync(client.Persons, db.Persons, _logger);
                var dealSync = new DealEntitySync(client.Deals, db.Deals, _logger);
                var activitySync = new ActivityEntitySync(client.Activities, db.Activities, _logger);
                var noteSync = new NoteEntitySync(client.Notes, db.Notes, _logger);

                var syncs = new IEntitySync[] { orgSync, personSync, dealSync, activitySync, noteSync };

                await Task.WhenAll(syncs.Select(sync => sync.Load()));
                personSync.SetNavigationProperties(orgSync.Entities);
                dealSync.SetNavigationProperties(orgSync.Entities, personSync.Entities);
                activitySync.SetNavigationProperties(orgSync.Entities, personSync.Entities, dealSync.Entities);
                noteSync.SetNavigationProperties(orgSync.Entities, personSync.Entities, dealSync.Entities);

                // Keep the order to avoid foreign key constraint errors
                await noteSync.Clean();
                await activitySync.Clean();
                await dealSync.Clean();
                await personSync.Clean();
                await orgSync.Clean();

                await Task.WhenAll(syncs.Select(sync => sync.Sync()));

                _logger?.Info("Saving to database...");
                await db.SaveChangesAsync();
                _logger?.Info("Finished.");
            }
        }

        private void EnsureDatabaseMigration()
        {
            var configuration = new Migrations.Configuration
            {
                TargetDatabase = new DbConnectionInfo(_sqlConnectionString, "System.Data.SqlClient")
            };
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}
