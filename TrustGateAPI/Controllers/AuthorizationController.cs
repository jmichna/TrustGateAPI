using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers
{
    public class AuthorizationController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService) => _authorizationService = authorizationService;

        [HttpPost("Get")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        public IActionResult GetToken([FromQuery] string login, string password)
        {
            try
            {
                var token = _authorizationService.GenerateToken(login, password);
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
                var newToken = _authorizationService.RefreshToken(token);
                return Ok(new TokenResponse { Token = newToken });
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse { Error = GetBadRequestMessage(nameof(RefresToken)) });
            }
        }
    }
}
