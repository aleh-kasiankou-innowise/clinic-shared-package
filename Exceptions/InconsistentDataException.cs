namespace Innowise.Clinic.Shared.Exceptions;

public class InconsistentDataException : ApplicationException
{
    private const string DefaultExceptionMessage =
        "The request failed due to data inconsistency. Please tyr again later or contact our support team.";

    public InconsistentDataException() : base(DefaultExceptionMessage)
    {
    }

    public InconsistentDataException(string message) : base(message)
    {
    }
}