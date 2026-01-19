using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

public class AuthorizationController(IAuthorizationService authorizationService)
    : BaseController
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await authorizationService.GenerateTokenAsync(
                request.Login,
                request.Password
            );

            return Ok(new TokenResponse { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new ErrorResponse
            {
                Error = GetUnauthorizedMessage(nameof(Login))
            });
        }
    }

    [HttpPut("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public IActionResult RefreshToken([FromQuery] string token)
    {
        try
        {
            var newToken = authorizationService.RefreshToken(token);
            return Ok(new TokenResponse { Token = newToken });
        }
        catch (Exception)
        {
            return BadRequest(new ErrorResponse
            {
                Error = GetBadRequestMessage(nameof(RefreshToken))
            });
        }
    }
}