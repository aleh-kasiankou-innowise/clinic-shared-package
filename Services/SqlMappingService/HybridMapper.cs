using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public class HybridMapper : CustomSqlMapper
{
    private static readonly Func<Type, string> TableNameMapper = MapTableName;
    private static readonly Func<Type, PropertyInfo, string> PropertyMapper = (type, prop) =>
        prop.Name;

    public HybridMapper() : base(TableNameMapper, PropertyMapper)
    {
    }

    private static string MapTableName(Type entityType)
    {
        var entityTableAttribute = entityType.GetCustomAttribute<TableAttribute>();
        return entityTableAttribute?.Name ?? entityType.Name;
    }

    private static string MapTableProperty(Type entityType, PropertyInfo entityPropertyInfo)
    {
        var propertyColumnAttribute = entityPropertyInfo.GetCustomAttribute<ColumnAttribute>();
        return propertyColumnAttribute?.Name ?? entityPropertyInfo.Name;
    }
}