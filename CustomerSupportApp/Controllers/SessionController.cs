using CustomerSupportApp.Model;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupportApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly Queue<UserSession> _chatSessions;
        public SessionController(Queue<UserSession> chatSessions)
        {
            _chatSessions = chatSessions;
        }

        [HttpPost("request-support")]
        public IActionResult RequestSupport([FromBody] UserSession session)
        {
            session.SessionId = Guid.NewGuid();
            session.Status = Status.Queued;
            _chatSessions.Enqueue(session);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hi");
        }
    }
}
