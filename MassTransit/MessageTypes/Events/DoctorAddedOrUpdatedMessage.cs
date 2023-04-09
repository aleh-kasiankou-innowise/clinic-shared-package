namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record DoctorAddedOrUpdatedMessage(Guid DoctorId, Guid SpecializationId, Guid OfficeId);

