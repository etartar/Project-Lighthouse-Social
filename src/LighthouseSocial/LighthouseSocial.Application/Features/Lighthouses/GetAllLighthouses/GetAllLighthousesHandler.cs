using LighthouseSocial.Application.Common;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Interfaces;

namespace LighthouseSocial.Application.Features.Lighthouses.GetAllLighthouses;

public class GetAllLighthousesHandler
{
    private readonly ILighthouseRepository _lighthouseRepository;

    public GetAllLighthousesHandler(ILighthouseRepository lighthouseRepository)
    {
        _lighthouseRepository = lighthouseRepository;
    }

    public async Task<Result<IEnumerable<LighthouseDto>>> HandleAsync()
    {
        var lighthouses = await _lighthouseRepository.GetAllAsync();

        if (lighthouses == null || !lighthouses.Any())
        {
            return Result<IEnumerable<LighthouseDto>>.Failure("No lighthouses found.");
        }

        var lighthouseDtos = lighthouses.Select(l => new LighthouseDto(
            l.Id,
            l.Name,
            l.CountryId,
            l.Location.Latitude,
            l.Location.Longitude));

        return Result<IEnumerable<LighthouseDto>>.Success(lighthouseDtos);
    }
}
