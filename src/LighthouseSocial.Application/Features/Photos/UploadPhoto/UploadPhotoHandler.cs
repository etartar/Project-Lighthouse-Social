using LighthouseSocial.Application.Common;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;
using LighthouseSocial.Domain.ValueObjects;

namespace LighthouseSocial.Application.Features.Photos.UploadPhoto;

public class UploadPhotoHandler
{
    private readonly IPhotoStorageService _storageService;
    private readonly IPhotoRepository _photoRepository;

    public UploadPhotoHandler(IPhotoStorageService storageService, IPhotoRepository photoRepository)
    {
        _storageService = storageService;
        _photoRepository = photoRepository;
    }

    public async Task<Result<Guid>> HandleAsync(PhotoDto dto, Stream content)
    {
        if (content == null || content.Length == 0)
        {
            return Result<Guid>.Failure("Photo content is empty");
        }

        var savedPath = await _storageService.SaveAsync(content, dto.Filename);

        var metaData = new PhotoMetadata("N/A", "Unknown", dto.CameraModel, dto.UploadedAt);

        var photo = new Photo(dto.UserId, dto.LighthouseId, savedPath, metaData);

        await _photoRepository.AddAsync(photo);

        return Result<Guid>.Success(photo.Id);
    }
}
