using NUnit.Framework;
using TrustGateCore.Models;

namespace TrustGateTests.TrustGateCore.Models;

[TestFixture]
public class ApiEndpointTests
{
    [Test]
    public void ApiEndpoint_InitializedWithDefaultValues_HasDefaultValues()
    {
        // arrange
        var apiEndpoint = new ApiEndpoint();

        // assert
        Assert.That(apiEndpoint.Id, Is.EqualTo(0));
        Assert.That(apiEndpoint.Name, Is.EqualTo(string.Empty));
        Assert.That(apiEndpoint.HttpMethod, Is.EqualTo(string.Empty));
        Assert.That(apiEndpoint.Route, Is.EqualTo(string.Empty));
        Assert.That(apiEndpoint.CompanyId, Is.EqualTo(0));
    }

    [Test]
    public void ApiEndpoint_SetProperties_ValuesAssignedCorrectly()
    {
        // arrange
        var apiEndpoint = new ApiEndpoint
        {
            Id = 1,
            Name = "GetUser",
            HttpMethod = "GET",
            Route = "/users",
            CompanyId = 100,
            Company = new Company { Id = 100, CompanyName = "Company X" }
        };

        // assert
        Assert.That(apiEndpoint.Id, Is.EqualTo(1));
        Assert.That(apiEndpoint.Name, Is.EqualTo("GetUser"));
        Assert.That(apiEndpoint.HttpMethod, Is.EqualTo("GET"));
        Assert.That(apiEndpoint.Route, Is.EqualTo("/users"));
        Assert.That(apiEndpoint.CompanyId, Is.EqualTo(100));
        Assert.That(apiEndpoint.Company.Id, Is.EqualTo(100));
    }

    [Test]
    public void ApiEndpoint_Equals_DifferentObjectsAreNotEqual()
    {
        // arrange
        var apiEndpoint1 = new ApiEndpoint
        {
            Id = 1,
            Name = "GetUser",
            HttpMethod = "GET",
            Route = "/users",
            CompanyId = 100,
            Company = new Company { Id = 100, CompanyName = "Company X" }
        };

        var apiEndpoint2 = new ApiEndpoint
        {
            Id = 2,
            Name = "CreateUser",
            HttpMethod = "POST",
            Route = "/users/create",
            CompanyId = 101,
            Company = new Company { Id = 101, CompanyName = "Company Y" }
        };

        // assert
        Assert.That(apiEndpoint1.Equals(apiEndpoint2), Is.False);
    }
}