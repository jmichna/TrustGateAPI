using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using TrustGateAPI.Models.Settings;
using TrustGateAPI.Services;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateTests.TrustGateAPI.Services;

[TestFixture]
public class AuthorizationServiceTests
{
    private Mock<IOptions<JsonSetting>> _jsonSettingsMock;
    private AuthorizationService _authorizationService;
    private JsonSetting _jsonSettings;

    [SetUp]
    public void SetUp()
    {
        _jsonSettingsMock = new Mock<IOptions<JsonSetting>>();
        _jsonSettings = new JsonSetting
        {
            JwtKey = "supersecretkeythatneedsToBeLongEnoughForSHA256Signature",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            TokenExpirationHours = 1
        };

        _jsonSettingsMock.Setup(x => x.Value).Returns(_jsonSettings);
        _authorizationService = new AuthorizationService(_jsonSettingsMock.Object);
    }

    [Test]
    public void GenerateToken_ValidLoginAndPassword_ReturnsToken()
    {
        // act
        var token = _authorizationService.GenerateToken("admin", "password");

        // assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.Empty);
    }

    [Test]
    public void GenerateToken_InvalidLoginOrPassword_ThrowsUnauthorizedAccessException()
    {
        // act & assert
        var ex = Assert.Throws<UnauthorizedAccessException>(() => _authorizationService.GenerateToken("invalid", "password"));
        Assert.That(ex.Message, Is.EqualTo("Invalid login or password"));
    }

    [Test]
    public void RefreshToken_ValidToken_ReturnsNewToken()
    {
        // arrange
        var validToken = _authorizationService.GenerateToken("admin", "password");

        // act
        var newToken = _authorizationService.RefreshToken(validToken);

        // assert
        Assert.That(newToken, Is.Not.Null);
        Assert.That(newToken, Is.Not.Empty);
    }

    [Test]
    public void RefreshToken_InvalidToken_ThrowsSecurityTokenException()
    {
        // act & assert
        var ex = Assert.Throws<SecurityTokenException>(() => _authorizationService.RefreshToken("invalid_token"));
        Assert.That(ex.Message, Is.EqualTo("Invalid or malformed token."));
    }
}