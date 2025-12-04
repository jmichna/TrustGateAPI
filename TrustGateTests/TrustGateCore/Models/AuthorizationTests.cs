using NUnit.Framework;
using TrustGateCore.Models;

namespace TrustGateTests.TrustGateCore.Models;
[TestFixture]
public class AuthorizationTests
{
    [Test]
    public void Authorization_InitializedWithDefaultValues_HasDefaultValues()
    {
        // arrange
        var authorization = new Authorization();

        // assert
        Assert.That(authorization.Id, Is.EqualTo(0));
        Assert.That(authorization.ControllerName, Is.Null);
        Assert.That(authorization.Generic, Is.EqualTo(false));
        Assert.That(authorization.UserId, Is.EqualTo(0));
        Assert.That(authorization.User, Is.EqualTo(default(User)));
        Assert.That(authorization.CompanyId, Is.EqualTo(0));
        Assert.That(authorization.Company, Is.EqualTo(default(Company)));
    }

    [Test]
    public void Authorization_SetProperties_ValuesAssignedCorrectly()
    {
        // arrange
        var authorization = new Authorization
        {
            Id = 1,
            ControllerName = "TestController",
            Generic = true,
            UserId = 101,
            User = new User { Id = 101, Name = "Test User" },
            CompanyId = 202,
            Company = new Company { Id = 202, CompanyName = "Test Company" }
        };

        // assert
        Assert.That(authorization.Id, Is.EqualTo(1));
        Assert.That(authorization.ControllerName, Is.EqualTo("TestController"));
        Assert.That(authorization.Generic, Is.EqualTo(true));
        Assert.That(authorization.UserId, Is.EqualTo(101));
        Assert.That(authorization.User.Id, Is.EqualTo(101));
        Assert.That(authorization.User.Name, Is.EqualTo("Test User"));
        Assert.That(authorization.CompanyId, Is.EqualTo(202));
        Assert.That(authorization.Company.Id, Is.EqualTo(202));
        Assert.That(authorization.Company.CompanyName, Is.EqualTo("Test Company"));
    }

    [Test]
    public void Authorization_NotEqualObjects_AreDifferent()
    {
        // arrange
        var authorization1 = new Authorization
        {
            Id = 1,
            ControllerName = "TestController",
            Generic = true,
            UserId = 101,
            User = new User { Id = 101, Name = "Test User" },
            CompanyId = 202,
            Company = new Company { Id = 202, CompanyName = "Test Company" }
        };

        var authorization2 = new Authorization
        {
            Id = 2,
            ControllerName = "AnotherController",
            Generic = false,
            UserId = 102,
            User = new User { Id = 102, Name = "Another User" },
            CompanyId = 203,
            Company = new Company { Id = 203, CompanyName = "Another Company" }
        };

        // assert
        Assert.That(authorization1, Is.Not.EqualTo(authorization2));
    }
}