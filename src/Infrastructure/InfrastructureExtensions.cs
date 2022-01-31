using Application;
using Application.Persistence;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddApplication(configuration)
                //
                .AddDbContext<InMemoryDataContext>(options => options.UseInMemoryDatabase(databaseName: "InMemoryDatabase"))
                .AddScoped<IDataContext>(provider => provider.GetService<InMemoryDataContext>())
                //
                .AddScoped<IEventDispatcherService, EventDispatcherService>()
                ;

            switch (configuration["Cache:Type"])
            {
                case "Redis":
                    services
                        .AddStackExchangeRedisCache(options => options.ConfigurationOptions = new ConfigurationOptions
                        {
                            EndPoints = {{configuration["Cache:Redis:Host"], int.Parse(configuration["Cache:Redis:Port"])}},
                            Password = configuration["Cache:Redis:Password"],
                            DefaultDatabase = string.IsNullOrEmpty(configuration["Cache:Redis:DatabaseId"]) ? null : int.Parse(configuration["Cache:Redis:DatabaseId"])
                        })
                        .AddScoped<ICacheService, CacheServiceRedis>();
                    break;
                default: //or InMemory
                {
                    services.AddScoped<ICacheService, CacheServiceInMemory>();
                    break;
                }
            }

            return services;
        }
    }
}