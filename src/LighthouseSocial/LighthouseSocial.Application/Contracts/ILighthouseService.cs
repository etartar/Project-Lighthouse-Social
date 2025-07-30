using LighthouseSocial.Application.Dtos;

namespace LighthouseSocial.Application.Contracts;

public interface ILighthouseService
{
    Task<IEnumerable<LighthouseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LighthouseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(LighthouseDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, LighthouseDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
