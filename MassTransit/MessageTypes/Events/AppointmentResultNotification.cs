namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record AppointmentResultNotification(Guid AppointmentId, Guid PatientId, Guid DoctorId, Guid ServiceId, DateTime AppointmentDateTime,
    string Complaints, string Conclusion, string Recommendations);