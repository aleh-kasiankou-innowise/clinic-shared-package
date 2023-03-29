using System.Reflection;
using Innowise.Clinic.Shared.BaseClasses;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public abstract class CustomSqlMapper : ISqlMapper
{
    private readonly Dictionary<KeyValuePair<string, Type>, PropertyInfo> _propertyMap = new();
    private readonly Dictionary<string, Type> _tableMap = new();

    // potentially can immediately map to sql members e.g. instead of productId save product.productId
    public CustomSqlMapper(Func<Type, string> tableNameMapper, Func<Type, PropertyInfo, string> propertyMapper)
    {
        var entities = EntityMappingHelper.GetAllEntities();

        Console.WriteLine("Registered the following models for SQL mapping:");
        foreach (var entity in entities)
        {
            Console.WriteLine(entity.FullName);

            _tableMap.Add(tableNameMapper(entity), entity);
            foreach (var property in entity.GetProperties())
            {
                if (property.PropertyType is not IEntity)
                {
                    var columnName = propertyMapper(entity, property);
                    Console.WriteLine($"Registering property mapping: {property.Name} - {columnName}");
                    _propertyMap.Add(new(columnName, entity), property);
                }
            }
        }
    }

    public string GetSqlPropertyName(Type type, PropertyInfo property)
    {
        return _propertyMap
            .Single(x => x.Key.Value == type && x.Value == property)
            .Key.Key;
    }

    public string GetSqlTableName(Type modelType)
    {
        return _tableMap.Single(x => x.Value == modelType).Key;
    }

    public PropertyInfo GetProperty(Type type, string columnName)
    {
        return _propertyMap[new(columnName, type)];
    }

    public Type GetTableType(string tableName)
    {
        return _tableMap[tableName];
    }

    public Dictionary<PropertyInfo, string> GetPropertyMappings(Type type)
    {
        var typeSpecificMappings = new Dictionary<PropertyInfo, string>();
        foreach (var mapping in _propertyMap.Where(x => x.Key.Value == type))
        {
            typeSpecificMappings.Add(mapping.Value, mapping.Key.Key);
        }

        return typeSpecificMappings;
    }
}