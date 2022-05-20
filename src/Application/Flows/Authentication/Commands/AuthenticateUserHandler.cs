using Application.Events.Users;
using Application.Exceptions;
using Application.Models;
using Application.Models.Authentication;
using Application.Persistence;
using Application.Request;
using Application.Services.Authentication;
using Application.Services.EventDispatcher;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Flows.Authentication.Commands;

public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticationObjectModel>
{
    private readonly IDataContext _repository;
    private readonly IValidator<AuthenticateUserCommand> _validator;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenService _tokenService;
    private readonly JwtOptions _jwtOptions;
    private readonly IEventDispatcherService _eventDispatcherService;
    private readonly ILogger<AuthenticateUserHandler> _logger;

    public AuthenticateUserHandler(IDataContext repository, IValidator<AuthenticateUserCommand> validator, IAuthenticator authenticator, ITokenService tokenService, IOptionsMonitor<JwtOptions> jwtSettings, IEventDispatcherService eventDispatcherService, ILogger<AuthenticateUserHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _authenticator = authenticator;
        _tokenService = tokenService;
        _jwtOptions = jwtSettings.CurrentValue;
        _eventDispatcherService = eventDispatcherService;
        _logger = logger;
    }

    public async Task<AuthenticationObjectModel> HandleAsync(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var entity = await _repository.Users.Where(w => w.Email == request.Email).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new NotFoundException($"User cannot be found with the email: {request.Email}");

        if (!await _authenticator.ValidatePasswordAsync(entity, request.Password, cancellationToken))
            throw new BadRequestException("Incorrect password.");

        var dto = new UserObjectModel(entity);

        var (token, expiresAtUtc) = await _tokenService.BuildAsync(dto, cancellationToken);

        var model = new AuthenticationObjectModel
        {
            Email = entity.Email,
            AccessToken = token,
            RefreshToken = Guid.NewGuid().ToString("N"),
            ExpiresAt = expiresAtUtc
        };

        await _eventDispatcherService.Dispatch(new UserSignedInEvent(dto), cancellationToken);

        return model;
    }
}