using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Infrastructure.IntegrationTests.Services;

public class CacheServiceRedisShould : IClassFixture<ServicesFixture>
{
    private readonly ICacheService _service;

    public CacheServiceRedisShould(ServicesFixture fixture)
    {
        _service = fixture.ServiceProvider.GetRequiredService<ICacheService>();
    }

    [Fact]
    public async Task CanGetPrimitiveValue()
    {
        await CanSavePrimitiveValue();

        var value = await _service.GetAsync("key", CancellationToken.None);

        Assert.Equal("value", value);
    }

    [Fact]
    public async Task CanGetComplexValue()
    {
        await CanSaveComplexValue();

        var value = await _service.GetAsync("key", CancellationToken.None);

        Assert.Equal(new UserObjectModel().ToString(), value);
    }

    [Fact]
    public async Task CanSavePrimitiveValue()
    {
        await _service.SaveAsync("key", "value", TimeSpan.FromSeconds(50), CancellationToken.None);
    }

    [Fact]
    public async Task CanSaveComplexValue()
    {
        await _service.SaveAsync("key", new UserObjectModel().ToString(), TimeSpan.FromSeconds(50), CancellationToken.None);
    }

    [Fact]
    public async Task CanRemove()
    {
        await CanSavePrimitiveValue();

        await _service.RemoveAsync("key", CancellationToken.None);
    }
}