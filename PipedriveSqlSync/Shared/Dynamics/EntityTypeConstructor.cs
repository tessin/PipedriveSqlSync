using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using PipeDriveApi;

namespace PipedriveSqlSync.Shared.Dynamics
{
    public class EntityTypeBuilder
    {
        private readonly Type[] _complexTypes = { typeof(UserId), typeof(OrganizationId), typeof(PersonId) };

        public Type Build(Type baseType, IEnumerable<Field> customFields)
        {
            if (!customFields.Any())
                return baseType;

            var assemblyName = new AssemblyName(GetUniqueName(nameof(EntityTypeBuilder)));
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule(GetUniqueName(nameof(EntityTypeBuilder)));
            var typeBuilder = moduleBuilder.DefineType(
                GetUniqueName(baseType.Name), TypeAttributes.Public | TypeAttributes.Class, baseType);

            foreach (var field in customFields)
            {
                var property = AddAutoProperty(
                    typeBuilder, GetName(field, customFields), GetTypeFromTypeName(field.FieldType));

                property.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(CustomFieldAttribute).GetConstructor(new[] { typeof(string) }),
                    new object[] { field.Key }));

                if (_complexTypes.Contains(property.PropertyType))
                {
                    property.SetCustomAttribute(new CustomAttributeBuilder(
                        typeof(NotMappedAttribute).GetConstructor(new Type[0]), new object[0]));

                    AddAutoProperty(typeBuilder, property.Name + "Id", typeof(int?));
                }
            }

            return typeBuilder.CreateType();
        }

        public void FillIdsForComplexTypes(IEnumerable<BaseEntity> entities, IEnumerable<Field> customFields)
        {
            foreach (var entity in entities)
            {
                var type = entity.GetType();
                var complexTypeProperties = type.GetProperties()
                    .Where(prop => 
                        _complexTypes.Contains(prop.PropertyType) && 
                        customFields.Any(field => prop.Name == GetName(field, customFields)));

                foreach (var property in complexTypeProperties)
                {
                    var idProperty = type.GetProperty(property.Name + "Id");
                    var complexTypeInstance = property.GetValue(entity);

                    if (complexTypeInstance != null)
                    {
                        var innerId = property.PropertyType.GetProperty("Value").GetValue(complexTypeInstance);
                        idProperty.SetValue(entity, innerId);
                    }
                    else
                    {
                        idProperty.SetValue(entity, null);
                    }
                }
            }
        }

        private static string GetUniqueName(string prefix) => $"{prefix}_{Guid.NewGuid():N}";

        // https://benohead.com/create-anonymous-types-at-runtime-in-c-sharp/
        private static PropertyBuilder AddAutoProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;

            var field = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            var property = typeBuilder.DefineProperty(
                propertyName, PropertyAttributes.None, propertyType, new[] { propertyType });

            var getMethodBuilder = typeBuilder.DefineMethod("get_value", getSetAttr, propertyType, Type.EmptyTypes);
            var getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, field);
            getIl.Emit(OpCodes.Ret);

            var setMethodBuilder = typeBuilder.DefineMethod("set_value", getSetAttr, null, new[] { propertyType });
            var setIl = setMethodBuilder.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, field);
            setIl.Emit(OpCodes.Ret);

            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);

            return property;
        }

        private static Type GetTypeFromTypeName(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new UnknownTypeException("Type cannot be null or whitespace.");

            switch (type.ToLower())
            {
                case "varchar":
                case "varchar_auto":
                case "text":
                case "set":
                case "enum":
                case "phone":
                    return typeof(string);
                case "double":
                    return typeof(double);
                case "monetary":
                    return typeof(decimal);
                case "user":
                    return typeof(UserId);
                case "org":
                    return typeof(OrganizationId);
                case "people":
                    return typeof(PersonId);
                case "time":
                case "timerange":
                    return typeof(TimeSpan);
                case "int":
                    return typeof(int);
                case "date":
                case "daterange":
                    return typeof(DateTime);
                default:
                    throw new UnknownTypeException($"Unknown PipeDriveApi field type '{type.ToLower()}'");
            }
        }

        private static string GetName(Field field, IEnumerable<Field> fields)
        {
            var name = field.Name;
            if (string.Equals(name, "Timezone", StringComparison.OrdinalIgnoreCase))
            {
                var match = Regex.Match(field.Key, "^([0-9A-Fa-f]{40})_timezone_id");
                if (match.Success)
                {
                    var mainField = fields.FirstOrDefault(f =>
                        string.Equals(f.Key, match.Groups[1].Value, StringComparison.OrdinalIgnoreCase));

                    if (mainField != null)
                    {
                        name = mainField.Name + " Timezone";
                    }
                }
            }
            return NormalizePropertyName(name);
        }

        //TODO: remove digits from beginning, replace spaces or snake_case by CamelCase
        private static string NormalizePropertyName(string name) => Regex.Replace(name, "[^0-9A-Za-z]*", "");
    }

    public class UnknownTypeException : Exception
    {
        public UnknownTypeException(string message) : base(message) { }
    }
}
