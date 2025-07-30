using LighthouseSocial.Application.Features.Lighthouses.GetAllLighthouses;
using LighthouseSocial.Domain.Countries;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;
using LighthouseSocial.Domain.ValueObjects;
using Moq;

namespace LighthouseSocial.UnitTests.Features.Lighthouses;

public class GetAllLighthousesHandlerTests
{
    private readonly Mock<ILighthouseRepository> _lighthouseRepositoryMock;
    private readonly GetAllLighthousesHandler _handler;

    public GetAllLighthousesHandlerTests()
    {
        _lighthouseRepositoryMock = new Mock<ILighthouseRepository>();
        _handler = new GetAllLighthousesHandler(_lighthouseRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenThereAreLighthouses()
    {
        // Arrange
        var lighthouses = new List<Lighthouse>
        {
            new("Roman Rock", new Country(27, "South Africa"), new Coordinates(34.10, 34.15)),
            new("Green Point", new Country(27, "South Africa"), new Coordinates(24.10, 22.05))
        };

        _lighthouseRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(Task.FromResult(lighthouses.AsEnumerable()));

        // Act
        var result = await _handler.HandleAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count());

        _lighthouseRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFail_WhenThereAreNoLighthouses()
    {
        // Arrange
        var lighthouses = new List<Lighthouse>();

        _lighthouseRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(Task.FromResult(lighthouses.AsEnumerable()));

        // Act
        var result = await _handler.HandleAsync();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("No lighthouses found.", result.ErrorMessage);

        _lighthouseRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}
