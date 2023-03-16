namespace Innowise.Clinic.Shared.Dto;

public class UserProfileLinkingDto
{
    public UserProfileLinkingDto(Guid userId, Guid profileId)
    {
        ProfileId = profileId;
        UserId = userId;
    }

    public Guid ProfileId { get; }
    public Guid UserId { get; }
}