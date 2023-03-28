using System.Reflection;
using Innowise.Clinic.Shared.BaseClasses;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public abstract class CustomSqlMapper : ISqlMapper
{
    private readonly Dictionary<KeyValuePair<string, Type>, PropertyInfo> _propertyMap = new();
    private readonly Dictionary<string, Type> _tableMap = new();

    // potentially can immediately map to sql members e.g. instead of productId save product.productId
    public CustomSqlMapper(Func<Type, string> tableNameMapper, Func<Type,PropertyInfo, string> propertyMapper)
    {
        var entities = EntityMappingHelper.GetAllEntities();
        
        foreach (var entity in entities)
        {
            _tableMap.Add(tableNameMapper(entity), entity);
            foreach (var property in entity.GetProperties())
            {
                if (property.PropertyType is not IEntity)
                {
                    _propertyMap.Add(new(propertyMapper(entity, property), entity), property);
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
}