using LighthouseSocial.Application.Contracts;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Application.Features.Lighthouses.CreateLighthouse;
using LighthouseSocial.Application.Features.Lighthouses.DeleteLighthouse;
using LighthouseSocial.Application.Features.Lighthouses.GetAllLighthouses;
using LighthouseSocial.Application.Features.Lighthouses.GetLighthouseById;
using LighthouseSocial.Domain.Entities;

namespace LighthouseSocial.Application.Services;

internal sealed class LighthouseService : ILighthouseService
{
    private readonly CreateLighthouseHandler _createLighthouseHandler;
    private readonly DeleteLighthouseHandler _deleteLighthouseHandler;
    private readonly GetAllLighthousesHandler _getAllLighthousesHandler;
    private readonly GetLighthouseByIdHandler _getLighthouseByIdHandler;

    public LighthouseService(
            CreateLighthouseHandler createLighthouseHandler,
            DeleteLighthouseHandler deleteLighthouseHandler,
            GetAllLighthousesHandler getAllLighthousesHandler,
            GetLighthouseByIdHandler getLighthouseByIdHandler)
    {
        _createLighthouseHandler = createLighthouseHandler;
        _deleteLighthouseHandler = deleteLighthouseHandler;
        _getAllLighthousesHandler = getAllLighthousesHandler;
        _getLighthouseByIdHandler = getLighthouseByIdHandler;
    }

    public async Task<IEnumerable<LighthouseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _getAllLighthousesHandler.HandleAsync();

        return result.IsSuccess ? result.Data : [];
    }

    public async Task<LighthouseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _getLighthouseByIdHandler.HandleAsync(id);
        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to get lighthouse by id: {result.ErrorMessage}");
        }
        return result.Data;
    }

    public async Task<IEnumerable<Photo>> GetPhotosByIdAsync(Guid photoId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<LighthouseDto>> GetTopAsync(TopDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateAsync(LighthouseDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _createLighthouseHandler.HandleAsync(dto);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Failed to create lighthouse: {result.ErrorMessage}");
        }

        return result.Data;
    }

    public async Task UpdateAsync(Guid id, LighthouseDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var _ = await _deleteLighthouseHandler.HandleAsync(id);
        //if (!result.Success)
        //{
        //    throw new InvalidOperationException($"Failed to create lighthouse: {result.ErrorMessage}");
        //}
    }
}
