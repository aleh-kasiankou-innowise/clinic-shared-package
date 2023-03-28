namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

public record UserProfileLinkingRequest(Guid ProfileId, Guid UserId);
public record UserProfileLinkingResponse(bool IsSuccessful);
