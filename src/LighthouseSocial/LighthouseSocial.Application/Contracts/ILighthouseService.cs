using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Entities;

namespace LighthouseSocial.Application.Contracts;

public interface ILighthouseService
{
    Task<IEnumerable<LighthouseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<LighthouseDto>> GetTopAsync(TopDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<Photo>> GetPhotosByIdAsync(Guid photoId, CancellationToken cancellationToken = default);
    Task<LighthouseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(LighthouseDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, LighthouseDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
