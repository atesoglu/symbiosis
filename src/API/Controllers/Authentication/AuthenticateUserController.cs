using API.Controllers.Base;
using Application.Flows.Authentication.Commands;
using Application.Models;
using Application.Request;
using Infrastructure.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Authentication
{
    public class AuthenticateUserController : ApiControllerBase
    {
        private readonly IRequestHandler<AuthenticateUserCommand, UserObjectModel> _handler;

        public AuthenticateUserController(IRequestHandler<AuthenticateUserCommand, UserObjectModel> handler, ILogger<ApiControllerBase> logger) : base(logger)
        {
            _handler = handler;
        }

        [AllowAnonymous, HttpPost("login")]
        public async Task<ActionResult<IResponseModel<UserObjectModel>>> AuthenticateUser(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            Logger.LogDebug("AuthenticateUser is requested.");

            var result = await _handler.HandleAsync(request, cancellationToken);
            var response = new ResponseModel<UserObjectModel>
            {
                Message = "Filtered by email{request.Email}",
                Data = result,
                Total = result != null ? 1 : 0
            };

            return new ActionResult<IResponseModel<UserObjectModel>>(response);
        }
    }
}