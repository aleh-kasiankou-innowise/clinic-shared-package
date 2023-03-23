using System.ComponentModel.DataAnnotations;

namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record AppointmentRemindEvent([EmailAddress] string PatientEmail, string PatientFullName, DateTime AppointmentDateTime, string ServiceName,
    string DoctorFullName);