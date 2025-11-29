using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TrustGateAPI.Contracts;
using TrustGateAPI.Controllers;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;

namespace TrustGateTests.TrustGateAPI.Controllers;

[TestFixture]
public class EndpointTokenControllerTests
{
    private Mock<IEndpointTokenService> _serviceMock = null!;
    private EndpointTokenController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<IEndpointTokenService>();
        _controller = new EndpointTokenController(_serviceMock.Object);
    }

    // -----------------------------------------------------------------
    //  Generate() – walidacja pustej listy
    // -----------------------------------------------------------------

    [Test]
    public async Task Generate_WhenEndpointIdsEmpty_ReturnsBadRequest()
    {
        // arrange
        var request = new GenerateEndpointTokenRequest
        {
            EndpointIds = []
        };

        // act
        var result = await _controller.Generate(request);

        // assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

        var badRequest = (BadRequestObjectResult)result;
        Assert.That(badRequest.Value, Is.EqualTo("EndpointIds is required."));

        _serviceMock.Verify(
            s => s.GenerateTokenForEndpointsAsync(It.IsAny<IReadOnlyList<int>>()),
            Times.Never);
    }

    // -----------------------------------------------------------------
    //  Generate() – poprawne wywołanie
    // -----------------------------------------------------------------

    [Test]
    public async Task Generate_WithValidRequest_ReturnsOkWithTokenAndEndpoints()
    {
        // arrange
        var endpointIds = new List<int> { 1, 2 };

        var request = new GenerateEndpointTokenRequest
        {
            EndpointIds = endpointIds
        };

        var token = new ApiToken
        {
            Id = 123,
            Value = "generated-token"
        };

        var endpoints = new List<ApiEndpoint>
        {
            new() { Id = 1, Name = "E1", Route = "/e1" },
            new() { Id = 2, Name = "E2", Route = "/e2" }
        };

        _serviceMock
            .Setup(s => s.GenerateTokenForEndpointsAsync(
                It.Is<IReadOnlyList<int>>(ids => ids.SequenceEqual(endpointIds))
            ))
            .Returns(Task.FromResult((token, (IReadOnlyList<ApiEndpoint>)endpoints)));

        // act
        var result = await _controller.Generate(request);

        // assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)result;

        Assert.That(ok.Value, Is.Not.Null);

        var value = ok.Value!;
        var type = value.GetType();

        // TokenId
        var tokenIdProp = type.GetProperty("TokenId");
        Assert.That(tokenIdProp, Is.Not.Null);
        Assert.That(tokenIdProp!.GetValue(value), Is.EqualTo(token.Id));

        // Token
        var tokenProp = type.GetProperty("Token");
        Assert.That(tokenProp, Is.Not.Null);
        Assert.That(tokenProp!.GetValue(value), Is.EqualTo(token.Value));

        // Endpoints
        var endpointsProp = type.GetProperty("Endpoints");
        Assert.That(endpointsProp, Is.Not.Null);

        var returnedEndpoints = ((IEnumerable<object>?)endpointsProp!.GetValue(value))!.ToList();
        Assert.That(returnedEndpoints.Count, Is.EqualTo(2));

        _serviceMock.Verify(
            s => s.GenerateTokenForEndpointsAsync(
                It.Is<IReadOnlyList<int>>(ids => ids.SequenceEqual(endpointIds))
            ),
            Times.Once);
    }
}