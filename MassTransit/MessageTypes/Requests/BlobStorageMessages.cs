using Innowise.Clinic.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;

public record BlobUploadRequest(FormFile File, string FileCategory);

public record BlobUploadResponse(bool IsSuccessful, string? FileUrl, string? FailReason);

public record BlobDeletionRequest(string FileUrl);

public record BlobDeletionResponse(bool IsSuccessful, string? FailReason);

public record BlobUpdateRequest(FormFile NewFile, string FileUrl);

public record BlobUpdateResponse(bool IsSuccessful, string? FailReason);