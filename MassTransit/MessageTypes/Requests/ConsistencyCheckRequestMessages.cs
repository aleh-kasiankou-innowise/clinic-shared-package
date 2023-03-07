using Innowise.Clinic.Shared.Exceptions;

namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

public abstract record ConsistencyCheckResponse(bool IsSuccessful, string? FailReason)
{
    public void CheckIfDataIsConsistent()
    {
        if (!IsSuccessful)
        {
            throw FailReason is null ? new InconsistentDataException() : new InconsistentDataException(FailReason);
        }
    }
}

public record ProfileExistsAndHasRoleRequest(Guid ProfileId, string Role);

public record ProfileExistsAndHasRoleResponse(bool IsSuccessful, string? FailReason) : ConsistencyCheckResponse(
    IsSuccessful, FailReason);

public record ServiceExistsAndBelongsToSpecializationRequest(Guid ServiceId, Guid SpecializationId);

public record ServiceExistsAndBelongsToSpecializationResponse(bool IsSuccessful, string? FailReason) :
    ConsistencyCheckResponse(
        IsSuccessful, FailReason);
