using PipedriveSqlSync.Shared;
using PipedriveSqlSync.Shared.Fields;

namespace PipedriveSqlSync.Persons
{
    public class PersonFieldEntityConfiguration : FieldEntityConfiguration<DbPersonField>
    {
        public PersonFieldEntityConfiguration() : base("PersonFields") { }
    }
}
