using System.Data.Entity.Migrations;

namespace PipedriveSqlSync.Migrations
{
    sealed class Configuration : DbMigrationsConfiguration<PipedriveDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PipedriveDbContext context) {}
    }
}
