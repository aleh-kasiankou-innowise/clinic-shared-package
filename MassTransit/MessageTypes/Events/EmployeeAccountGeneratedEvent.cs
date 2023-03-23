namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record EmployeeAccountGeneratedEvent(string Email, string Password, string Role);