using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Flows.Users.Queries;
using Application.Models;
using Application.Persistence;
using Application.Request;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Infrastructure.IntegrationTests.Flows.Flights.Queries
{
    public class FindUserByEmailHandlerShould : IClassFixture<ServicesFixture>
    {
        private readonly IRequestHandler<FindUserByEmailCommand, UserObjectModel> _handler;

        public FindUserByEmailHandlerShould(ServicesFixture fixture)
        {
            _handler = fixture.ServiceProvider.GetRequiredService<IRequestHandler<FindUserByEmailCommand, UserObjectModel>>();
            fixture.ServiceProvider.GetRequiredService<IDataContext>().SeedData(fixture.ServiceProvider.GetRequiredService<ILogger<ServicesFixture>>());
        }

        [Fact]
        public async Task NotFoundExceptionShouldBeThrownIfResourceIsNotFound()
        {
            var command = new FindUserByEmailCommand
            {
                Email = Guid.NewGuid().ToString("N") + "@email.com"
            };

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command, CancellationToken.None));
        }
    }
}