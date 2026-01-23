using NUnit.Framework;
using TrustGateCore.Models;

namespace TrustGateTests.TrustGateCore.Models;

[TestFixture]
public class AdminTests
{
    [Test]
    public void Admin_InitializedWithDefaultValues_HasDefaultValues()
    {
        // arrange
        var admin = new Admin();

        // assert
        Assert.That(admin.Id, Is.EqualTo(0));
        Assert.That(admin.Login, Is.EqualTo(string.Empty));
        Assert.That(admin.Password, Is.EqualTo(string.Empty));
        Assert.That(admin.Email, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Admin_SetProperties_ValuesAssignedCorrectly()
    {
        // arrange
        var admin = new Admin
        {
            Id = 1,
            Login = "adminUser",
            Password = "securePassword",
            Email = "admin@example.com"
        };

        // assert
        Assert.That(admin.Id, Is.EqualTo(1));
        Assert.That(admin.Login, Is.EqualTo("adminUser"));
        Assert.That(admin.Password, Is.EqualTo("securePassword"));
        Assert.That(admin.Email, Is.EqualTo("admin@example.com"));
    }

    [Test]
    public void Admin_Equals_DifferentObjectsAreNotEqual()
    {
        // arrange
        var admin1 = new Admin
        {
            Id = 1,
            Login = "adminUser",
            Password = "securePassword",
            Email = "admin@example.com"
        };

        var admin2 = new Admin
        {
            Id = 2,
            Login = "userAdmin",
            Password = "otherPassword",
            Email = "user@example.com"
        };

        // assert
        Assert.That(admin1.Equals(admin2), Is.False);
    }
}