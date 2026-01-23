using NUnit.Framework;
using TrustGateAPI.Controllers;

namespace TrustGateTests.TrustGateAPI.Controllers;

[TestFixture]
public class BaseControllerTests
{
    private TestBaseController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _controller = new TestBaseController();
    }

    [Test]
    public void GetNotFoundMessage_ReturnsExpectedString()
    {
        // arrange
        var methodName = "GetToken";

        // act
        var result = _controller.PublicGetNotFoundMessage(methodName);

        // assert
        Assert.That(result, Is.EqualTo($"No data found while executing {methodName}"));
    }

    [Test]
    public void GetBadRequestMessage_ReturnsExpectedString()
    {
        // arrange
        var methodName = "RefresToken";

        // act
        var result = _controller.PublicGetBadRequestMessage(methodName);

        // assert
        Assert.That(result, Is.EqualTo($"Error during execution {methodName}"));
    }

    [Test]
    public void GetUnauthorizedMessage_ReturnsExpectedString()
    {
        // arrange
        var methodName = "GetToken";

        // act
        var result = _controller.PublicGetUnauthorizedMessage(methodName);

        // assert
        Assert.That(result, Is.EqualTo($"Unauthorized access attempt in {methodName}"));
    }


    private class TestBaseController : BaseController
    {
        public string PublicGetNotFoundMessage(string methodName)
            => GetNotFoundMessage(methodName);

        public string PublicGetBadRequestMessage(string methodName)
            => GetBadRequestMessage(methodName);

        public string PublicGetUnauthorizedMessage(string methodName)
            => GetUnauthorizedMessage(methodName);
    }
}