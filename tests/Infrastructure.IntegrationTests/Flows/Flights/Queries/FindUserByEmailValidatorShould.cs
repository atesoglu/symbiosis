using System;
using Application.Flows.Users.Queries;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Infrastructure.IntegrationTests.Flows.Flights.Queries
{
    public class FilterLocationsValidatorShould
    {
        private readonly FilterLocationsValidator _validator;

        public FilterLocationsValidatorShould()
        {
            _validator = new FilterLocationsValidator();
        }

        [Fact]
        public void NotAllowEmptyEmail()
        {
            var command = new Faker<FindUserByEmailCommand>()
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();

            command.Email = string.Empty;

            _validator.Validate(command).IsValid.Should().BeFalse();
        }

        [Fact]
        public void NotAllowInvalidEmail()
        {
            var command = new Faker<FindUserByEmailCommand>()
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .Generate();

            command.Email = "email.com";

            _validator.Validate(command).IsValid.Should().BeFalse();
        }
    }
}