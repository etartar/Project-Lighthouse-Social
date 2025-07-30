using LighthouseSocial.Application.Common;
using LighthouseSocial.Domain.Interfaces;

namespace LighthouseSocial.Application.Features.Photos.DeletePhoto;

public class DeletePhotoHandler(IPhotoRepository repository, IPhotoStorageService storage)
{
    public async Task<Result> HandleAsync(Guid photoId)
    {
        var photo = await repository.GetByIdAsync(photoId);

        if (photo == null)
        {
            return Result.Failure("Photo not found.");
        }

        await storage.DeleteAsync(photo.Filename);

        await repository.DeleteAsync(photoId);

        return Result.Success();
    }
}
