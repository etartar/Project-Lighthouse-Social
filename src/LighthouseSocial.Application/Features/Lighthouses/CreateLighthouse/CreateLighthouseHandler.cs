using LighthouseSocial.Application.Common;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Countries;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;
using LighthouseSocial.Domain.ValueObjects;

namespace LighthouseSocial.Application.Features.Lighthouses.CreateLighthouse;

public class CreateLighthouseHandler
{
    private readonly ILighthouseRepository _lighthouseRepository;
    private readonly ICountryRegistry _countryRegistry;

    public CreateLighthouseHandler(ILighthouseRepository lighthouseRepository, ICountryRegistry countryRegistry)
    {
        _lighthouseRepository = lighthouseRepository;
        _countryRegistry = countryRegistry;
    }

    public async Task<Result<Guid>> HandleAsync(LighthouseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return Result<Guid>.Failure("Lighthouse name is required.");
        }

        Country? country;

        try
        {
            country = _countryRegistry.GetById(dto.CountryId);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Invalid country Id :  {dto.CountryId}, {ex.Message}");
        }

        var location = new Coordinates(dto.Latitude, dto.Longitude);

        var lighthouse = new Lighthouse(dto.Name, country, location);

        await _lighthouseRepository.AddAsync(lighthouse);

        return Result<Guid>.Success(lighthouse.Id);
    }
}
