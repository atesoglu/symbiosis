using System.Text.Json;
using Application.Flows.Users.Queries;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Infrastructure.IntegrationTests.Flows.Flights.Queries
{
    public class FindUserByEmailCommandShould
    {
        [Fact]
        public void ToStringSerializedAsJson()
        {
            var command = new Faker<FindUserByEmailCommand>()
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();

            command.ToString().Should().BeEquivalentTo(JsonSerializer.Serialize(command));
        }
    }
}