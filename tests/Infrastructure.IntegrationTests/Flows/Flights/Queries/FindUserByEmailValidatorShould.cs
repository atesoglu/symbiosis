using System;
using Application.Flows.Users.Queries;
using Bogus;
using FluentAssertions;
using Xunit;

namespace Infrastructure.IntegrationTests.Flows.Flights.Queries
{
    public class FindUserByEmailValidatorShould
    {
        private readonly FindUserByEmailValidator _validator;

        public FindUserByEmailValidatorShould()
        {
            _validator = new FindUserByEmailValidator();
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