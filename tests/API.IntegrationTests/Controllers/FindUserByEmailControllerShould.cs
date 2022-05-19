using System;
using System.Threading;
using System.Threading.Tasks;
using API.Controllers;
using API.Controllers.Base;
using API.Controllers.Users;
using Application.Exceptions;
using Application.Flows.Users.Queries;
using Application.Models;
using Application.Persistence;
using Application.Request;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace API.IntegrationTests.Controllers
{
    public class FindUserByEmailControllerShould : IClassFixture<ServicesFixture>
    {
        private readonly IRequestHandler<FindUserByEmailCommand, UserObjectModel> _handler;

        public FindUserByEmailControllerShould(ServicesFixture fixture)
        {
            _handler = fixture.ServiceProvider.GetRequiredService<IRequestHandler<FindUserByEmailCommand, UserObjectModel>>();
            fixture.ServiceProvider.GetRequiredService<IDataContext>().SeedData(fixture.ServiceProvider.GetRequiredService<ILogger<ServicesFixture>>());
        }

        [Fact]
        public async Task ResultShouldHavePropertyValueMatch()
        {
            var controller = new FindUserByEmailController(_handler, new NullLogger<ApiControllerBase>());

            var command = new FindUserByEmailCommand
            {
                Email = Guid.NewGuid().ToString("N") + "@email.com"
            };

            await Assert.ThrowsAsync<NotFoundException>(() => controller.FindUserByEmail(command, CancellationToken.None));
        }
    }
}