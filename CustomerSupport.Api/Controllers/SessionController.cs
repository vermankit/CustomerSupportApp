using CustomerSupport.Api.Common;
using CustomerSupport.Api.Model;
using CustomerSupport.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionManagerService _sessionManagerService;
        public SessionController( ISessionManagerService sessionManagerService)
        {
            _sessionManagerService = sessionManagerService;
        }

        [HttpPost("request-support")]
        public IActionResult RequestSupport([FromBody] string name)
        {

            var result = _sessionManagerService.AddUserSession(name);

            if (result == Guid.Empty)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Busy");
            }
            return Ok(result);
        }

    }
}
