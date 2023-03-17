namespace Innowise.Clinic.Shared.Exceptions;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string entityName, string searchParameter, string searchParameterValue) : base(
        BuildExceptionMessage(entityName, searchParameter, searchParameterValue))
    {
    }

    private static string BuildExceptionMessage(string entityName, string searchParameter, string searchParameterValue)
    {
        return $"The {entityName} with {searchParameter} : {searchParameterValue} is not registered in the system.";
    }
}