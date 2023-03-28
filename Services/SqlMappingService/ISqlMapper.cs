using System.Reflection;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public interface ISqlMapper
{
    string GetSqlPropertyName(Type type, PropertyInfo property);
    string GetSqlTableName(Type modelType);
    PropertyInfo GetProperty(Type type, string columnName);
    Type GetTableType(string tableName);
    Dictionary<PropertyInfo, string> GetPropertyMappings(Type type);
}