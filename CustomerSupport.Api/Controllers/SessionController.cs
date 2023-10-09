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
        private readonly IAgentService _agentService;
        public SessionController( ISessionManagerService sessionManagerService, IAgentService agentService)
        {
            _sessionManagerService = sessionManagerService;
            _agentService = agentService;
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

        [HttpPost("agent-login")]
        public IActionResult AgentLogin([FromBody] string name)
        {

            var agentId = _agentService.GetAgentId(name);
            if (agentId == Guid.Empty)
            {
                return NotFound("Agent Not found");
            }

            var result = _agentService.UpdateAgentStatus(agentId);
            return Ok(result);
        }

        [HttpGet("all")]
        public IActionResult GetAllActiveSession([FromQuery] Guid agentId)
        {
            var userList = _sessionManagerService.GetActiveUser(agentId);
            return Ok(userList);
        }

    }
}
