using Application.Events.Users;
using Application.Exceptions;
using Application.Models;
using Application.Models.Authentication;
using Application.Persistence;
using Application.Request;
using Application.Services.Authentication;
using Application.Services.EventDispatcher;
using Domain.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Flows.Authentication.Commands;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserObjectModel>
{
    private readonly IDataContext _repository;
    private readonly IValidator<RegisterUserCommand> _validator;
    private readonly IAuthenticator _authenticator;
    private readonly IEventDispatcherService _eventDispatcherService;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(IDataContext repository, IValidator<RegisterUserCommand> validator, IAuthenticator authenticator, IEventDispatcherService eventDispatcherService, ILogger<RegisterUserHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _authenticator = authenticator;
        _eventDispatcherService = eventDispatcherService;
        _logger = logger;
    }

    public async Task<UserObjectModel> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var entity = await _repository.Users.Where(w => w.Email == request.Email).FirstOrDefaultAsync(cancellationToken);

        if (entity != null)
            throw new ConflictException($"Email has already been registered: {request.Email}");

        var salt = await _authenticator.CreateSaltAsync(cancellationToken);
        entity = new UserModel
        {
            Email = request.Email,
            PasswordSalt = salt,
            PasswordHash = await _authenticator.HashPasswordAsync(request.Password, salt, cancellationToken)
        };

        await _repository.Users.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var dto = new UserObjectModel(entity);

        await _eventDispatcherService.Dispatch(new UserRegisteredEvent(dto), cancellationToken);

        return dto;
    }
}