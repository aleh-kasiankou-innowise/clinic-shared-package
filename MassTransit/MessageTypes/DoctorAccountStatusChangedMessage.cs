namespace Innowise.Clinic.Shared.MassTransit.MessageTypes;

public record DoctorAccountStatusChangedMessage(Guid AccountId, bool IsActiveStatus);