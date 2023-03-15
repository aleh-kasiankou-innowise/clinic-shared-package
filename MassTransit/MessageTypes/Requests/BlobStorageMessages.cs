namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

public record BlobUploadRequest(byte[] FileContent, string FileType, string FileCategory);

public record BlobUploadResponse(bool IsSuccessful, string? FileUrl, string? FailReason);

public record BlobDeletionRequest(string FileUrl);

public record BlobDeletionResponse(bool IsSuccessful, string? FailReason);

public record BlobUpdateRequest(byte[] FileContent, string FileType, string FileUrl);

public record BlobUpdateResponse(bool IsSuccessful, string? FailReason);