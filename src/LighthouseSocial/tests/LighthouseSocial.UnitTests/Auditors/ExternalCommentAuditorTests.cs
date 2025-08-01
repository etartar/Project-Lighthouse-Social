﻿using LighthouseSocial.Infrastructure.Auditors;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace LighthouseSocial.UnitTests.Auditors;

public class ExternalCommentAuditorTests
{
    [Fact]
    public async Task IsTextCleanAsync_ShouldReturnTrue_WhenTextValid()
    {
        // Arrange
        var responseBody = JsonSerializer.Serialize(new AuditResult { IsClean = true });

        var httpHandlerMock = new Mock<HttpMessageHandler>();
        httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseBody)
            });

        var httpClient = new HttpClient(httpHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.audit")
        };

        var auditor = new ExternalCommentAuditor(httpClient);

        // Act
        var result = await auditor.IsTextCleanAsync("It's a lovely lighthouse photo. I love it!");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsTextCleanAsync_ShouldReturnFalse_WhenTextContainsBadWords()
    {
        // Arrange
        var responseBody = JsonSerializer.Serialize(new AuditResult { IsClean = false });

        var httpHandlerMock = new Mock<HttpMessageHandler>();
        httpHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseBody)
            });

        var httpClient = new HttpClient(httpHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.audit")
        };

        var auditor = new ExternalCommentAuditor(httpClient);

        // Act
        var result = await auditor.IsTextCleanAsync("This comment contains a badword.");

        // Assert
        Assert.False(result);
    }
}