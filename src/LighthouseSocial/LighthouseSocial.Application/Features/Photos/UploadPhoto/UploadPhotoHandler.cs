using FluentValidation;
using LighthouseSocial.Application.Common;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;
using LighthouseSocial.Domain.ValueObjects;

namespace LighthouseSocial.Application.Features.Photos.UploadPhoto;

public class UploadPhotoHandler(IPhotoStorageService storageService, IPhotoRepository photoRepository, IValidator<PhotoDto> validator)
{
    public async Task<Result<Guid>> HandleAsync(PhotoDto dto, Stream content)
    {
        var validation = validator.Validate(dto);

        if (!validation.IsValid)
        {
            var errors = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));

            return Result<Guid>.Failure(errors);
        }

        var savedPath = await storageService.SaveAsync(content, dto.FileName);

        var metaData = new PhotoMetadata("N/A", "Unknown", dto.CameraType, dto.UploadedAt);

        var photo = new Photo(dto.UserId, dto.LighthouseId, savedPath, metaData);

        await photoRepository.AddAsync(photo);

        return Result<Guid>.Success(photo.Id);
    }
}
