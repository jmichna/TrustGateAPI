using NUnit.Framework;
using TrustGateCore.Models;
using System.Collections.Generic;

namespace TrustGateTests.TrustGateCore.Models;
[TestFixture]
public class UserTests
{
    [Test]
    public void User_InitializedWithDefaultValues_HasDefaultValues()
    {
        // arrange
        var user = new User();

        // assert
        Assert.That(user.Id, Is.EqualTo(0));
        Assert.That(user.Name, Is.EqualTo(string.Empty));
        Assert.That(user.Initials, Is.EqualTo(string.Empty));
        Assert.That(user.Password, Is.EqualTo(string.Empty));
        Assert.That(user.Authorizations, Is.Empty);
    }

    [Test]
    public void User_SetProperties_ValuesAssignedCorrectly()
    {
        // arrange
        var user = new User
        {
            Id = 1,
            Name = "John Doe",
            Initials = "JD",
            Password = "SecurePassword123",
            Authorizations = new List<Authorization>
            {
                new Authorization { Id = 1, ControllerName = "TestController", Generic = true }
            }
        };

        // assert
        Assert.That(user.Id, Is.EqualTo(1));
        Assert.That(user.Name, Is.EqualTo("John Doe"));
        Assert.That(user.Initials, Is.EqualTo("JD"));
        Assert.That(user.Password, Is.EqualTo("SecurePassword123"));
        Assert.That(user.Authorizations.Count, Is.EqualTo(1));
    }

    [Test]
    public void User_Initialization_CollectionsInitializedCorrectly()
    {
        // arrange
        var user = new User();

        // assert
        Assert.That(user.Authorizations, Is.Not.Null);
        Assert.That(user.Authorizations.Count, Is.EqualTo(0));
    }

    [Test]
    public void User_NotEqualObjects_AreDifferent()
    {
        // arrange
        var user1 = new User
        {
            Id = 1,
            Name = "John Doe",
            Initials = "JD",
            Password = "SecurePassword123"
        };

        var user2 = new User
        {
            Id = 2,
            Name = "Jane Doe",
            Initials = "JD",
            Password = "AnotherPassword123"
        };

        // assert
        Assert.That(user1, Is.Not.EqualTo(user2));
    }
}