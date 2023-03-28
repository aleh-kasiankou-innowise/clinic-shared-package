using System.Reflection;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public class DefaultMapper : CustomSqlMapper
{
   

    public DefaultMapper(Func<Type, string> tableNameMapper, Func<Type, PropertyInfo, string> propertyMapper) : base(tableNameMapper, propertyMapper)
    {
    }
}