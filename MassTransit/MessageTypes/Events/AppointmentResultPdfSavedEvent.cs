namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record AppointmentResultPdfSavedEvent(Guid AppointmentId, string FileUrl);