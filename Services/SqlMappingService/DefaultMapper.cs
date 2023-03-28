using System.Reflection;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public class DefaultMapper : CustomSqlMapper
{
    private static readonly Func<Type, string> TableNameMapper = x => x.Name;
    private static readonly Func<Type, PropertyInfo, string> PropertyMapper = (type, prop) =>
        prop.Name;

    public DefaultMapper() : base(TableNameMapper, PropertyMapper)
    {
    }
}