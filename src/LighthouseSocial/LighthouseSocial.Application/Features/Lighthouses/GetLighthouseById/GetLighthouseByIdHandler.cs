using LighthouseSocial.Application.Common;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Interfaces;

namespace LighthouseSocial.Application.Features.Lighthouses.GetLighthouseById;

public class GetLighthouseByIdHandler(ILighthouseRepository repository)
{
    private readonly ILighthouseRepository _repository = repository;

    public async Task<Result<LighthouseDto>> HandleAsync(Guid id)
    {
        var lighthouse = await _repository.GetByIdAsync(id);

        if (lighthouse == null)
        {
            return Result<LighthouseDto>.Failure("No lighthouse found.");
        }

        var dto = new LighthouseDto(
            lighthouse.Id,
            lighthouse.Name,
            lighthouse.CountryId,
            lighthouse.Location.Latitude,
            lighthouse.Location.Longitude
        );

        return Result<LighthouseDto>.Success(dto);
    }
}
