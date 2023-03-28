using Innowise.Clinic.Shared.BaseClasses;

namespace Innowise.Clinic.Shared.Services.SqlMappingService;

public static class EntityMappingHelper
{
    private static readonly IEnumerable<Type> Entities;
    public static IEnumerable<Type> GetAllEntities() => Entities;

    static EntityMappingHelper()
    {
        Entities = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.GetInterface(nameof(IEntity)) is not null);
    }
}