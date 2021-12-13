using Domain.Models;
using Domain.Models.Base;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Models
{
    public class UserModelTests
    {
        [Fact]
        public void ExtendsFromModelBase()
        {
            var actual = new UserModel();
            actual.Should().BeAssignableTo<ModelBase>();
        }

        [Fact]
        public void EmailPropertyShouldDefaultToNull()
        {
            var actual = new UserModel();
            actual.Email.Should().BeNull();
        }
    }
}