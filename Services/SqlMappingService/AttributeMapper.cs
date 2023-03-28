using System.Reflection;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public class AttributeMapper : CustomSqlMapper
{
    private static readonly Func<Type, string> TableNameMapper = x => x.Name;

    private static readonly Func<Type, PropertyInfo, string> PropertyMapper = (type, prop) =>
        prop.Name;

    public AttributeMapper() : base(TableNameMapper, PropertyMapper)
    {
    }
}