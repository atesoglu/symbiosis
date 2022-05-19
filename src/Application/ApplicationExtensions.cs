using Application.Flows.Users.Queries;
using Application.Models;
using Application.Models.Authentication;
using Application.Request;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMemoryCache()
                .AddSingleton<JwtSettings>(_ => new JwtSettings {Key = configuration["Jwt:Key"], Issuer = configuration["Jwt:Issuer"], Audience = configuration["Jwt:Audience"]})
                //
                // .AddTransient<IRequestHandler<CreateTokenCommand, TokenObjectModel>, CreateTokenHandler>().AddTransient<IValidator<CreateTokenCommand>, CreateTokenValidator>()
                //
                .AddTransient<IRequestHandler<FindUserByEmailCommand, UserObjectModel>, FindUserByEmailHandler>().AddTransient<IValidator<FindUserByEmailCommand>, FindUserByEmailValidator>()
                //
                ;

            return services;
        }
    }
}