using API.Controllers.Base;
using Application.Flows.Users.Queries;
using Application.Models;
using Application.Request;
using Infrastructure.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users
{
    public class FindUserByEmailController : ApiControllerBase
    {
        private readonly IRequestHandler<FindUserByEmailCommand, UserObjectModel> _handler;

        public FindUserByEmailController(IRequestHandler<FindUserByEmailCommand, UserObjectModel> handler, ILogger<ApiControllerBase> logger) : base(logger)
        {
            _handler = handler;
        }

        [HttpGet("users")]
        public async Task<ActionResult<IResponseModel<UserObjectModel>>> FindUserByEmail([FromQuery] FindUserByEmailCommand request, CancellationToken cancellationToken)
        {
            Logger.LogDebug("FindUserByEmail is requested.");

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