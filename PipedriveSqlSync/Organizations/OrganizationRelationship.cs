using PipeDriveApi;

namespace PipedriveSqlSync.Organizations
{
    public class DbOrganizationRelationship : OrganizationRelationship
    {
        public int OwnerOrganizationId
        {
            get { return RelOwnerOrgId?.Id ?? -1; }
            set { RelOwnerOrgId = new OrganizationId { Id = value }; }
        }

        public int LinkedOrganizationId
        {
            get { return RelLinkedOrgId?.Id ?? -1; }
            set { RelLinkedOrgId = new OrganizationId { Id = value }; }
        }
    }
}
