namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

public record BlobSaveRequest(Guid FileId, string FileCategory, byte[] FileContent, string FileType);

public record BlobUploadResponse(bool IsSuccessful, string? FileUrl, string? FailReason);

public record BlobDeletionRequest(string FileUrl);

public record BlobDeletionResponse(bool IsSuccessful, string? FailReason);

public record BlobUpdateResponse(bool IsSuccessful, string? FailReason);