using Application.Events.User;
using Application.Exceptions;
using Application.Flows.Users.Queries;
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

namespace Application.Flows.Authentication.Commands
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, UserObjectModel>
    {
        private readonly IDataContext _repository;
        private readonly IValidator<AuthenticateUserCommand> _validator;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly ILogger<AuthenticateUserHandler> _logger;

        public AuthenticateUserHandler(IDataContext repository, IValidator<AuthenticateUserCommand> validator, ITokenService tokenService, IOptionsMonitor<JwtSettings> jwtSettings, IEventDispatcherService eventDispatcherService, ILogger<AuthenticateUserHandler> logger)
        {
            _repository = repository;
            _validator = validator;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.CurrentValue;
            _eventDispatcherService = eventDispatcherService;
            _logger = logger;
        }

        public async Task<UserObjectModel> HandleAsync(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var entity = await _repository.Users.Where(w => w.Email == request.Email).FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                throw new NotFoundException($"User can not be found with the email: {request.Email}");

            var dto = new UserObjectModel(entity);

            var token = _tokenService.BuildAsync(dto, cancellationToken);

            await _eventDispatcherService.Dispatch(new UserSignedInEvent(dto), cancellationToken);

            return dto;
        }
    }
}