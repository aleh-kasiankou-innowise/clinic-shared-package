namespace Innowise.Clinic.Profiles.Services.MassTransitService.MessageTypes;

public record DoctorAccountStatusChangedMessage(Guid AccountId, bool IsActiveStatus);