using Microsoft.AspNetCore.Mvc;

namespace TrustGateAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        [NonAction]
        protected string GetNotFoundMessage(string methodName)
        => $"No data found while executing {methodName}";

        [NonAction]
        protected string GetBadRequestMessage(string methodName)
            => $"Error during execution {methodName}";

        [NonAction]
        protected string GetUnauthorizedMessage(string methodName)
            => $"Unauthorized access attempt in {methodName}";

        public class TokenResponse
        {
            public string Token { get; set; } = string.Empty;
        }

        public class ErrorResponse
        {
            public string Error { get; set; } = string.Empty;
        }
    }
}
