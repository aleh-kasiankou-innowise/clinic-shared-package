namespace Innowise.Clinic.Shared.Exceptions;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string message) : base(message)
    {
        
    }
}