using PipedriveSqlSync.Users;
using PipeDriveApi;

namespace PipedriveSqlSync.Deals
{
    public class DbDeal : Deal
    {
        public int DbCreatorUserId
        {
            get { return CreatorUserId?.Value ?? -1; }
            set { CreatorUserId = new Owner { Value = value }; }
        }

        public DbUser CreatorUser { get; set; }

        public int? DbUserId
        {
            get { return UserId?.Value ?? -1; }
            set { UserId = value.HasValue ? new Owner { Value = value.Value } : null; }
        }

        public DbUser User { get; set; }

        public int DbPersonId
        {
            get { return PersonId?.Value ?? -1; }
            set { PersonId = new PersonId { Value = value }; }
        }

        public int DbOrgId
        {
            get { return OrgId?.Id ?? -1; }
            set { OrgId = new OrganizationId { Id = value }; }
        }
    }
}
