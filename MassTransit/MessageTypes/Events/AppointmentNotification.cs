namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record AppointmentNotification(Guid AppointmentId, Guid PatientId, Guid DoctorId, Guid ServiceId, DateTime AppointmentDateTime);