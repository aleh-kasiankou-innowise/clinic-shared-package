namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

public record TimeSlotReservationRequest(Guid DoctorId, DateTime AppointmentStart, DateTime AppointmentEnd);

public record TimeSlotReservationResponse(bool IsSuccessful, Guid? ReservedTimeSlotId, string? FailReason);