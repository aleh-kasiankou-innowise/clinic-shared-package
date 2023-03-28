using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public class AttributeMapper : CustomSqlMapper
{
    private static readonly Func<Type, string> TableNameMapper = x => x.GetCustomAttribute<TableAttribute>().Name;
    private static readonly Func<Type, PropertyInfo, string> PropertyMapper = (type, prop) =>
        prop.GetCustomAttribute<ColumnAttribute>().Name;

    public AttributeMapper() : base(TableNameMapper, PropertyMapper)
    {
    }
}