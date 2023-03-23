namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record PatientAccountCreatedEvent(string Email, string EmailConfirmationLink);