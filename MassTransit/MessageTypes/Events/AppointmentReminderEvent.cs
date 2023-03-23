namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record AppointmentRemindEvent(string PatientFullName, DateTime AppointmentDateTime, string ServiceName,
    string DoctorFullName);