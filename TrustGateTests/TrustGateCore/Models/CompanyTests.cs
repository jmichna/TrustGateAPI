using NUnit.Framework;
using TrustGateCore.Models;
using System.Collections.Generic;

namespace TrustGateTests.TrustGateCore.Models;
[TestFixture]
public class CompanyTests
{
    [Test]
    public void Company_InitializedWithDefaultValues_HasDefaultValues()
    {
        // arrange
        var company = new Company();

        // assert
        Assert.That(company.Id, Is.EqualTo(0));
        Assert.That(company.CompanyName, Is.EqualTo(string.Empty));
        Assert.That(company.CompanyInitials, Is.EqualTo(string.Empty));
        Assert.That(company.ProjectName, Is.EqualTo(string.Empty));
        Assert.That(company.ProjectId, Is.EqualTo(0));
        Assert.That(company.Authorizations, Is.Empty);
        Assert.That(company.ApiEndpoints, Is.Empty);
    }

    // Test 2: Przypisanie wartości do właściwości
    [Test]
    public void Company_SetProperties_ValuesAssignedCorrectly()
    {
        // arrange
        var company = new Company
        {
            Id = 1,
            CompanyName = "Test Company",
            CompanyInitials = "TC",
            ProjectName = "Test Project",
            ProjectId = 123,
            Authorizations = new List<Authorization>
            {
                new Authorization { Id = 1, ControllerName = "Controller1", Generic = true }
            },
            ApiEndpoints = new List<ApiEndpoint>
            {
                new ApiEndpoint { Id = 1, Name = "GetUser", HttpMethod = "GET", Route = "/users" }
            }
        };

        // assert
        Assert.That(company.Id, Is.EqualTo(1));
        Assert.That(company.CompanyName, Is.EqualTo("Test Company"));
        Assert.That(company.CompanyInitials, Is.EqualTo("TC"));
        Assert.That(company.ProjectName, Is.EqualTo("Test Project"));
        Assert.That(company.ProjectId, Is.EqualTo(123));
        Assert.That(company.Authorizations.Count, Is.EqualTo(1));
        Assert.That(company.ApiEndpoints.Count, Is.EqualTo(1));
    }

    [Test]
    public void Company_Initialization_CollectionsInitializedCorrectly()
    {
        // arrange
        var company = new Company();

        // assert
        Assert.That(company.Authorizations, Is.Not.Null);
        Assert.That(company.ApiEndpoints, Is.Not.Null);
        Assert.That(company.Authorizations.Count, Is.EqualTo(0));
        Assert.That(company.ApiEndpoints.Count, Is.EqualTo(0));
    }


    [Test]
    public void Company_NotEqualObjects_AreDifferent()
    {
        // arrange
        var company1 = new Company
        {
            Id = 1,
            CompanyName = "Test Company",
            CompanyInitials = "TC",
            ProjectName = "Test Project",
            ProjectId = 123
        };

        var company2 = new Company
        {
            Id = 2,
            CompanyName = "Another Company",
            CompanyInitials = "AC",
            ProjectName = "Another Project",
            ProjectId = 456
        };

        // assert
        Assert.That(company1, Is.Not.EqualTo(company2));
    }
}