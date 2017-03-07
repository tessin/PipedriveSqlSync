using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PipeDriveApi;

namespace PipedriveSqlSync.Shared.Dynamics
{
    public class DynamicEntityService<TEntity>
    {
        private readonly IPipeDriveClient _client;
        private readonly Type _serviceType;
        private readonly Type _entityType;

        public DynamicEntityService(IPipeDriveClient client, Type serviceType, Type entityType)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            _client = client;
            _serviceType = serviceType;
            _entityType = entityType;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            dynamic entityService = Activator.CreateInstance(_serviceType.MakeGenericType(_entityType), _client);

            var entities = await entityService.GetAllAsync();

            var result = new List<TEntity>();
            foreach (var entity in entities)
            {
                result.Add(entity);
            }

            return result;
        }
    }
}
