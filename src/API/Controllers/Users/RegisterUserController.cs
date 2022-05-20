using API.Controllers.Base;
using Application.Flows.Authentication.Commands;
using Application.Models;
using Application.Models.Authentication;
using Application.Request;
using Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

public class RegisterUserController : ApiControllerBase
{
    private readonly IRequestHandler<RegisterUserCommand, UserObjectModel> _handler;

    public RegisterUserController(IRequestHandler<RegisterUserCommand, UserObjectModel> handler, ILogger<ApiControllerBase> logger) : base(logger)
    {
        _handler = handler;
    }

    [AllowAnonymous, HttpPost("register")]
    public async Task<ActionResult<IResponseModel<UserObjectModel>>> RegisterUser(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        Logger.LogDebug("RegisterUser is requested.");

        var result = await _handler.HandleAsync(request, cancellationToken);
        var response = new ResponseModel<UserObjectModel>
        {
            Message = "User registered successfully.",
            Data = result,
            Total = result != null ? 1 : 0
        };

        return new ActionResult<IResponseModel<UserObjectModel>>(response);
    }
}