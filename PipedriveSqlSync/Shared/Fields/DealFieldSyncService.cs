using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PipeDriveApi;

namespace PipedriveSqlSync.Shared.Fields
{
    public abstract class FieldSyncService<T> : ISyncService<T> where T : Field
    {
        public IEnumerable<T> Entities { get; protected set; }

        public abstract Task Fetch(IPipeDriveClient client);

        public virtual void Sync(PipeDriveDbContext context)
        {
            foreach (var activity in Entities)
            {
                context.Entry(activity).State = EntityState.Added;
                foreach (var field in Entities)
                {
                    if (field.Id == null)
                        field.Id = Entities.Max(f => f.Id) + 1;
                }
            }
        }
    }
}
