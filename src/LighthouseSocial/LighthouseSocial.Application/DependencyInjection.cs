using FluentValidation;
using LighthouseSocial.Application.Contracts;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Application.Features.Lighthouses.CreateLighthouse;
using LighthouseSocial.Application.Features.Lighthouses.DeleteLighthouse;
using LighthouseSocial.Application.Features.Lighthouses.GetAllLighthouses;
using LighthouseSocial.Application.Features.Lighthouses.GetLighthouseById;
using LighthouseSocial.Application.Services;
using LighthouseSocial.Application.Validators;
using LighthouseSocial.Domain.Countries;
using Microsoft.Extensions.DependencyInjection;

namespace LighthouseSocial.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ILighthouseService, LighthouseService>();
        //services.AddScoped<IPhotoService, PhotoService>();

        services.AddScoped<GetAllLighthousesHandler>();
        services.AddScoped<GetLighthouseByIdHandler>();
        services.AddScoped<CreateLighthouseHandler>();
        services.AddScoped<DeleteLighthouseHandler>();

        services.AddScoped<ICountryRegistry, CountryRegistry>();

        services.AddScoped<IValidator<LighthouseDto>, LighthouseDtoValidator>();

        return services;
    }
}
