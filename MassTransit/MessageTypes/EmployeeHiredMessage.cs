namespace Innowise.Clinic.Shared.MassTransit.MessageTypes;

public record EmployeeHiredMessage(Guid ProfileId, String Role, String Email);