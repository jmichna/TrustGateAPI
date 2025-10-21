using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController(IAuthorizationService authorizationService) : BaseController
{

    [HttpPost("Get")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    public IActionResult GetToken([FromQuery] string login, string password)
    {
        try
        {
            var token = authorizationService.GenerateToken(login, password);
            return Ok(new TokenResponse { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ErrorResponse { Error = GetUnauthorizedMessage(nameof(GetToken)) });
        }
    }

    [HttpPut("Refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public IActionResult RefresToken([FromQuery] string token)
    {
        try
        {
            var newToken = authorizationService.RefreshToken(token);
            return Ok(new TokenResponse { Token = newToken });
        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse { Error = GetBadRequestMessage(nameof(RefresToken)) });
        }
    }
}
