using System;

namespace PipedriveSqlSync.Shared.Dynamics
{
    public class DynamicEntityConfiguration
    {
        public Type EntityConfigurationGenericType { get; set; }
        public Type EntityType { get; set; }

        public DynamicEntityConfiguration(Type entityConfigurationGenericType, Type entityType)
        {
            EntityConfigurationGenericType = entityConfigurationGenericType;
            EntityType = entityType;
        }
    }
}