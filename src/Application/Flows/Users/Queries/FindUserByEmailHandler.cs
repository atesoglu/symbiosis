using Application.Events.Users;
using Application.Exceptions;
using Application.Models;
using Application.Persistence;
using Application.Request;
using Application.Services;
using Application.Services.EventDispatcher;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Flows.Users.Queries;

public class FindUserByEmailHandler : IRequestHandler<FindUserByEmailCommand, UserObjectModel>
{
    private readonly IDataContext _repository;
    private readonly IValidator<FindUserByEmailCommand> _validator;
    private readonly IEventDispatcherService _eventDispatcherService;
    private readonly ILogger<FindUserByEmailHandler> _logger;

    public FindUserByEmailHandler(IDataContext repository, IValidator<FindUserByEmailCommand> validator, IEventDispatcherService eventDispatcherService, ILogger<FindUserByEmailHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _eventDispatcherService = eventDispatcherService;
        _logger = logger;
    }

    public async Task<UserObjectModel> HandleAsync(FindUserByEmailCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var entity = await _repository.Users.Where(w => w.Email == request.Email).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new NotFoundException($"User cannot be found with the email: {request.Email}");

        var dto = new UserObjectModel(entity);
        await _eventDispatcherService.Dispatch(new UserSignedInEvent(dto), cancellationToken);

        return dto;
    }
}