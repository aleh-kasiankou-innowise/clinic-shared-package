namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

public record PatientNameRequest(Guid PatientId);

public record PatientNameResponse(string PatientFullName);

public record DoctorNameRequest(Guid DoctorId);

public record DoctorNameResponse(string DoctorFullName);

public record ServiceNameRequest(Guid ServiceId);

public record ServiceNameResponse(string ServiceName);