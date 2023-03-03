namespace Innowise.Clinic.Profiles.Services.MassTransitService.MessageTypes;

public record EmployeeHiredMessage(Guid ProfileId, String Role, String Email);