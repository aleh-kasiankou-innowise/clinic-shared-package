using System.ComponentModel.DataAnnotations;
using Innowise.Clinic.Shared.Enums;

namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record AppointmentResultChangeEvent([EmailAddress] string PatientEmail ,AppointmentResultChangeType ChangeType, DateTime AppointmentDateTime,
    string PatientFulName, string DoctorFullName, string ServiceName, string Complaints, string Conclusion,
    string Recommendations);