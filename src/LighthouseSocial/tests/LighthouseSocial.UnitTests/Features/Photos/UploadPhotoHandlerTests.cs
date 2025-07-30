using FluentValidation;
using FluentValidation.Results;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Application.Features.Photos.UploadPhoto;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;
using Moq;

namespace LighthouseSocial.UnitTests.Features.Photos;

public class UploadPhotoHandlerTests
{
    private readonly Mock<IPhotoStorageService> _storageServiceMock;
    private readonly Mock<IPhotoRepository> _photoRepositoryMock;
    private readonly Mock<IValidator<PhotoDto>> _validatorMock;
    private readonly UploadPhotoHandler _handler;

    public UploadPhotoHandlerTests()
    {
        _storageServiceMock = new Mock<IPhotoStorageService>();
        _photoRepositoryMock = new Mock<IPhotoRepository>();
        _validatorMock = new Mock<IValidator<PhotoDto>>();
        _handler = new UploadPhotoHandler(_storageServiceMock.Object, _photoRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenValidInput()
    {
        // Arrange
        var dto = new PhotoDto(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), "SunDownOfCapeTown.jpg", DateTime.UtcNow, "DSLR");

        var stream = new MemoryStream([24, 42, 32]);

        _storageServiceMock.Setup(s => s.SaveAsync(It.IsAny<Stream>(), dto.FileName)).ReturnsAsync("uploads/SunDownOfCapeTown.jpg");

        _validatorMock.Setup(v => v.Validate(It.IsAny<PhotoDto>())).Returns(new ValidationResult());

        // Act
        var result = await _handler.HandleAsync(dto, stream);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Data);

        _photoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Photo>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenValidationFails()
    {
        // Arrange
        var dto = new PhotoDto(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), "Noname", DateTime.Now.AddDays(2), string.Empty);

        var validationFailures = new List<ValidationFailure>
        {
            new("FileName","Filename can not be empty."),
            new("UploadedAt","Upload date must be in the past or now."),
            new("CameraType","Camera type is not recognized"),
            new("UserId","UserId is required."),
            new("UserId","LighthouseId is required.")
        };

        _validatorMock.Setup(v => v.Validate(It.IsAny<PhotoDto>())).Returns(new ValidationResult(validationFailures));

        // Act
        var stream = new MemoryStream([24, 42, 32]);

        var result = await _handler.HandleAsync(dto, stream);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Filename can not be empty.", result.ErrorMessage);
        Assert.Contains("Upload date must be in the past or now.", result.ErrorMessage);
        Assert.Contains("Camera type is not recognized", result.ErrorMessage);
        Assert.Contains("UserId is required.", result.ErrorMessage);
        Assert.Contains("LighthouseId is required.", result.ErrorMessage);
    }
}
