namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record DoctorAddedMessage(Guid DoctorId, Guid SpecializationId, Guid OfficeId);

public record DoctorUpdatedMessage(Guid DoctorId, Guid SpecializationId, Guid OfficeId);

