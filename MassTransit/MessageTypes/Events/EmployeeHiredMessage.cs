namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record EmployeeHiredMessage(Guid ProfileId, String Role, String Email);