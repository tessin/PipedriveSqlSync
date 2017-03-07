using PipedriveSqlSync.Shared.Fields;

namespace PipedriveSqlSync.Organizations
{
    public class OrganizationFieldEntityConfiguration : FieldEntityConfiguration<DbOrganizationField>
    {
        public OrganizationFieldEntityConfiguration() : base("OrganizationFields") { }
    }
}
