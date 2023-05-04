using Innowise.Clinic.Shared.Enums;

namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record AppointmentResultPdfGeneratedEvent(Guid AppointmentId, AppointmentResultChangeType ActionType,
    byte[] File);