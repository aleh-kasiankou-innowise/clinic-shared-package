namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record PatientCreatedProfileMessage(Guid ProfileId, Guid UserId);