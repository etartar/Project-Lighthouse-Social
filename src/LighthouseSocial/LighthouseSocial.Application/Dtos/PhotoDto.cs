namespace LighthouseSocial.Application.Dtos;

public record PhotoDto(
    Guid Id,
    Guid UserId,
    Guid LighthouseId,
    string FileName,
    DateTime UploadedAt,
    string CameraType);