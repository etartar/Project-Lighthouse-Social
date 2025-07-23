namespace LighthouseSocial.Application.Dtos;

public record PhotoDto(
    Guid Id,
    Guid UserId,
    Guid LighthouseId,
    string Filename,
    DateTime UploadedAt,
    string Lens,
    string Resolution,
    string CameraModel,
    DateTime TakenAt);