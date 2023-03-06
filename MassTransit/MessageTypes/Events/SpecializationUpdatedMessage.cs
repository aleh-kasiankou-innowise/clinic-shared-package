using Innowise.Clinic.Shared.Dto;
using Innowise.Clinic.Shared.Enums;

namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;

public record SpecializationUpdatedMessage(SpecializationChange TaskType, SpecializationDto SpecializationDto);