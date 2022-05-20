using Application.Flows.Authentication.Commands;
using Application.Flows.Users.Queries;
using Application.Models;
using Application.Models.Authentication;
using Application.Request;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMemoryCache()
            .Configure<JwtOptions>(configuration.GetSection("Jwt"))
            //
            // .AddTransient<IRequestHandler<CreateTokenCommand, TokenObjectModel>, CreateTokenHandler>().AddTransient<IValidator<CreateTokenCommand>, CreateTokenValidator>()
            //
            .AddTransient<IRequestHandler<RegisterUserCommand, UserObjectModel>, RegisterUserHandler>().AddTransient<IValidator<RegisterUserCommand>, RegisterUserValidator>()
            .AddTransient<IRequestHandler<AuthenticateUserCommand, AuthenticationObjectModel>, AuthenticateUserHandler>().AddTransient<IValidator<AuthenticateUserCommand>, AuthenticateUserValidator>()
            .AddTransient<IRequestHandler<FindUserByEmailCommand, UserObjectModel>, FindUserByEmailHandler>().AddTransient<IValidator<FindUserByEmailCommand>, FindUserByEmailValidator>()
            //
            ;

        return services;
    }
}