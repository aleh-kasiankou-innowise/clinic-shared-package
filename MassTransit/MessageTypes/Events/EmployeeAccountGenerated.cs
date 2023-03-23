namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record EmployeeAccountGenerated(string Email, string Password, string Role);