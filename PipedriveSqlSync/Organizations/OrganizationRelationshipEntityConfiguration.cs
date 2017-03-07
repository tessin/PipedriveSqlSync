using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PipedriveSqlSync.Organizations
{
    public class OrganizationRelationshipEntityConfiguration : EntityTypeConfiguration<DbOrganizationRelationship> 
    {
        public OrganizationRelationshipEntityConfiguration()
        {
            ToTable("OrganizationRelationships");
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Ignore(x => x.RelOwnerOrgId);
            Property(x => x.OwnerOrganizationId).HasColumnName(nameof(DbOrganizationRelationship.RelOwnerOrgId));

            Ignore(x => x.RelLinkedOrgId);
            Property(x => x.LinkedOrganizationId).HasColumnName(nameof(DbOrganizationRelationship.RelLinkedOrgId));
        }
    }
}