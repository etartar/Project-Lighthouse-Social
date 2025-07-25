﻿using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Application.Features.Lighthouses.CreateLighthouse;
using LighthouseSocial.Domain.Countries;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;
using Moq;
using System.Diagnostics.Metrics;

namespace LighthouseSocial.UnitTests.Features.Lighthouses;

public class CreateLighthouseHandlerTests
{
    private readonly Mock<ILighthouseRepository> _repositoryMock;
    private readonly Mock<ICountryRegistry> _registryMock;
    private readonly CreateLighthouseHandler _handler;

    public CreateLighthouseHandlerTests()
    {
        _repositoryMock = new Mock<ILighthouseRepository>();
        _registryMock = new Mock<ICountryRegistry>();
        _handler = new CreateLighthouseHandler(_repositoryMock.Object, _registryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenInputIsValid()
    {
        // Arrange
        var dto = new LighthouseDto(Guid.Empty, "Roman Rock", 27, 34.10, 34.13);
        var country = new Country(27, "South Africa");

        _registryMock.Setup(r => r.GetById(dto.CountryId)).Returns(country);

        // Act
        var result = await _handler.HandleAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Data);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Lighthouse>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenCountryNotFound()
    {
        // Arrange
        var dto = new LighthouseDto(Guid.Empty, "Roman Rock", 27, 34.10, 34.13);

        _registryMock.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception("Invalid country"));

        // Act
        var result = await _handler.HandleAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);

        Assert.Contains("Invalid country", result.ErrorMessage);
    }
}
