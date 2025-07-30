using LighthouseSocial.Application.Dtos;

namespace LighthouseSocial.Application.Contracts;

public interface IPhotoService
{
    Task<Guid> UploadAsync(PhotoDto dto, Stream fileContent, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid photoId, CancellationToken cancellationToken = default);
}