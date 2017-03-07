using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Fields;

namespace PipedriveSqlSync.Activities
{
    public class ActivityFieldEntityConfiguration : FieldEntityConfiguration<DbActivityField>
    {
        public ActivityFieldEntityConfiguration() : base("ActivityFields") { }
    }
}
