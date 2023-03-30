using System.Reflection;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public class NameBasedMapper : CustomSqlMapper
{
    private static readonly Func<Type, string> TableNameMapper = x => x.Name;
    private static readonly Func<Type, PropertyInfo, string> PropertyMapper = (type, prop) =>
        prop.Name;

    public NameBasedMapper() : base(TableNameMapper, PropertyMapper)
    {
    }
}