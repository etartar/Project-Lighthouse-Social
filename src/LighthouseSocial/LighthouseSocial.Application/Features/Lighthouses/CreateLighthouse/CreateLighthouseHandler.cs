using FluentValidation;
using LighthouseSocial.Application.Common;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Countries;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;
using LighthouseSocial.Domain.ValueObjects;

namespace LighthouseSocial.Application.Features.Lighthouses.CreateLighthouse;

public class CreateLighthouseHandler(ILighthouseRepository lighthouseRepository, ICountryRegistry countryRegistry, IValidator<LighthouseDto> validator)
{
    public async Task<Result<Guid>> HandleAsync(LighthouseDto dto)
    {
        var validation = validator.Validate(dto);

        if (!validation.IsValid)
        {
            var errors = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));

            return Result<Guid>.Failure(errors);
        }

        Country? country;

        try
        {
            country = await countryRegistry.GetByIdAsync(dto.CountryId);

            var location = new Coordinates(dto.Latitude, dto.Longitude);

            var lighthouse = new Lighthouse(dto.Id, dto.Name, country, location);

            await lighthouseRepository.AddAsync(lighthouse);

            return Result<Guid>.Success(lighthouse.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Invalid country Id :  {dto.CountryId}, {ex.Message}");
        }
    }
}
