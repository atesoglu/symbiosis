using API.Controllers.Base;
using Application.Flows.Authentication.Commands;
using Application.Models.Authentication;
using Application.Request;
using Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

public class AuthenticateUserController : ApiControllerBase
{
    private readonly IRequestHandler<AuthenticateUserCommand, AuthenticationObjectModel> _handler;

    public AuthenticateUserController(IRequestHandler<AuthenticateUserCommand, AuthenticationObjectModel> handler, ILogger<ApiControllerBase> logger) : base(logger)
    {
        _handler = handler;
    }

    [AllowAnonymous, HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponseModel>> AuthenticateUser(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        Logger.LogDebug("AuthenticateUser is requested.");

        var result = await _handler.HandleAsync(request, cancellationToken);

        return new ActionResult<AuthenticationResponseModel>(new AuthenticationResponseModel(result));
    }
}