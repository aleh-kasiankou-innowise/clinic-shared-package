using Innowise.Clinic.Shared.BaseClasses;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public static class EntityMappingHelper
{
    public static IEnumerable<Type> GetAllEntities()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.GetInterface(nameof(IEntity)) is not null);
    }

    public static void AddToTableDictionary(string tableName, Type entityType, Dictionary<string, Type> tableDictionary)
    {
    }

    public static void AddToPropertyDictionary()
    {
    }
}