using LighthouseSocial.Application.Contracts;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Application.Features.Photos.DeletePhoto;
using LighthouseSocial.Application.Features.Photos.UploadPhoto;

namespace LighthouseSocial.Application.Services;

internal sealed class PhotoService(
    UploadPhotoHandler uploadPhotoHandler,
    DeletePhotoHandler deletePhotoHandler) : IPhotoService
{
    public async Task<Guid> UploadAsync(PhotoDto dto, Stream fileContent, CancellationToken cancellationToken = default)
    {
        var result = await uploadPhotoHandler.HandleAsync(dto, fileContent);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to upload photo: {result.ErrorMessage}");
        }

        return result.Data;
    }

    public async Task DeleteAsync(Guid photoId, CancellationToken cancellationToken = default)
    {
        var result = await deletePhotoHandler.HandleAsync(photoId);
    }
}
