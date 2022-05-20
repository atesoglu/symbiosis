using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base;

[ApiController, Authorize]
public abstract class ApiControllerBase : ControllerBase
{
    protected readonly ILogger<ApiControllerBase> Logger;

    protected int UserId => Convert.ToInt32(User.Claims.First(w => w.Type == ClaimTypes.Actor).Value);
    protected string UserEmail => User.Claims.First(w => w.Type == ClaimTypes.Email).Value;

    protected ApiControllerBase(ILogger<ApiControllerBase> logger)
    {
        Logger = logger;
    }
}