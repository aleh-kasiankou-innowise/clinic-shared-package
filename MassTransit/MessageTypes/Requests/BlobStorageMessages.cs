namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

public record BlobSaveRequest(Guid FileId, string FileCategory, byte[] FileContent, string FileType);

public record BlobSaveResponse(bool IsSuccessful, string? FileUrl, string? FailReason);

public record BlobDeletionRequest(string FileUrl);

public record BlobDeletionResponse(bool IsSuccessful, string? FailReason);