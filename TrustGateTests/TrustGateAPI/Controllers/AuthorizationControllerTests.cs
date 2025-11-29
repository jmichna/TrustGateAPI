using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TrustGateAPI.Controllers;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateTests.TrustGateAPI.Controllers;

[TestFixture]
public class AuthorizationControllerTests
{
    private Mock<IAuthorizationService> _authServiceMock = null!;
    private AuthorizationController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _authServiceMock = new Mock<IAuthorizationService>();
        _controller = new AuthorizationController(_authServiceMock.Object);
    }

    [Test]
    public void GetToken_ValidCredentials_ReturnsOkWithToken()
    {
        // arrange
        var login = "admin";
        var password = "password";
        var expectedToken = "jwt-token";

        _authServiceMock
            .Setup(s => s.GenerateToken(login, password))
            .Returns(expectedToken);

        // act
        var result = _controller.GetToken(login, password);

        // assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;

        Assert.That(okResult.Value, Is.InstanceOf<BaseController.TokenResponse>());
        var response = (BaseController.TokenResponse)okResult.Value!;

        Assert.That(response.Token, Is.EqualTo(expectedToken));

        _authServiceMock.Verify(
            s => s.GenerateToken(login, password),
            Times.Once);
    }

    [Test]
    public void GetToken_InvalidCredentials_ReturnsUnauthorizedWithError()
    {
        // arrange
        var login = "user@test.com";
        var password = "badPass";

        _authServiceMock
            .Setup(s => s.GenerateToken(login, password))
            .Throws<UnauthorizedAccessException>();

        // act
        var result = _controller.GetToken(login, password);

        // assert
        Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
        var unauthorized = (UnauthorizedObjectResult)result;

        Assert.That(unauthorized.Value, Is.InstanceOf<BaseController.ErrorResponse>());
        var error = (BaseController.ErrorResponse)unauthorized.Value!;

        Assert.That(string.IsNullOrWhiteSpace(error.Error), Is.False);
    }

    // ---------- RefresToken ----------

    [Test]
    public void RefresToken_ValidToken_ReturnsOkWithNewToken()
    {
        // arrange
        var oldToken = "old-token";
        var newToken = "new-token";

        _authServiceMock
            .Setup(s => s.RefreshToken(oldToken))
            .Returns(newToken);

        // act
        var result = _controller.RefresToken(oldToken);

        // assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;

        Assert.That(okResult.Value, Is.InstanceOf<BaseController.TokenResponse>());
        var response = (BaseController.TokenResponse)okResult.Value!;

        Assert.That(response.Token, Is.EqualTo(newToken));

        _authServiceMock.Verify(
            s => s.RefreshToken(oldToken),
            Times.Once);
    }

    [Test]
    public void RefresToken_ServiceThrows_ReturnsBadRequestWithError()
    {
        // arrange
        var token = "invalid-token";

        _authServiceMock
            .Setup(s => s.RefreshToken(token))
            .Throws<Exception>();

        // act
        var result = _controller.RefresToken(token);

        // assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequest = (BadRequestObjectResult)result;

        Assert.That(badRequest.Value, Is.InstanceOf<BaseController.ErrorResponse>());
        var error = (BaseController.ErrorResponse)badRequest.Value!;

        Assert.That(string.IsNullOrWhiteSpace(error.Error), Is.False);
    }
}