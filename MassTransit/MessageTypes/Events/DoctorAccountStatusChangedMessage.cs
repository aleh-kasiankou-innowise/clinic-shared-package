namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record DoctorAccountStatusChangedMessage(Guid AccountId, bool IsActiveStatus);