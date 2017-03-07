using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PipedriveSqlSync.Activities;
using PipedriveSqlSync.Deals;
using PipedriveSqlSync.Notes;
using PipedriveSqlSync.Organizations;
using PipedriveSqlSync.Persons;
using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Dynamics;
using PipedriveSqlSync.Users;
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
            _logger?.Info("Cleaning the database...");
            await CleanDatabase();
            _logger?.Info("Database is cleaned.");

            _logger?.Info("Creating synchronization services...");
            var client = new PipeDriveClient(_pipedriveApiKey);
            var syncServices = CreateSyncServices();
            _logger?.Info("Synchronization services are created.");

            _logger?.Info("Loading dynamic data types...");
            var dynamicConfigurations = new List<DynamicEntityConfiguration>();
            foreach (var syncService in syncServices.OfType<IDynamicSyncService>())
            {
                dynamicConfigurations.Add(await syncService.ConstructType(client));
            }
            _logger?.Info("Dynamic data types are loaded.");

            _logger?.Info("Loading data from API...");
            foreach (var syncService in syncServices)
            {
                await syncService.Fetch(client);
            }
            _logger?.Info("API data is loaded.");

            _logger?.Info("Synchronizing data with the database...");
            using (var context = new PipeDriveDbContext(_sqlConnectionString, dynamicConfigurations))
            {
                foreach (var syncService in syncServices)
                {
                    syncService.Sync(context);
                }

                _logger?.Info("Saving to the database...");
                await context.SaveChangesAsync();
            }
            _logger?.Info("Data is synchronized.");
            _logger?.Info("Success.");
        }

        private ISyncService[] CreateSyncServices()
        {
            var userService = new UserSyncService(_logger);
            var orgService = new OrganizationSyncService(userService, _logger);
            var personService = new PersonSyncService(userService, _logger);
            var dealService = new DealSyncService(userService, _logger);
            var activityService = new ActivitySyncService(userService, _logger);
            var noteService = new NoteSyncService(userService, _logger);
            var orgRelationshipService = new OrganizationRelationshipSyncService(orgService, _logger);

            return new ISyncService[]
            {
                userService, orgService, personService, dealService, activityService, noteService,
                orgRelationshipService,
                new ActivityFieldSyncService(_logger),
                new ActivityTypeSyncService(_logger),
                new PersonFieldSyncService(_logger), 
                new DealFieldSyncService(_logger),
                new NoteFieldSyncService(_logger), 
                new OrganizationFieldSyncService(_logger)
            };
        }

        private async Task CleanDatabase()
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                await connection.OpenAsync();

                // http://stackoverflow.com/a/1473313/860913

                // Drop all Foreign Key constraints
                using (var command = new SqlCommand(@"
                    DECLARE @name VARCHAR(128)
                    DECLARE @constraint VARCHAR(254)
                    DECLARE @SQL VARCHAR(254)

                    SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME)

                    WHILE @name is not null
                    BEGIN
                        SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
                        WHILE @constraint IS NOT NULL
                        BEGIN
                            SELECT @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint) +']'
                            EXEC (@SQL)
                            PRINT 'Dropped FK Constraint: ' + @constraint + ' on ' + @name
                            SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
                        END
                    SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME)
                    END", 
                    connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                // Drop all Primary Key constraints
                using (var command = new SqlCommand(@"
                    DECLARE @name VARCHAR(128)
                    DECLARE @constraint VARCHAR(254)
                    DECLARE @SQL VARCHAR(254)

                    SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY TABLE_NAME)

                    WHILE @name IS NOT NULL
                    BEGIN
                        SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
                        WHILE @constraint is not null
                        BEGIN
                            SELECT @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT [' + RTRIM(@constraint)+']'
                            EXEC (@SQL)
                            PRINT 'Dropped PK Constraint: ' + @constraint + ' on ' + @name
                            SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
                        END
                    SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'PRIMARY KEY' ORDER BY TABLE_NAME)
                    END",
                    connection))
                {
                    await command.ExecuteNonQueryAsync();
                }

                // Drop all tables
                using (var command = new SqlCommand(@"
                    DECLARE @name VARCHAR(128)
                    DECLARE @SQL VARCHAR(254)

                    SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 ORDER BY [name])

                    WHILE @name IS NOT NULL
                    BEGIN
                        SELECT @SQL = 'DROP TABLE [dbo].[' + RTRIM(@name) +']'
                        EXEC (@SQL)
                        PRINT 'Dropped Table: ' + @name
                        SELECT @name = (SELECT TOP 1 [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 AND [name] > @name ORDER BY [name])
                    END",
                    connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
