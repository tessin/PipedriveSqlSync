using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using PipeDriveApi;
using PipeDriveApi.EntityServices;

namespace PipedriveSqlSync.EntitySyncs
{
    public class EntitySync<TEntity> : IEntitySync where TEntity : BaseEntity
    {
        protected readonly PagingEntityService<TEntity> EntityService;
        protected readonly DbSet<TEntity> DbSet;
        protected readonly ILogger Logger;

        public IReadOnlyList<TEntity> Entities { get; protected set; }

        public EntitySync(PagingEntityService<TEntity> entityService, DbSet<TEntity> dbSet, ILogger logger = null)
        {
            if (entityService == null) throw new ArgumentNullException(nameof(entityService));
            if (dbSet == null) throw new ArgumentNullException(nameof(dbSet));

            EntityService = entityService;
            DbSet = dbSet;
            Logger = logger;
        }

        public virtual async Task Load()
        {
            Entities = await EntityService.GetAllAsync();
        }

        public virtual async Task Clean()
        {
            await DbSet.DeleteAsync();
        }

        public virtual Task Sync()
        {
            DbSet.AddRange(Entities);
            return Task.CompletedTask;
        }
    }
}
