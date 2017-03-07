using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Fields;

namespace PipedriveSqlSync.Deals
{
    public class DealFieldEntityConfiguration : FieldEntityConfiguration<DbDealField>
    {
        public DealFieldEntityConfiguration() : base("DealFields") { }
    }
}
