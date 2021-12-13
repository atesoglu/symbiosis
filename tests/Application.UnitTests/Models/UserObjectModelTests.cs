using System.Text.Json;
using Application.Models;
using Application.Models.Base;
using Bogus;
using Domain.Models;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Models
{
    public class UserObjectModelTests
    {
        [Fact]
        public void ExtendsFromObjectModelBase()
        {
            var dto = new UserObjectModel();
            dto.Should().BeAssignableTo<ObjectModelBase>();
        }

        [Fact]
        public void ExtendsFromObjectModelBaseOfT()
        {
            var dto = new UserObjectModel();
            dto.Should().BeAssignableTo<ObjectModelBase<UserModel>>();
        }

        [Fact]
        public void AssignableFromDomainModel()
        {
            var faker = new Faker<UserModel>()
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();

            var dto = new UserObjectModel(faker);
            var assigned = new UserObjectModel(faker);

            assigned.Should().BeEquivalentTo(dto, options => options.Excluding(e => e.UserId));
        }

        [Fact]
        public void NullDomainModelAssignmentReturnsDefault()
        {
            var dto = new UserObjectModel();
            var assigned = new UserObjectModel(null);

            assigned.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public void ToStringSerializedAsJson()
        {
            var dto = new Faker<UserObjectModel>()
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();

            dto.ToString().Should().BeEquivalentTo(JsonSerializer.Serialize(dto));
        }
    }
}